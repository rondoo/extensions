﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34003
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Signum.Web.Extensions.Processes.Views
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using Signum.Entities;
    
    #line 1 "..\..\Processes\Views\UserProcessSession.cshtml"
    using Signum.Entities.Processes;
    
    #line default
    #line hidden
    using Signum.Utilities;
    using Signum.Web;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Processes/Views/UserProcessSession.cshtml")]
    public partial class UserProcessSession : System.Web.Mvc.WebViewPage<dynamic>
    {
        public UserProcessSession()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n");

            
            #line 3 "..\..\Processes\Views\UserProcessSession.cshtml"
 using (var e = Html.TypeContext<UserProcessSessionDN>())
{ 
    
            
            #line default
            #line hidden
            
            #line 5 "..\..\Processes\Views\UserProcessSession.cshtml"
Write(Html.EntityLine(e, f => f.User, f => f.ReadOnly = true));

            
            #line default
            #line hidden
            
            #line 5 "..\..\Processes\Views\UserProcessSession.cshtml"
                                                            
}

            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591