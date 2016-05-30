﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Signum.Engine.Authorization;
using Signum.Engine.Basics;
using Signum.Engine.Cache;
using Signum.Engine.Operations;
using Signum.Entities;
using Signum.Entities.Authorization;
using Signum.Entities.Processes;
using Signum.Utilities;
using Signum.Utilities.ExpressionTrees;
using System.Data.SqlClient;
using Signum.Engine.Maps;
using System.Linq.Expressions;
using Signum.Entities.Basics;

namespace Signum.Engine.Processes
{
    public static class ProcessRunnerLogic
    {
        static Dictionary<Lite<ProcessEntity>, ExecutingProcess> executing = new Dictionary<Lite<ProcessEntity>, ExecutingProcess>();

        static Timer timerNextExecution;
        static Timer timerPeriodic;
        public static int PoolingPeriodMilliseconds = 30 * 1000;

        internal static DateTime? nextPlannedExecution;

        public static int MaxDegreeOfParallelism = 2;

        static bool running = false;

        static int initialDelayMiliseconds;

        static CancellationTokenSource CancelNewProcesses;

        static AutoResetEvent autoResetEvent = new AutoResetEvent(false);

        public static ProcessLogicState ExecutionState()
        {
            return new ProcessLogicState
            {
                Running = running,
                InitialDelayMiliseconds = initialDelayMiliseconds,
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                NextPlannedExecution = nextPlannedExecution,
                JustMyProcesses = ProcessLogic.JustMyProcesses,
                Executing = executing.Values.Select(p => new ExecutionState
                {
                    IsCancellationRequested = p.CancelationSource.IsCancellationRequested,
                    Process = p.CurrentExecution.ToLite(),
                    State = p.CurrentExecution.State,
                    Progress = p.CurrentExecution.Progress,
                    MachineName = p.CurrentExecution.MachineName,
                    ApplicationName = p.CurrentExecution.ApplicationName,
                }).ToList()
            };
        }

