﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Signum.Entities.Basics;
using Signum.Utilities;
using System.Reflection;
using Signum.Entities;
using Signum.Web.Operations;
using Signum.Entities.Isolation;
using Signum.Engine.Isolation;
using System.Web.Mvc;

namespace Signum.Web.Isolation
{
    public static class IsolationClient
    {
        public static string ViewPrefix = "~/Isolation/Views/{0}.cshtml";
        public static JsModule Module = new JsModule("Extensions/Signum.Web.Extensions/Isolation/Scripts/Isolation");

        public static void Start()
        {
            if (Navigator.Manager.NotDefined(MethodInfo.GetCurrentMethod()))
            {
                Navigator.RegisterArea(typeof(IsolationClient));

                WidgetsHelper.GetWidget += ctx => ctx.Entity is Entity && MixinDeclarations.IsDeclared(ctx.Entity.GetType(), typeof(IsolationMixin)) ?
                    IsolationWidgetHelper.CreateWidget(ctx) : null;

                Navigator.AddSetting(new EntitySettings<IsolationEntity> { PartialViewName = _ => ViewPrefix.FormatWith("Isolation") });
                 
                Constructor.ClientManager.GlobalPreConstructors += ctx =>
                    (!MixinDeclarations.IsDeclared(ctx.Type, typeof(IsolationMixin)) || IsolationEntity.Current != null) ? null :
                    Module["getIsolation"](ClientConstructorManager.ExtraJsonParams, ctx.Prefix,
                    IsolationMessage.SelectAnIsolation.NiceToString(),
                    GetIsolationChooserOptions(ctx.Type));

                //Unnecessary with the filter
                Constructor.Manager.PreConstructors += ctx =>
                    !MixinDeclarations.IsDeclared(ctx.Type, typeof(IsolationMixin)) ? null :
                    IsolationEntity.Override(GetIsolation(ctx.Controller.ControllerContext)); 
            }
        }

        private static IEnumerable<ChooserOption> GetIsolationChooserOptions(Type type)
        {
            var isolations = IsolationLogic.Isolations.Value.Select(iso => iso.ToChooserOption());
            if (IsolationLogic.GetStrategy(type) != IsolationStrategy.Optional)
                return isolations;

            var list = isolations.ToList();
            list.Add(new ChooserOption("", "Null"));
            return list;
        }

        public static Lite<IsolationEntity> GetIsolation(ControllerContext ctx)
        {
            var isolation = ctx.Controller.ControllerContext.HttpContext.Request["Isolation"] ??
                ctx.Controller.ControllerContext.HttpContext.Request.Headers["Isolation"];

            if (isolation.HasText())
                return Lite.Parse<IsolationEntity>(isolation);

            return null;
        }
    }

    public class IsolationFilterAttribute : ActionFilterAttribute
    {
        static string Key = "isolationDisposer";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var iso = IsolationClient.GetIsolation(filterContext.Controller.ControllerContext); 

            ViewDataDictionary viewData = filterContext.Controller.ViewData;

            IDisposable isolation = IsolationEntity.Override(iso);
            if (isolation != null)
                viewData.Add(Key, isolation);

        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception != null)
            {
                ViewDataDictionary viewData = filterContext.Controller.ViewData;
                Dispose(viewData);
            }

            base.OnActionExecuted(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            ViewDataDictionary viewData = filterContext.Controller.ViewData;

            IDisposable isolation = (IDisposable)viewData[Key];

            if (isolation == null && filterContext.Result is ViewResult)
            {
                var model = ((ViewResult)filterContext.Result).Model;

                Entity entity = (model as TypeContext).Try(tc => tc.UntypedValue as Entity) ?? model as Entity;

                if (entity != null)
                    viewData[Key] = IsolationEntity.Override(entity.TryIsolation());
            }
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            ViewDataDictionary viewData = filterContext.Controller.ViewData;
            Dispose(viewData);
        }

        void Dispose(ViewDataDictionary viewData)
        {
            IDisposable elapsed = (IDisposable)viewData.TryGetC(Key);
            if (elapsed != null)
            {
                elapsed.Dispose();
                viewData.Remove(Key);
            }
        }
    }
}