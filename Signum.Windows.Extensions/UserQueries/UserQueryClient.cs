using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Signum.Windows.Reports;
using Signum.Entities;
using Signum.Services;
using System.Reflection;
using Signum.Entities.DynamicQuery;
using Signum.Entities.UserQueries;
using Signum.Windows.Authorization;
using Signum.Entities.Authorization;
using Signum.Windows.Omnibox;
using System.Windows;
using Signum.Utilities;
using System.Windows.Controls;
using Signum.Entities.Chart;
using Signum.Windows.Basics;
using Signum.Entities.Reflection;

namespace Signum.Windows.UserQueries
{
    public class UserQueryClient
    {
        public static readonly DependencyProperty UserQueryProperty =
            DependencyProperty.RegisterAttached("UserQuery", typeof(UserQueryDN), typeof(UserQueryClient), new FrameworkPropertyMetadata((s, e)=>OnUserQueryChanged(s, (UserQueryDN)e.NewValue)));
        public static UserQueryDN GetUserQuery(DependencyObject obj)
        {
            return (UserQueryDN)obj.GetValue(UserQueryProperty);
        }

        public static void SetUserQuery(DependencyObject obj, UserQueryDN value)
        {
            obj.SetValue(UserQueryProperty, value);
        }

        private static void OnUserQueryChanged(DependencyObject s, UserQueryDN uc)
        {
            UserQueryPermission.ViewUserQuery.Authorize();

            var csc = s as CountSearchControl;
            if (csc != null)
            {
                csc.QueryName = QueryClient.queryNames[uc.Query.Key];
                UserQueryClient.ToCountSearchControl(uc, csc);
                csc.Search();
                return;
            }

            var sc = s as SearchControl;
            if (sc != null && sc.ShowHeader == false)
            {
                sc.QueryName = QueryClient.queryNames[uc.Query.Key];
                UserQueryClient.ToSearchControl(uc, sc);
                sc.Search();
                return;
            }

            return;
        }

