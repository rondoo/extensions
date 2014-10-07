﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Signum.Engine.Maps;
using Signum.Engine.DynamicQuery;
using Signum.Entities.Chart;
using Signum.Engine.Basics;
using Signum.Entities.DynamicQuery;
using Signum.Entities;
using Signum.Entities.Authorization;
using Signum.Engine.Authorization;
using Signum.Engine.Operations;
using Signum.Utilities;
using Signum.Engine.UserQueries;
using Signum.Entities.Basics;
using Signum.Entities.UserQueries;
using Signum.Engine.UserAssets;
using Signum.Entities.UserAssets;
using Signum.Engine.ViewLog;
using Signum.Engine.Exceptions;

namespace Signum.Engine.Chart
{
    public static class UserChartLogic
    {
        public static ResetLazy<Dictionary<Lite<UserChartDN>, UserChartDN>> UserCharts;
        public static ResetLazy<Dictionary<Type, List<Lite<UserChartDN>>>> UserChartsByType;
        public static ResetLazy<Dictionary<object, List<Lite<UserChartDN>>>> UserChartsByQuery;

        public static void Start(SchemaBuilder sb, DynamicQueryManager dqm)
        {
            if (sb.NotDefined(MethodInfo.GetCurrentMethod()))
            {
                if (sb.Schema.Tables.ContainsKey(typeof(UserChartDN)))
                    throw new InvalidOperationException("UserChart has already been registered");

                UserAssetsImporter.UserAssetNames.Add("UserChart", typeof(UserChartDN));

                sb.Schema.Synchronizing += Schema_Synchronizing;

                sb.Include<UserChartDN>();

                dqm.RegisterQuery(typeof(UserChartDN), () =>
                    from uq in Database.Query<UserChartDN>()
                    select new
                    {
                        Entity = uq,
                        uq.Query,
                        uq.EntityType,
                        uq.Id,
                        uq.DisplayName,
                        uq.ChartScript,
                        uq.GroupResults,
                    });

                sb.Schema.EntityEvents<UserChartDN>().Retrieved += ChartLogic_Retrieved;

                new Graph<UserChartDN>.Execute(UserChartOperation.Save)
                {
                    AllowsNew = true,
                    Lite = false,
                    Execute = (uc, _) => { }
                }.Register();

                new Graph<UserChartDN>.Delete(UserChartOperation.Delete)
                {
                    Delete = (uc, _) => { uc.Delete(); }
                }.Register();

                UserCharts = sb.GlobalLazy(() => Database.Query<UserChartDN>().ToDictionary(a => a.ToLite()),
                 new InvalidateWith(typeof(UserChartDN)));

                UserChartsByQuery = sb.GlobalLazy(() => UserCharts.Value.Values.Where(a => a.EntityType == null).GroupToDictionary(a => a.Query.ToQueryName(), a => a.ToLite()),
                    new InvalidateWith(typeof(UserChartDN)));

                UserChartsByType = sb.GlobalLazy(() => UserCharts.Value.Values.Where(a => a.EntityType != null).GroupToDictionary(a => TypeLogic.IdToType.GetOrThrow(a.EntityType.Id), a => a.ToLite()),
                    new InvalidateWith(typeof(UserChartDN)));
            }
        }

        public static UserChartDN ParseData(this UserChartDN userChart)
        {
            if (!userChart.IsNew || userChart.queryName == null)
                throw new InvalidOperationException("userChart should be new and have queryName");

            userChart.Query = QueryLogic.GetQuery(userChart.queryName);

            QueryDescription description = DynamicQueryManager.Current.QueryDescription(userChart.queryName);

            userChart.ParseData(description);

            return userChart;
        }

        static void ChartLogic_Retrieved(UserChartDN userQuery)
        {
            object queryName = QueryLogic.ToQueryName(userQuery.Query.Key);

            QueryDescription description = DynamicQueryManager.Current.QueryDescription(queryName);

            foreach (var item in userQuery.Columns)
            {
                item.parentChart = userQuery;
            }

            userQuery.ParseData(description);
        }