        public static void StartRunningProcesses(int delayMilliseconds)
        {
            initialDelayMiliseconds = delayMilliseconds;

            if (initialDelayMiliseconds == 0)
                StartRunningProcesses();

            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(initialDelayMiliseconds);
                StartRunningProcesses();
            });
        }

        static int SetAsQueued(this IQueryable<ProcessEntity> query)
        {
            return query.UnsafeUpdate()
            .Set(p => p.State, p => ProcessState.Queued)
            .Set(p => p.QueuedDate, p => TimeZoneManager.Now)
            .Set(p => p.ExecutionStart, p => null)
            .Set(p => p.ExecutionEnd, p => null)
            .Set(p => p.SuspendDate, p => null)
            .Set(p => p.Progress, p => null)
            .Set(p => p.Exception, p => null)
            .Set(p => p.ExceptionDate, p => null)
            .Set(p => p.MachineName, p => ProcessLogic.JustMyProcesses ? Environment.MachineName : ProcessEntity.None)
            .Set(p => p.ApplicationName, p => ProcessLogic.JustMyProcesses ? Schema.Current.ApplicationName : ProcessEntity.None)
            .Execute();
        }

        internal static void SetAsQueued(this ProcessEntity process)
        {
            process.State = ProcessState.Queued;
            process.QueuedDate = TimeZoneManager.Now;
            process.ExecutionStart = null;
            process.ExecutionEnd = null;
            process.SuspendDate = null;
            process.Progress = null;
            process.Exception = null;
            process.ExceptionDate = null;
            process.MachineName = ProcessLogic.JustMyProcesses ? Environment.MachineName : ProcessEntity.None;
            process.ApplicationName = ProcessLogic.JustMyProcesses ? Schema.Current.ApplicationName : ProcessEntity.None;
        }

        static Expression<Func<ProcessEntity, bool>> IsMineExpression =
            p => p.MachineName == Environment.MachineName && p.ApplicationName == Schema.Current.ApplicationName; 
        [ExpressionField] 
        public static bool IsMine(this ProcessEntity p)
        {
            return IsMineExpression.Evaluate(p);
        }

        static Expression<Func<ProcessEntity, bool>> IsSharedExpression =
            p => !ProcessLogic.JustMyProcesses && p.MachineName == ProcessEntity.None;
        [ExpressionField]
        public static bool IsShared(this ProcessEntity p)
        {
            return IsSharedExpression.Evaluate(p);
        }

        internal static List<T> ToListWakeup<T>(this IQueryable<T> query, string action)
        {
            if (CacheLogic.WithSqlDependency)
                query.ToListWithInvalidation(typeof(ProcessEntity), action, a => WakeUp(action, a));

            return query.ToList();
        }

        public static void StartRunningProcesses()
        {
            if (running)
                throw new InvalidOperationException("ProcessLogic is running");


            Task.Factory.StartNew(() =>
            {
                var database = Schema.Current.Table(typeof(ProcessEntity)).Name.Schema?.Database; 

                using (AuthLogic.Disable())
                {
                    try
                    {
                        running = true;

                        (from p in Database.Query<ProcessEntity>()
                         where p.IsMine() && (p.State == ProcessState.Executing || p.State == ProcessState.Suspending || p.State == ProcessState.Suspended) ||
                         p.IsShared() && p.State == ProcessState.Suspended
                         select p).SetAsQueued();

                        CancelNewProcesses = new CancellationTokenSource();

                        autoResetEvent.Set();

                        timerNextExecution = new Timer(ob => WakeUp("TimerNextExecution", null), // main timer
                             null,
                             Timeout.Infinite,
                             Timeout.Infinite);

                        if (!CacheLogic.WithSqlDependency)
                            timerPeriodic = new Timer(ob => WakeUp("TimerPeriodic", null), null, PoolingPeriodMilliseconds, PoolingPeriodMilliseconds);

                        while (autoResetEvent.WaitOne())
                        {
                            if (CancelNewProcesses.IsCancellationRequested)
                                return;

                            using (HeavyProfiler.Log("PWL", () => "Process Runner"))
                            {
                                (from p in Database.Query<ProcessEntity>()
                                 where p.State == ProcessState.Planned && p.PlannedDate <= TimeZoneManager.Now
                                 select p).SetAsQueued();

                                var list = Database.Query<ProcessEntity>()
                                        .Where(p => p.IsMine() || p.IsShared())
                                        .Where(p => p.State == ProcessState.Planned)
                                        .Select(p => p.PlannedDate)
                                        .ToListWakeup("Planned dependency");

                                SetNextPannedExecution(list.Min());

                                lock (executing)
                                {
                                    int remaining = MaxDegreeOfParallelism - executing.Count;

                                    if (remaining > 0)
                                    {

                                    retry:
                                        var queued = Database.Query<ProcessEntity>()
                                            .Where(p => p.State == ProcessState.Queued)
                                            .Where(p => p.IsMine() || p.IsShared())
                                            .Select(a => new { Process = a.ToLite(), a.QueuedDate, a.MachineName })
                                            .ToListWakeup("Planned dependency");

                                        var afordable = queued
                                            .OrderByDescending(p => p.MachineName == Environment.MachineName)
                                            .OrderBy(a => a.QueuedDate)
                                            .Take(remaining).ToList();

                                        var taken = afordable.Where(p => p.MachineName == ProcessEntity.None).Select(a => a.Process).ToList();

                                        if (taken.Any())
                                        {
                                            using (Transaction tr = Transaction.ForceNew())
                                            {
                                                Database.Query<ProcessEntity>()
                                                    .Where(p => taken.Contains(p.ToLite()) && p.MachineName == ProcessEntity.None)
                                                    .UnsafeUpdate()
                                                    .Set(p => p.MachineName, p => Environment.MachineName)
                                                    .Set(p => p.ApplicationName, p => Schema.Current.ApplicationName)
                                                    .Execute();

                                                tr.Commit();
                                            }


                                            goto retry;
                                        }

                                        foreach (var pair in afordable)
                                        {
                                            ProcessEntity pro = pair.Process.Retrieve();

                                            IProcessAlgorithm algorithm = ProcessLogic.GetProcessAlgorithm(pro.Algorithm);

                                            ExecutingProcess executingProcess = new ExecutingProcess(algorithm, pro);

                                            executing.Add(pro.ToLite(), executingProcess);

                                            executingProcess.TakeForThisMachine();

                                            Task.Factory.StartNew(() =>
                                            {
                                                try
                                                {
                                                    executingProcess.Execute();
                                                }
                                                catch (Exception ex)
                                                {
                                                    try
                                                    {
                                                        ex.LogException(edn =>
                                                        {
                                                            edn.ControllerName = "ProcessWorker";
                                                            edn.ActionName = executingProcess.CurrentExecution.ToLite().Key();
                                                        });
                                                    }
                                                    catch { }
                                                }
                                                finally
                                                {
                                                    lock (executing)
                                                    {
                                                        executing.Remove(pro.ToLite());
                                                        WakeUp("Process ended", null);
                                                    }
                                                }
                                            });
                                        }

                                        var suspending = Database.Query<ProcessEntity>()
                                                .Where(p => p.State == ProcessState.Suspending)
                                                .Where(p => p.IsMine())
                                                .Select(a => a.ToLite())
                                                .ToListWakeup("Suspending dependency");

                                        foreach (var s in suspending)
                                        {
                                            ExecutingProcess execProc = executing.GetOrThrow(s);

                                            if (execProc.CurrentExecution.State != ProcessState.Finished)
                                            {
                                                execProc.CurrentExecution = s.Retrieve();
                                                execProc.CancelationSource.Cancel();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (ThreadAbortException)
                    {
                        //Ignore
                    }
                    catch (Exception e)
                    {
                        try
                        {
                            e.LogException(edn =>
                            {
                                edn.ControllerName = "ProcessWorker";
                                edn.ActionName = "MainLoop";
                            });
                        }
                        catch { }
                    }
                    finally
                    {
                        running = false;
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        internal static bool WakeUp(string reason, SqlNotificationEventArgs args)
        {
            using (HeavyProfiler.Log("WakeUp", () => "WakeUp! "+ reason + ToString(args)))
            {
                return autoResetEvent.Set();
            }
        }

        private static string ToString(SqlNotificationEventArgs args)
        {
            if (args == null)
                return null;

            return " ({0} {1} {2})".FormatWith(args.Type, args.Source, args.Info); 
        }

        private static void SetNextPannedExecution(DateTime? next)
        {
            nextPlannedExecution = next;

            if (next == null)
            {
                timerNextExecution.Change(Timeout.Infinite, Timeout.Infinite);
            }
            else
            {
                TimeSpan ts = next.Value - TimeZoneManager.Now;
                if (ts < TimeSpan.Zero)
                    ts = TimeSpan.Zero;
                else
                    ts = ts.Add(TimeSpan.FromSeconds(2));

                timerNextExecution.Change((int)ts.TotalMilliseconds, Timeout.Infinite); // invoke after the timespan
            }

        }

        public static void Stop()
        {
            if (!running)
                throw new InvalidOperationException("ProcessLogic is not running");

            timerNextExecution.Dispose();
            if (timerPeriodic != null)
                timerPeriodic.Dispose();

            CancelNewProcesses.Cancel();

            WakeUp("Stop", null);

            foreach (var p in executing.Values)
            {
                p.CancelationSource.Cancel();
            }
        }
    }


    public sealed class ExecutingProcess
    {
        public ProcessEntity CurrentExecution { get; internal set; }
        internal IProcessAlgorithm Algorithm;
        internal CancellationTokenSource CancelationSource;

        public bool WriteToConsole = false;

        public ExecutingProcess(IProcessAlgorithm processAlgorithm, ProcessEntity process)
        {
            this.CancelationSource = new CancellationTokenSource();
            this.Algorithm = processAlgorithm;
            this.CurrentExecution = process;
        }

        public IProcessDataEntity Data
        {
            get { return CurrentExecution.Data; }
        }

        public CancellationToken CancellationToken
        {
            get { return CancelationSource.Token; }
        }

        public static int DecimalPlaces = 3;

        public void ProgressChanged(int position, int count)
        {
            if (position > count)
                throw new InvalidOperationException("Position ({0}) should not be greater thant count ({1}). Maybe the process is not making progress.".FormatWith(position, count));

            decimal progress = Math.Round(((decimal)position) / count, DecimalPlaces);

            if (WriteToConsole)
                SafeConsole.WriteSameLine("{0:p} [{1}/{2}]".FormatWith(progress, position, count));

            ProgressChanged(progress);
        }

        public void ProgressChanged(decimal progress)
        {
            if (progress != CurrentExecution.Progress)
            {
                CurrentExecution.Progress = progress;
                CurrentExecution.InDB().UnsafeUpdate().Set(a => a.Progress, a => progress).Execute();
            }
        }


        public void TakeForThisMachine()
        {
            CurrentExecution.State = ProcessState.Executing;
            CurrentExecution.ExecutionStart = TimeZoneManager.Now;
            CurrentExecution.Progress = 0;
            CurrentExecution.MachineName = Environment.MachineName;
            CurrentExecution.ApplicationName = Schema.Current.ApplicationName;

            using (Transaction tr = new Transaction())
            {
                if (CurrentExecution.InDB().Any(a => a.State == ProcessState.Executing))
                    throw new InvalidOperationException("The process {0} is allready Executing!".FormatWith(CurrentExecution.Id));
                         
                using (OperationLogic.AllowSave<ProcessEntity>())
                    CurrentExecution.Save();

                tr.Commit();
            }
        }

        public void Execute()
        {
            var user = ExecutionMode.Global().Using(_ => CurrentExecution.User.Retrieve());

            using (UserHolder.UserSession(user))
            {
                using (ProcessLogic.OnApplySession(CurrentExecution))
                {
                    if (UserEntity.Current == null)
                        UserEntity.Current = AuthLogic.SystemUser;
                    try
                    {
                        Algorithm.Execute(this);

                        CurrentExecution.ExecutionEnd = TimeZoneManager.Now;
                        CurrentExecution.State = ProcessState.Finished;
                        CurrentExecution.Progress = null;
                        using (OperationLogic.AllowSave<ProcessEntity>())
                            CurrentExecution.Save();
                    }
                    catch (OperationCanceledException e)
                    {
                        if (!e.CancellationToken.Equals(this.CancellationToken))
                            throw;

                        CurrentExecution.SuspendDate = TimeZoneManager.Now;
                        CurrentExecution.State = ProcessState.Suspended;
                        using (OperationLogic.AllowSave<ProcessEntity>())
                            CurrentExecution.Save();
                    }
                    catch (Exception e)
                    {
                        if (Transaction.InTestTransaction)
                            throw;

                        CurrentExecution.State = ProcessState.Error;
                        CurrentExecution.ExceptionDate = TimeZoneManager.Now;
                        CurrentExecution.Exception = e.LogException(el => el.ActionName = CurrentExecution.Algorithm.ToString()).ToLite();
                        using (OperationLogic.AllowSave<ProcessEntity>())
                            CurrentExecution.Save();
                    }
                }
            }
        }

        public override string ToString()
        {
            return "Execution (ID = {0}): {1} ".FormatWith(CurrentExecution.Id, CurrentExecution);
        }
    }



    public class ProcessLogicState
    {
        public int MaxDegreeOfParallelism;
        public int InitialDelayMiliseconds;
        public bool Running;
        public bool JustMyProcesses;
        public DateTime? NextPlannedExecution;
        public List<ExecutionState> Executing;
    }

    public class ExecutionState
    {
        public Lite<ProcessEntity> Process;
        public ProcessState State;
        public bool IsCancellationRequested;
        public decimal? Progress;
        public string MachineName;
        public string ApplicationName; 
    }
}