        public static void Start()
        {
            if (Navigator.Manager.NotDefined(MethodInfo.GetCurrentMethod()))
            {
                TypeClient.Start();
                QueryClient.Start();
                Navigator.AddSetting(new EntitySettings<UserQueryDN> { View = _ => new UserQuery(), Icon = ExtensionsImageLoader.GetImageSortName("userQuery.png")  });
                SearchControl.GetMenuItems += SearchControl_GetCustomMenuItems;
                UserAssetsClient.Start();
                UserAssetsClient.RegisterExportAssertLink<UserQueryDN>();

                Constructor.Register<UserQueryDN>(ctx =>
                {
                    MessageBox.Show(Window.GetWindow(ctx.Element),
                        ChartMessage._0CanOnlyBeCreatedFromTheSearchWindow.NiceToString().Formato(typeof(UserQueryDN).NicePluralName()),
                        ChartMessage.CreateNew.NiceToString(),
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return null;
                });

                LinksClient.RegisterEntityLinks<IdentifiableEntity>((entity, ctrl) =>
                {
                    if (!UserQueryPermission.ViewUserQuery.IsAuthorized())
                        return null;

                    return Server.Return((IUserQueryServer us) => us.GetUserQueriesEntity(entity.EntityType))
                        .Select(cp => new UserQueryQuickLink(cp, entity)).ToArray();
                });
            }
        }

        class UserQueryQuickLink : QuickLink
        {
            Lite<UserQueryDN> userQuery;
            Lite<IdentifiableEntity> entity;

            public UserQueryQuickLink(Lite<UserQueryDN> userQuery, Lite<IdentifiableEntity> entity)
            {
                this.ToolTip = userQuery.ToString();
                this.Label = userQuery.ToString();
                this.userQuery = userQuery;
                this.entity = entity;
                this.Icon = ExtensionsImageLoader.GetImageSortName("userQuery.png");
                this.IsVisible = true;
            }

            public override void Execute()
            {
                UserQueryClient.Explore(userQuery.Retrieve(), entity.Retrieve());
            }

            public override string Name
            {
                get { return userQuery.Key(); }
            }
        }


        static MenuItem SearchControl_GetCustomMenuItems(SearchControl seachControl)
        {
            if (!Navigator.IsViewable(typeof(UserQueryDN)))
                return null;

            return UserQueryMenuItemConstructor.Construct(seachControl);
        }

        internal static UserQueryDN FromSearchControl(SearchControl searchControl)
        {
            QueryDescription description = DynamicQueryServer.GetQueryDescription(searchControl.QueryName);

            return searchControl.GetQueryRequest(true).ToUserQuery(description, 
                QueryClient.GetQuery(searchControl.QueryName), 
                FindOptions.DefaultPagination, 
                searchControl.SimpleFilterBuilder != null);
        }

        internal static void ToSearchControl(UserQueryDN uq, SearchControl searchControl)
        {
            var filters = uq.WithoutFilters ? searchControl.FilterOptions.ToList() :
                 searchControl.FilterOptions.Where(f => f.Frozen).Concat(uq.Filters.Select(qf => new FilterOption
             {
                 Path = qf.Token.Token.FullKey(),
                 Operation = qf.Operation,
                 Value = qf.Value
             })).ToList();

            var columns = uq.Columns.Select(qc => new ColumnOption
            {
                Path = qc.Token.Token.FullKey(),
                DisplayName = qc.DisplayName
            }).ToList();

            var orders = uq.Orders.Select(of => new OrderOption
            {
                Path = of.Token.Token.FullKey(),
                OrderType = of.OrderType,
            }).ToList();

            var pagination = uq.GetPagination() ?? Navigator.GetQuerySettings(searchControl.QueryName).Pagination ?? FindOptions.DefaultPagination;

            searchControl.Reinitialize(filters, columns, uq.ColumnsMode, orders, pagination);
        }

        internal static void ToCountSearchControl(UserQueryDN uq, CountSearchControl countSearchControl)
        {
            var filters = uq.WithoutFilters ? countSearchControl.FilterOptions.ToList() :
                countSearchControl.FilterOptions.Where(f => f.Frozen).Concat(uq.Filters.Select(qf => new FilterOption
                {
                    Path = qf.Token.Token.FullKey(),
                    Operation = qf.Operation,
                    Value = qf.Value
                })).ToList();

            var columns = uq.Columns.Select(qc => new ColumnOption
            {

                Path = qc.Token.Token.FullKey(),
                DisplayName = qc.DisplayName
            }).ToList();

            var orders = uq.Orders.Select(of => new OrderOption
            {
                Path = of.Token.Token.FullKey(),
                OrderType = of.OrderType,
            }).ToList();

            countSearchControl.Reinitialize(filters, columns, uq.ColumnsMode, orders);
            countSearchControl.Text = uq.DisplayName + ": {0}";
            countSearchControl.LinkClick += (object sender, EventArgs e) =>
            {
                Navigator.Explore(new ExploreOptions(countSearchControl.QueryName)
                {
                    InitializeSearchControl = sc => UserQueryClient.SetUserQuery(sc, uq)
                });
            };
        }

        internal static void Explore(UserQueryDN userQuery, IdentifiableEntity currentEntity)
        {
            if (userQuery.EntityType != null)
            {
                if (currentEntity == null)
                {
                    var entity = Navigator.Find(new FindOptions(Server.GetType(userQuery.EntityType.ToString())));

                    if (entity == null)
                        return;

                    currentEntity = entity.Retrieve();
                }

                CurrentEntityConverter.SetFilterValues(userQuery.Filters, currentEntity);
            }

            var query = QueryClient.GetQueryName(userQuery.Query.Key);

            Navigator.Explore(new ExploreOptions(query)
            {
                InitializeSearchControl = sc => UserQueryClient.SetUserQuery(sc, userQuery)
            });
        }
    }
}
