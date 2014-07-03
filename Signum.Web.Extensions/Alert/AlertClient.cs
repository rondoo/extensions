﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Signum.Entities.Basics;
using Signum.Utilities;
using System.Reflection;
using Signum.Entities;
using Signum.Entities.Alerts;
using Signum.Web.Operations;

namespace Signum.Web.Alerts
{
    public static class AlertClient
    {
        public static string ViewPrefix = "~/Alert/Views/{0}.cshtml";

        public static JsModule Module = new JsModule("Extensions/Signum.Web.Extensions/Alert/Scripts/Alerts");

        public static Type[] Types;

        public static void Start(params Type[] types)
        {
            if (Navigator.Manager.NotDefined(MethodInfo.GetCurrentMethod()))
            {
                Navigator.RegisterArea(typeof(AlertClient));

                Navigator.AddSettings(new List<EntitySettings>
                {
                    new EntitySettings<AlertDN> { PartialViewName = _ => ViewPrefix.Formato("Alert") },
                    new EntitySettings<AlertTypeDN> { PartialViewName = _ => ViewPrefix.Formato("AlertType") },
                });

                Types = types;

                WidgetsHelper.GetWidget += WidgetsHelper_GetWidget;

                OperationClient.AddSettings(new List<OperationSettings>
                {
                    new EntityOperationSettings(AlertOperation.CreateAlertFromEntity){ IsVisible = a => false },
                    new EntityOperationSettings(AlertOperation.SaveNew){ IsVisible = a => a.Entity.IsNew },
                    new EntityOperationSettings(AlertOperation.Save){ IsVisible = a => !a.Entity.IsNew }
                });
            }
        }

        public static IWidget WidgetsHelper_GetWidget(WidgetContext ctx)
        {
            IdentifiableEntity ie = ctx.Entity as IdentifiableEntity;
            if (ie == null || ie.IsNew)
                return null;

            if (!Types.Contains(ie.GetType()))
                return null;

            if (!Navigator.IsFindable(typeof(AlertDN)))
                return null;

            return AlertWidgetHelper.CreateWidget(ctx);
        }
    }
}