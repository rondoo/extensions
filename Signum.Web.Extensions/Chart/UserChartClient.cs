using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using Signum.Entities.DynamicQuery;
using Signum.Utilities;
using Signum.Engine.Basics;
using Signum.Entities.Chart;
using Signum.Engine.DynamicQuery;
using Signum.Entities.UserQueries;
using Signum.Entities;
using Signum.Web.Reports;
using Signum.Entities.Authorization;
using Signum.Engine.Authorization;
using Signum.Engine;
using System.Web.Mvc;
using System.Web.Routing;
using Signum.Engine.Chart;
using Signum.Web.Basic;
using Signum.Web.Operations;
using Signum.Web.Extensions.UserQueries;

namespace Signum.Web.Chart
{
    public static class UserChartClient
    {
        public static void Start()
        {
            if (Navigator.Manager.NotDefined(MethodInfo.GetCurrentMethod()))
            {
                QueryClient.Start();

                UserAssetsClient.Start();
                UserAssetsClient.RegisterExportAssertLink<UserChartDN>();

                Mapping<QueryTokenDN> qtMapping = ctx =>
                {
                    string tokenStr = UserQueries.UserQueriesHelper.GetTokenString(ctx);

                    string queryKey = ctx.Parent.Parent.Parent.Inputs[TypeContextUtilities.Compose("Query", "Key")];
                    object queryName = QueryLogic.ToQueryName(queryKey);

                    var chart = ((UserChartDN)ctx.Parent.Parent.Parent.UntypedValue);

                    QueryDescription qd = DynamicQueryManager.Current.QueryDescription(queryName);
                    return new QueryTokenDN(QueryUtils.Parse(tokenStr, qd, canAggregate: chart.GroupResults));
                };

                Navigator.AddSettings(new List<EntitySettings>
                {
                    new EntitySettings<UserChartDN> 
                    { 
                        PartialViewName = _ => ChartClient.ViewPrefix.Formato("UserChart"),
                        MappingMain = new EntityMapping<UserChartDN>(true)
                        
                            .SetProperty(cb=>cb.Columns, new ChartClient.MListCorrelatedOrDefaultMapping<ChartColumnDN>(ChartClient.MappingChartColumn))
                            .SetProperty(cr => cr.Filters, new MListMapping<QueryFilterDN>
                            {
                                ElementMapping = new EntityMapping<QueryFilterDN>(false)
                                    .CreateProperty(a=>a.Operation)
                                    .CreateProperty(a=>a.ValueString)
                                    .SetProperty(a=>a.Token, qtMapping)
                            })
                            .SetProperty(cr => cr.Orders, new MListMapping<QueryOrderDN>
                            {
                                ElementMapping = new EntityMapping<QueryOrderDN>(false)
                                    .CreateProperty(a=>a.OrderType)
                                    .SetProperty(a=>a.Token, qtMapping)
                            })
                    }
                });

                RouteTable.Routes.MapRoute(null, "UC/{webQueryName}/{lite}",
                     new { controller = "Chart", action = "ViewUserChart" });

                UserChartDN.SetConverters(query => QueryLogic.ToQueryName(query.Key), queryName =>
                    QueryLogic.GetQuery(queryName));

                OperationClient.AddSetting(new EntityOperationSettings(UserChartOperation.Delete)
                {
                    OnClick = ctx => ChartClient.Module["deleteUserChart"](ctx.Options(), Navigator.FindRoute(((UserChartDN)ctx.Entity).Query.ToQueryName()))
                });

                LinksClient.RegisterEntityLinks<IdentifiableEntity>((entity, ctrl) =>
                {
                    if (!ChartPermission.ViewCharting.IsAuthorized())
                        return null;

                    return UserChartLogic.GetUserChartsEntity(entity.EntityType)
                        .Select(cp => new UserChartQuickLink(cp, entity)).ToArray();
                });

            }
        }

        class UserChartQuickLink : QuickLink
        {
            Lite<UserChartDN> userChart;
            Lite<IdentifiableEntity> entity;

            public UserChartQuickLink(Lite<UserChartDN> userChart, Lite<IdentifiableEntity> entity)
            {
                this.Text = userChart.ToString();
                this.userChart = userChart;
                this.entity = entity;
                this.IsVisible = true;
            }

            public override MvcHtmlString Execute()
            {
                return new HtmlTag("a").Attr("href", RouteHelper.New().Action((ChartController c) => c.ViewUserChart(userChart, entity))).SetInnerText(Text);
            }
        }

        public static List<ToolBarButton> GetChartMenu(ControllerContext controllerContext, UrlHelper url, object queryName, Type entityType, string prefix, Lite<UserChartDN> currentUserChart)
        {
            if (!Navigator.IsNavigable(typeof(UserChartDN), null, isSearch: true))
                return new List<ToolBarButton>();

            var buttons = new List<ToolBarButton>();
            buttons.Add(UserCharButton(url, queryName, prefix, currentUserChart));

            if (ReportSpreadsheetClient.ToExcelPlain)
                buttons.Add(ReportSpreadsheetClient.UserChartButton(url, prefix)); 

            return buttons;
        }

        private static ToolBarButton UserCharButton(UrlHelper url, object queryName, string prefix, Lite<UserChartDN> currentUserChart)
        {

            var items = new List<IMenuItem>();

            foreach (var uc in UserChartLogic.GetUserCharts(queryName))
            {
                items.Add(new MenuItem
                {
                    Text = uc.ToString(),
                    Title = uc.ToString(),
                    Href = RouteHelper.New().Action<ChartController>(c => c.ViewUserChart(uc, null)),
                    CssClass = (currentUserChart.Is(uc) ? " sf-userchart-selected" : "")
                });
            }

            if (items.Count > 0)
                items.Add(new MenuItemSeparator());

            if (Navigator.IsCreable(typeof(UserChartDN), isSearch: true))
            {
                string uqNewText = ChartMessage.CreateNew.NiceToString();
                items.Add(new MenuItem
                {
                    Id = TypeContextUtilities.Compose(prefix, "qbUserChartNew"),
                    Title = uqNewText,
                    Text = uqNewText,
                    OnClick = ChartClient.Module["createUserChart"](prefix, url.Action((ChartController c) => c.CreateUserChart())),
                });
            }

            if (currentUserChart != null && currentUserChart.IsAllowedFor(TypeAllowedBasic.Modify, inUserInterface: true))
            {
                string ucEditText = ChartMessage.EditUserChart.NiceToString();
                items.Add(new MenuItem
                {
                    Id = TypeContextUtilities.Compose(prefix, "qbUserChartEdit"),
                    Title = ucEditText,
                    Text = ucEditText,
                    Href = Navigator.NavigateRoute(currentUserChart)
                });
            }

            string ucUserChartText = typeof(UserChartDN).NicePluralName();

            return new ToolBarDropDown
            {
                Id = TypeContextUtilities.Compose(prefix, "tmUserCharts"),
                Title = ucUserChartText,
                Text = ucUserChartText,
                Items = items
            };
        }
    }
}