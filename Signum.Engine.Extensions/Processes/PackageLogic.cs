﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Signum.Engine.Maps;
using Signum.Engine.DynamicQuery;
using Signum.Entities.Processes;
using Signum.Entities;
using Signum.Engine.Basics;
using System.Reflection;
using Signum.Entities.Scheduler;
using Signum.Utilities.Reflection;
using Signum.Engine.Extensions.Properties;
using Signum.Engine.Authorization;
using Signum.Entities.Authorization;
using Signum.Engine.Exceptions;
using System.Linq.Expressions;
using Signum.Utilities;
using Signum.Entities.Basics;
using Signum.Engine.Operations;

namespace Signum.Engine.Processes
{
    public static class PackageLogic
    {
        static Expression<Func<PackageDN, IQueryable<PackageLineDN>>> LinesExpression =
            p => Database.Query<PackageLineDN>().Where(pl => pl.Package.RefersTo(p));
        public static IQueryable<PackageLineDN> Lines(this PackageDN p)
        {
            return LinesExpression.Evaluate(p);
        }

        static Expression<Func<PackageDN, IQueryable<PackageLineDN>>> RemainingLinesExpression =
            p => p.Lines().Where(a => a.FinishTime == null && a.Exception == null);
        public static IQueryable<PackageLineDN> RemainingLines(this PackageDN p)
        {
            return RemainingLinesExpression.Evaluate(p);
        }

        static Expression<Func<PackageDN, IQueryable<PackageLineDN>>> ExceptionLinesExpression =
            p => p.Lines().Where(a => a.Exception != null);
        public static IQueryable<PackageLineDN> ExceptionLines(this PackageDN p)
        {
            return ExceptionLinesExpression.Evaluate(p);
        }

        public static void AssertStarted(SchemaBuilder sb)
        {
            sb.AssertDefined(ReflectionTools.GetMethodInfo(() => Start(null, null, true, true)));
        }

        public static void Start(SchemaBuilder sb, DynamicQueryManager dqm, bool packages, bool packageOperations)
        {
            if (sb.NotDefined(MethodInfo.GetCurrentMethod()))
            {
                ProcessLogic.AssertStarted(sb);

                sb.Include<PackageLineDN>();

                if (packages)
                {
                    sb.Settings.AssertImplementedBy((PackageLineDN pl) => pl.Package, typeof(PackageDN));
                    sb.Settings.AssertImplementedBy((ProcessExecutionDN pe) => pe.ProcessData, typeof(PackageDN));

                    sb.Include<PackageDN>();
                    dqm.RegisterQuery(typeof(PackageDN), ()=>
                        from p in Database.Query<PackageDN>()
                         select new
                         {
                             Entity = p,
                             p.Id,
                             p.Name,
                             Lines = p.Lines().Count()
                         });

                }

                if (packageOperations)
                {
                    sb.Settings.AssertImplementedBy((PackageLineDN pl) => pl.Package, typeof(PackageOperationDN));
                    sb.Settings.AssertImplementedBy((ProcessExecutionDN pe) => pe.ProcessData, typeof(PackageOperationDN));


                    sb.Include<PackageOperationDN>();
                    dqm.RegisterQuery(typeof(PackageOperationDN), ()=>
                        from p in Database.Query<PackageOperationDN>()
                         select new
                         {
                             Entity = p,
                             p.Id,
                             p.Name,
                             p.Operation,
                             Lines = p.Lines().Count()
                         });

                    ProcessLogic.Register(PackageOperationProcess.PackageOperation, new PackageOperationAlgorithm());
                }

                dqm.RegisterQuery(typeof(PackageLineDN), ()=>
                    from pl in Database.Query<PackageLineDN>()
                     select new
                     {
                         Entity = pl,
                         Package = pl.Package,
                         pl.Id,
                         Target = pl.Entity,
                         pl.FinishTime,
                         pl.Exception,
                     });

                dqm.RegisterExpression((PackageDN p) => p.Lines());
                dqm.RegisterExpression((PackageDN p) => p.RemainingLines());
                dqm.RegisterExpression((PackageDN p) => p.ExceptionLines());
            }
        }

        public static PackageDN CreateLines(this PackageDN package, IEnumerable<Lite<IIdentifiable>> lites)
        {
            package.Save();

            lites.Select(lite => new PackageLineDN
            {
                Package = package.ToLite(),
                Entity = lite
            }).SaveList();

            return package;
        }

        public static ProcessExecutionDN CreatePackageOperation(IEnumerable<Lite<IIdentifiable>> entities, Enum operationKey)
        {
            return CreatePackageOperation(entities, MultiEnumLogic<OperationDN>.ToEntity((Enum)operationKey));
        }

