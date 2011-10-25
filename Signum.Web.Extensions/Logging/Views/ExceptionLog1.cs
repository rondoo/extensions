﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASP
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using Signum.Utilities;
    using Signum.Entities;
    using Signum.Web;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel.DataAnnotations;
    using System.Configuration;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.Caching;
    using System.Web.DynamicData;
    using System.Web.SessionState;
    using System.Web.Profile;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using System.Web.UI.HtmlControls;
    using System.Xml.Linq;
    using Signum.Entities.Logging;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MvcRazorClassGenerator", "1.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Logging/Views/ExceptionLog.cshtml")]
    public class _Page_Logging_Views_ExceptionLog_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {


        public _Page_Logging_Views_ExceptionLog_cshtml()
        {
        }
        protected System.Web.HttpApplication ApplicationInstance
        {
            get
            {
                return ((System.Web.HttpApplication)(Context.ApplicationInstance));
            }
        }
        public override void Execute()
        {

WriteLiteral("\r\n");


 using (var e = Html.TypeContext<ExceptionLogDN>())
{
    
Write(Html.ValueLine(e, f => f.CreationDate));


WriteLiteral("(");


                                                         
                                                    Write(e.Value.CreationDate.ToUserInterface().ToAgoString());


WriteLiteral(")");

WriteLiteral("\r\n");



    
Write(Html.ValueLine(e, f => f.ThreadId));

                                       
    
Write(Html.ValueLine(e, f => f.RequestUrl));

                                         
    
Write(Html.ValueLine(e, f => f.UserAgent));

                                        
    
Write(Html.ValueLine(e, f => f.ActionName));

                                         
    
Write(Html.ValueLine(e, f => f.ControllerName));

                                             

    
Write(Html.EntityLine(e, f => f.User));

                                    
    
Write(Html.EntityLine(e, f => f.Context));

                                       


WriteLiteral("    <h3>");


   Write(e.Value.ExceptionType);

WriteLiteral("</h3>\r\n");



WriteLiteral("    <pre>");


    Write(e.Value.ExceptionMessage);

WriteLiteral("</pre>\r\n");



WriteLiteral("    <h3>StackTrace</h3>\r\n");



WriteLiteral("    <pre>");


    Write(e.Value.StackTrace);

WriteLiteral("</pre>\r\n");


}

        }
    }
}