        public static List<Lite<UserChartDN>> GetUserCharts(object queryName)
        {
            var isAllowed = Schema.Current.GetInMemoryFilter<UserChartDN>(userInterface: true);

            return UserChartsByQuery.Value.TryGetC(queryName).EmptyIfNull()
                .Where(e => isAllowed(UserCharts.Value.GetOrThrow(e))).ToList();
        }

        public static List<Lite<UserChartDN>> GetUserChartsEntity(Type entityType)
        {
            var isAllowed = Schema.Current.GetInMemoryFilter<UserChartDN>(userInterface: true);

            return UserChartsByType.Value.TryGetC(entityType).EmptyIfNull()
                .Where(e => isAllowed(UserCharts.Value.GetOrThrow(e))).ToList();
        }

        public static List<Lite<UserChartDN>> Autocomplete(string subString, int limit)
        {
            var isAllowed = Schema.Current.GetInMemoryFilter<UserChartDN>(userInterface: true);

            return UserCharts.Value.Where(a => a.Value.EntityType == null && isAllowed(a.Value))
                .Select(a => a.Key).Autocomplete(subString, limit).ToList();
        }

        public static UserChartDN RetrieveUserChart(this Lite<UserChartDN> userChart)
        {
            using (ViewLogLogic.LogView(userChart, "UserChart"))
            {
                var result = UserCharts.Value.GetOrThrow(userChart);

                var isAllowed = Schema.Current.GetInMemoryFilter<UserChartDN>(userInterface: true);
                if (!isAllowed(result))
                    throw new EntityNotFoundException(userChart.EntityType, userChart.Id);

                return result;
            }
        }

        public static void RegisterUserTypeCondition(SchemaBuilder sb, TypeConditionSymbol typeCondition)
        {
            sb.Schema.Settings.AssertImplementedBy((UserChartDN uq) => uq.Owner, typeof(UserDN));

            TypeConditionLogic.RegisterCompile<UserChartDN>(typeCondition, uq => uq.Owner.RefersTo(UserDN.Current));
        }


        public static void RegisterRoleTypeCondition(SchemaBuilder sb, TypeConditionSymbol typeCondition)
        {
            sb.Schema.Settings.AssertImplementedBy((UserChartDN uq) => uq.Owner, typeof(RoleDN));

            TypeConditionLogic.RegisterCompile<UserChartDN>(typeCondition, uq => AuthLogic.CurrentRoles().Contains(uq.Owner));
        }

        static SqlPreCommand Schema_Synchronizing(Replacements replacements)
        {
            if (!SafeConsole.IsConsolePresent)
                return null;

            var list = Database.Query<UserChartDN>().ToList();

            var table = Schema.Current.Table(typeof(UserChartDN));

            SqlPreCommand cmd = list.Select(uq => ProcessUserChart(replacements, table, uq)).Combine(Spacing.Double);

            return cmd;
        }

