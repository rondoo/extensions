﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
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
    
    #line 1 "..\..\Chart\Views\ChartScriptColumn.cshtml"
    using Signum.Entities.Chart;
    
    #line default
    #line hidden
    using Signum.Utilities;
    using Signum.Web;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Chart/Views/ChartScriptColumn.cshtml")]
    public partial class _Chart_Views_ChartScriptColumn_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {
        public _Chart_Views_ChartScriptColumn_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Chart\Views\ChartScriptColumn.cshtml"
 using (var cc = Html.TypeContext<ChartScriptColumnEntity>())
{
    
            
            #line default
            #line hidden
            
            #line 4 "..\..\Chart\Views\ChartScriptColumn.cshtml"
Write(Html.ValueLine(cc, c => c.DisplayName, vl => vl.ValueColumns = new BsColumn(8)));

            
            #line default
            #line hidden
            
            #line 4 "..\..\Chart\Views\ChartScriptColumn.cshtml"
                                                                                    

    using (var sc = cc.SubContext())
    {

            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"col-sm-6\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 10 "..\..\Chart\Views\ChartScriptColumn.cshtml"
       Write(Html.ValueLine(sc, c => c.ColumnType, vl=> vl.LabelColumns = new BsColumn(4)));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n        <div");

WriteLiteral(" class=\"col-sm-3\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 13 "..\..\Chart\Views\ChartScriptColumn.cshtml"
       Write(Html.ValueLine(sc, c => c.IsGroupKey, vl=> vl.LabelColumns = new BsColumn(6)));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n        <div");

WriteLiteral(" class=\"col-sm-3\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 16 "..\..\Chart\Views\ChartScriptColumn.cshtml"
       Write(Html.ValueLine(sc, c => c.IsOptional, vl=> vl.LabelColumns = new BsColumn(6)));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n    </div>\r\n");

            
            #line 19 "..\..\Chart\Views\ChartScriptColumn.cshtml"
    }
}

            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
