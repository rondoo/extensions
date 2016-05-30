﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Signum.Entities.DynamicQuery;
using System.Reflection;
using Signum.Utilities;
using System.Linq.Expressions;
using Signum.Utilities.DataStructures;
using System.ComponentModel;
using Signum.Utilities.Reflection;
using Signum.Entities.Basics;
using Signum.Utilities.ExpressionTrees;
using Signum.Entities.UserQueries;

namespace Signum.Entities.Chart
{
    public interface IChartBase
    {
        ChartScriptEntity ChartScript { get; }

        bool GroupResults { get; set; }

        MList<ChartColumnEntity> Columns { get; }
        MList<ChartParameterEntity> Parameters { get; }

        void InvalidateResults(bool needNewQuery);

        bool Invalidator { get; }

        void FixParameters(ChartColumnEntity chartColumnEntity);
    }

    [Serializable]
    public class ChartRequest : ModelEntity, IChartBase
    {
        public ChartRequest(object queryName)
        {
            this.queryName = queryName;
        }

        object queryName;
        [NotNullValidator]
        public object QueryName
        {
            get { return queryName; }
        }

        ChartScriptEntity chartScript;
        [NotNullValidator]
        public ChartScriptEntity ChartScript
        {
            get { return chartScript; }
            set
            {
                if (Set(ref chartScript, value))
                {
                    var newQuery = chartScript.SyncronizeColumns(this);
                    NotifyAllColumns();
                    InvalidateResults(newQuery);
                }
            }
        }

        bool groupResults = true;
        public bool GroupResults
        {
            get { return groupResults; }
            set
            {
                if (chartScript != null)
                {
                    if (chartScript.GroupBy == GroupByChart.Always && value == false)
                        return;

                    if (chartScript.GroupBy == GroupByChart.Never && value == true)
                        return;
                }

                if (Set(ref groupResults, value))
                {
                    NotifyAllColumns();
                    InvalidateResults(true);
                }
            }
        }

        [NotifyCollectionChanged, ValidateChildProperty, NotNullable]
        public MList<ChartColumnEntity> Columns { get; set; } = new MList<ChartColumnEntity>();

        [NotNullable]
        [NotNullValidator, NoRepeatValidator]
        public MList<ChartParameterEntity> Parameters { get; set; } = new MList<ChartParameterEntity>();

        void NotifyAllColumns()
        {
            foreach (var item in Columns)
            {
                item.NotifyAll();
            }
        }

        [field: NonSerialized, Ignore]
        public event Action ChartRequestChanged;

        public void InvalidateResults(bool needNewQuery)
        {
            if (needNewQuery)
                this.NeedNewQuery = true;

            if (ChartRequestChanged != null)
                ChartRequestChanged();

            Notify(() => Invalidator);
        }

        public bool Invalidator { get { return false; } }

        [NonSerialized]
        bool needNewQuery;
        [HiddenProperty]
        public bool NeedNewQuery
        {
            get { return needNewQuery; }
            set { Set(ref needNewQuery, value); }
        }


        public List<Filter> Filters { get; set; } = new List<Filter>();

        public List<Order> Orders { get; set; } = new List<Order>();

        public List<QueryToken> AllTokens()
        {
            var allTokens = Columns.Select(a => a.Token?.Token).ToList();

            if (Filters != null)
                allTokens.AddRange(Filters.Select(a => a.Token));

            if (Orders != null)
                allTokens.AddRange(Orders.Select(a => a.Token));

            return allTokens;
        }

        public List<CollectionElementToken> Multiplications
        {
            get { return CollectionElementToken.GetElements(AllTokens().ToHashSet()); }
        }

        public void CleanOrderColumns()
        {
            if (GroupResults)
            {
                var keys = this.Columns.Where(a => a.IsGroupKey.Value).Select(a => a.Token).NotNull().Select(a => a.Token).ToList();

                Orders.RemoveAll(o => !(o.Token is AggregateToken) && !keys.Contains(o.Token));
            }
            else
            {
                Orders.RemoveAll(o => o.Token is AggregateToken);
            }
        }


        public void FixParameters(ChartColumnEntity chartColumn)
        {
            ChartUtils.FixParameters(this, chartColumn);
        }
    }
}