        public static ProcessExecutionDN CreatePackageOperation(IEnumerable<Lite<IIdentifiable>> entities, OperationDN operation)
        {
            return ProcessLogic.Create(PackageOperationProcess.PackageOperation, new PackageOperationDN()
            {
                Operation = operation
            }.CreateLines(entities));
        }

        public static void RegisterUserTypeCondition(SchemaBuilder sb, Enum conditionName)
        {
            TypeConditionLogic.Register<UserProcessSessionDN>(conditionName,
               se => se.User.RefersTo(UserDN.Current));

            TypeConditionLogic.Register<ProcessExecutionDN>(conditionName,
                pe => ((UserProcessSessionDN)pe.SessionData).InCondition(conditionName));

            TypeConditionLogic.Register<PackageOperationDN>(conditionName,
                po => Database.Query<ProcessExecutionDN>().WhereCondition(conditionName).Any(pe => pe.ProcessData == po));

            TypeConditionLogic.Register<PackageLineDN>(conditionName,
                pl => ((PackageOperationDN)pl.Package.Entity).InCondition(conditionName));
        }
    }

    public class PackageOperationAlgorithm : IProcessAlgorithm
    {
        public void Execute(IExecutingProcess executingProcess)
        {
            PackageOperationDN package = (PackageOperationDN)executingProcess.Data;

            Enum operationKey = MultiEnumLogic<OperationDN>.ToEnum(package.Operation);

            executingProcess.ForEachLine(package.RemainingLines(), line =>
            {
                OperationType operationType = OperationLogic.OperationType(line.Entity.EntityType, operationKey);

                switch (operationType)
                {
                    case OperationType.Execute:
                        OperationLogic.ServiceExecuteLite(line.Entity, operationKey);
                        break;
                    case OperationType.Delete:
                        OperationLogic.ServiceDelete(line.Entity, operationKey);
                        break;
                    case OperationType.ConstructorFrom:
                        {
                            var result = OperationLogic.ServiceConstructFromLite(line.Entity, operationKey);
                            line.Result = result.ToLite();
                        }
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected operation type {0}".Formato(operationType));
                }

                line.FinishTime = TimeZoneManager.Now;
                line.Save();
            });
        }
    }

    public class PackageDeleteAlgorithm<T> : IProcessAlgorithm where T : class, IIdentifiable
    {
        public Enum OperationKey { get; private set; }

        public PackageDeleteAlgorithm(Enum operationKey)
        {
            if (operationKey == null)
                throw new ArgumentNullException("operatonKey");

            this.OperationKey = operationKey;
        }

        public virtual void Execute(IExecutingProcess executingProcess)
        {
            PackageDN package = (PackageDN)executingProcess.Data;

            executingProcess.ForEachLine(package.RemainingLines(), line =>
            {
                ((Lite<T>)line.Entity).Delete<T>(OperationKey);

                line.FinishTime = TimeZoneManager.Now;
                line.Save();
            });
        }
    }
   
    public class PackageExecuteAlgorithm<T> : IProcessAlgorithm where T : class, IIdentifiable
    {
        public Enum OperationKey { get; private set; }

        public PackageExecuteAlgorithm(Enum operationKey)
        {
            if(operationKey == null)
                throw new ArgumentNullException("operatonKey");

            this.OperationKey = operationKey;
        }

        public virtual void Execute(IExecutingProcess executingProcess)
        {
            PackageDN package = (PackageDN)executingProcess.Data;

            executingProcess.ForEachLine(package.RemainingLines(), line =>
            {
                ((Lite<T>)line.Entity).ExecuteLite(OperationKey);
                line.FinishTime = TimeZoneManager.Now;
                line.Save();
            });
        }
    }

    public class PackageConstructFromAlgorithm<F, T> : IProcessAlgorithm
        where T : class, IIdentifiable
        where F : class, IIdentifiable
    {
        public Enum OperationKey { get; private set; }

        public PackageConstructFromAlgorithm(Enum operationKey)
        {
            this.OperationKey = operationKey;
        }

        public virtual void Execute(IExecutingProcess executingProcess)
        {
            PackageDN package = (PackageDN)executingProcess.Data;

            executingProcess.ForEachLine(package.RemainingLines(), line =>
            {
                var result = line.Entity.ConstructFromLite<T>(OperationKey);
                if (result.IsNew)
                    result.Save();

                line.Result = result.ToLite();

                line.FinishTime = TimeZoneManager.Now;
                line.Save();
            });
        }
    }
}

