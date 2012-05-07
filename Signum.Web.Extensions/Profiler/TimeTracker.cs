﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Signum.Utilities;
using Signum.Engine;
using Signum.Engine.Profiler;

namespace Signum.Web.Profiler
{
    public class ProfilerFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var action = filterContext.RouteData.Values["controller"] + "." + filterContext.RouteData.Values["action"];

            ViewDataDictionary viewData = filterContext.Controller.ViewData;

            viewData.Add("elapsed", TimeTracker.Start(action));

            IDisposable profiler = HeavyProfiler.Log(role: "MvcRequest", aditionalData: filterContext.HttpContext.Request.Url.PathAndQuery);
            if (profiler != null)
                viewData.Add("profiler", profiler);


            if (ProfilerLogic.SessionTimeout != null)
            {
                IDisposable sessionTimeout = Connection.CommandTimeoutScope(ProfilerLogic.SessionTimeout.Value);
                if (sessionTimeout != null)
                    viewData.Add("sessiontimeout", sessionTimeout);
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception != null)
            {
                ViewDataDictionary viewData = filterContext.Controller.ViewData;
                Dispose(viewData, "profiler");
                Dispose(viewData, "elapsed");
                Dispose(viewData, "sessiontimeout");
            }

            base.OnActionExecuted(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (filterContext.Controller.ViewData.ContainsKey("profiler"))
            {
                IDisposable viewProfiler = HeavyProfiler.Log(role: "MvcResult", aditionalData: filterContext.Result.ToString());
                if (viewProfiler != null)
                    filterContext.Controller.ViewData.Add("viewProfiler", viewProfiler);
            }
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            ViewDataDictionary viewData = filterContext.Controller.ViewData;

            Dispose(viewData, "viewProfiler");
            Dispose(viewData, "profiler");
            Dispose(viewData, "elapsed");
            Dispose(viewData, "sessiontimeout");
        }

        private void Dispose(ViewDataDictionary viewData, string key)
        {
            IDisposable elapsed = (IDisposable)viewData.TryGetC(key);
            if (elapsed != null)
            {
                elapsed.Dispose();
                viewData.Remove(key);
            }
        }
    }
}