        static SqlPreCommand ProcessUserChart(Replacements replacements, Table table, UserChartDN uc)
        {
            try
            {
                Console.Clear();

                SafeConsole.WriteLineColor(ConsoleColor.White, "UserChart: " + uc.DisplayName);
                Console.WriteLine(" ChartScript: " + uc.ChartScript.ToString());
                Console.WriteLine(" Query: " + uc.Query.Key);

                if (uc.Filters.Any(a => a.Token.ParseException != null) ||
                   uc.Columns.Any(a => a.Token != null && a.Token.ParseException != null) ||
                   uc.Orders.Any(a => a.Token.ParseException != null))
                {
                    QueryDescription qd = DynamicQueryManager.Current.QueryDescription(uc.Query.ToQueryName());

                    SubTokensOptions canAggregate = uc.GroupResults ? SubTokensOptions.CanAggregate : 0;

                    if (uc.Filters.Any())
                    {
                        Console.WriteLine(" Filters:");
                        foreach (var item in uc.Filters.ToList())
                        {
                            QueryTokenDN token = item.Token;
                            switch (QueryTokenSynchronizer.FixToken(replacements, ref token, qd, SubTokensOptions.CanAnyAll | SubTokensOptions.CanElement | canAggregate, "{0} {1}".Formato(item.Operation, item.ValueString)))
                            {
                                case FixTokenResult.Nothing: break;
                                case FixTokenResult.DeleteEntity: return table.DeleteSqlSync(uc);
                                case FixTokenResult.RemoveToken: uc.Filters.Remove(item); break;
                                case FixTokenResult.SkipEntity: return null;
                                case FixTokenResult.Fix: item.Token = token; break;
                                default: break;
                            }
                        }
                    }

                    if (uc.Columns.Any())
                    {
                        Console.WriteLine(" Columns:");
                        foreach (var item in uc.Columns.ToList())
                        {
                            QueryTokenDN token = item.Token;
                            if (item.Token == null)
                                break;

                            switch (QueryTokenSynchronizer.FixToken(replacements, ref token, qd, SubTokensOptions.CanElement | canAggregate, item.ScriptColumn.DisplayName, allowRemoveToken: item.ScriptColumn.IsOptional))
                            {
                                case FixTokenResult.Nothing: break;
                                case FixTokenResult.DeleteEntity: return table.DeleteSqlSync(uc);
                                case FixTokenResult.RemoveToken: item.Token = null; break;
                                case FixTokenResult.SkipEntity: return null;
                                case FixTokenResult.Fix: item.Token = token; break;
                                default: break;
                            }
                        }
                    }

                    if (uc.Orders.Any())
                    {
                        Console.WriteLine(" Orders:");
                        foreach (var item in uc.Orders.ToList())
                        {
                            QueryTokenDN token = item.Token;
                            switch (QueryTokenSynchronizer.FixToken(replacements, ref token, qd, SubTokensOptions.CanElement | canAggregate, item.OrderType.ToString()))
                            {
                                case FixTokenResult.Nothing: break;
                                case FixTokenResult.DeleteEntity: return table.DeleteSqlSync(uc);
                                case FixTokenResult.RemoveToken: uc.Orders.Remove(item); break;
                                case FixTokenResult.SkipEntity: return null;
                                case FixTokenResult.Fix: item.Token = token; break;
                                default: break;
                            }
                        }
                    }
                }

                foreach (var item in uc.Filters.ToList())
                {
                    string val = item.ValueString;
                    switch (QueryTokenSynchronizer.FixValue(replacements, item.Token.Token.Type, ref val, allowRemoveToken: true, isList: item.Operation == FilterOperation.IsIn))
                    {
                        case FixTokenResult.Nothing: break;
                        case FixTokenResult.DeleteEntity: return table.DeleteSqlSync(uc);
                        case FixTokenResult.RemoveToken: uc.Filters.Remove(item); break;
                        case FixTokenResult.SkipEntity: return null;
                        case FixTokenResult.Fix: item.ValueString = val; break;
                    }
                }

                foreach (var item in uc.Columns)
                {
                    item.FixParameters();
                }


                try
                {
                    return table.UpdateSqlSync(uc, includeCollections: true);
                }
                catch(Exception e)
                {
                    Console.WriteLine("Integrity Error:");
                    SafeConsole.WriteLineColor(ConsoleColor.DarkRed, e.Message);
                    while (true)
                    {
                        SafeConsole.WriteLineColor(ConsoleColor.Yellow, "- s: Skip entity");
                        SafeConsole.WriteLineColor(ConsoleColor.Red, "- d: Delete entity");

                        string answer = Console.ReadLine();

                        if (answer == null)
                            throw new InvalidOperationException("Impossible to synchronize interactively without Console");

                        answer = answer.ToLower();

                        if (answer == "s")
                            return null;

                        if (answer == "d")
                            return table.DeleteSqlSync(uc);
                    }
                }

               
            }
            catch (Exception e)
            {
                return new SqlPreCommandSimple("-- Exception in {0}: {1}".Formato(uc.BaseToString(), e.Message));
            }
            finally
            {
                Console.Clear();
            }
        }
    }
}
