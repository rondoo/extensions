﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
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
    
    #line 1 "..\..\Processes\Views\PackageOperation.cshtml"
    using Signum.Entities.Processes;
    
    #line default
    #line hidden
    using Signum.Utilities;
    using Signum.Web;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Processes/Views/PackageOperation.cshtml")]
    public partial class PackageOperation : System.Web.Mvc.WebViewPage<dynamic>
    {
        public PackageOperation()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n");

            
            #line 3 "..\..\Processes\Views\PackageOperation.cshtml"
 using (var e = Html.TypeContext<PackageOperationDN>())
{
    
            
            #line default
            #line hidden
            
            #line 5 "..\..\Processes\Views\PackageOperation.cshtml"
Write(Html.ValueLine(e, f => f.Name));

            
            #line default
            #line hidden
            
            #line 5 "..\..\Processes\Views\PackageOperation.cshtml"
                                   
    
            
            #line default
            #line hidden
            
            #line 6 "..\..\Processes\Views\PackageOperation.cshtml"
Write(Html.EntityLine(e, f => f.Operation, f => f.ReadOnly = true));

            
            #line default
            #line hidden
            
            #line 6 "..\..\Processes\Views\PackageOperation.cshtml"
                                                                 

            
            #line default
            #line hidden
WriteLiteral("    <fieldset>\r\n        <legend>Lines</legend>\r\n");

WriteLiteral("        ");

            
            #line 9 "..\..\Processes\Views\PackageOperation.cshtml"
   Write(Html.SearchControl(new FindOptions(typeof(PackageLineDN), "Package", e.Value), new Context(e, "lines")));

            
            #line default
            #line hidden
WriteLiteral("\r\n    </fieldset>\r\n");

            
            #line 11 "..\..\Processes\Views\PackageOperation.cshtml"
}
            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
