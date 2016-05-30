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
    
    #line 2 "..\..\Processes\Views\ProcessPanelTable.cshtml"
    using Signum.Engine.Processes;
    
    #line default
    #line hidden
    using Signum.Entities;
    
    #line 4 "..\..\Processes\Views\ProcessPanelTable.cshtml"
    using Signum.Entities.DynamicQuery;
    
    #line default
    #line hidden
    using Signum.Utilities;
    
    #line 1 "..\..\Processes\Views\ProcessPanelTable.cshtml"
    using Signum.Utilities.ExpressionTrees;
    
    #line default
    #line hidden
    using Signum.Web;
    
    #line 3 "..\..\Processes\Views\ProcessPanelTable.cshtml"
    using Signum.Web.Processes;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Processes/Views/ProcessPanelTable.cshtml")]
    public partial class _Processes_Views_ProcessPanelTable_cshtml : System.Web.Mvc.WebViewPage<ProcessLogicState>
    {
        public _Processes_Views_ProcessPanelTable_cshtml()
        {
        }
        public override void Execute()
        {
WriteLiteral("<div");

WriteLiteral(" id=\"processMainDiv\"");

WriteLiteral(">\r\n    <br />\r\n    State: <strong>\r\n");

            
            #line 9 "..\..\Processes\Views\ProcessPanelTable.cshtml"
        
            
            #line default
            #line hidden
            
            #line 9 "..\..\Processes\Views\ProcessPanelTable.cshtml"
         if (Model.Running)
        {

            
            #line default
            #line hidden
WriteLiteral("            <span");

WriteLiteral(" style=\"color: Green\"");

WriteLiteral(">RUNNING</span>\r\n");

            
            #line 12 "..\..\Processes\Views\ProcessPanelTable.cshtml"
        }
        else
        {

            
            #line default
            #line hidden
WriteLiteral("            <span");

WriteLiteral(" style=\"color: Red\"");

WriteLiteral(">STOPPED</span>\r\n");

            
            #line 16 "..\..\Processes\Views\ProcessPanelTable.cshtml"
        }
            
            #line default
            #line hidden
WriteLiteral("</strong>\r\n        <br />\r\n    JustMyProcesses: ");

            
            #line 18 "..\..\Processes\Views\ProcessPanelTable.cshtml"
                Write(Model.JustMyProcesses);

            
            #line default
            #line hidden
WriteLiteral("\r\n    <br />\r\n    MaxDegreeOfParallelism: ");

            
            #line 20 "..\..\Processes\Views\ProcessPanelTable.cshtml"
                       Write(Model.MaxDegreeOfParallelism);

            
            #line default
            #line hidden
WriteLiteral("\r\n    <br />\r\n    InitialDelayMiliseconds: ");

            
            #line 22 "..\..\Processes\Views\ProcessPanelTable.cshtml"
                        Write(Model.InitialDelayMiliseconds);

            
            #line default
            #line hidden
WriteLiteral("\r\n    <br />\r\n    NextPlannedExecution: ");

            
            #line 24 "..\..\Processes\Views\ProcessPanelTable.cshtml"
                      Write(Model.NextPlannedExecution?.ToString() ?? "-None-");

            
            #line default
            #line hidden
WriteLiteral("\r\n    <br />\r\n    <table");

WriteLiteral(" class=\"table\"");

WriteLiteral(@">
        <thead>
            <tr>
                <th>Process
                </th>
                <th>State
                </th>
                <th>Progress
                </th>
                <th>MachineName
                </th>
                <th>ApplicationName
                </th>
                <th>IsCancellationRequested
                </th>

            </tr>
        </thead>
        <tbody>
            <tr>
                <td");

WriteLiteral(" colspan=\"4\"");

WriteLiteral(">\r\n                    <b>");

            
            #line 47 "..\..\Processes\Views\ProcessPanelTable.cshtml"
                  Write(Model.Executing.Count);

            
            #line default
            #line hidden
WriteLiteral(" processes executing in ");

            
            #line 47 "..\..\Processes\Views\ProcessPanelTable.cshtml"
                                                                Write(Environment.MachineName);

            
            #line default
            #line hidden
WriteLiteral(" ()</b>\r\n                </td>\r\n            </tr>\r\n");

            
            #line 50 "..\..\Processes\Views\ProcessPanelTable.cshtml"
            
            
            #line default
            #line hidden
            
            #line 50 "..\..\Processes\Views\ProcessPanelTable.cshtml"
             foreach (var item in Model.Executing)
            {

            
            #line default
            #line hidden
WriteLiteral("                <tr>\r\n                    <td>");

            
            #line 53 "..\..\Processes\Views\ProcessPanelTable.cshtml"
                   Write(Html.LightEntityLine(item.Process, true));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </td>\r\n                    <td>");

            
            #line 55 "..\..\Processes\Views\ProcessPanelTable.cshtml"
                   Write(item.State);

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </td>\r\n                    <td>");

            
            #line 57 "..\..\Processes\Views\ProcessPanelTable.cshtml"
                   Write(item.Progress);

            
            #line default
            #line hidden
WriteLiteral("?.ToString(\"p\")\r\n                    </td>\r\n                    <td>");

            
            #line 59 "..\..\Processes\Views\ProcessPanelTable.cshtml"
                   Write(item.MachineName);

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </td>\r\n                    <td>");

            
            #line 61 "..\..\Processes\Views\ProcessPanelTable.cshtml"
                   Write(item.ApplicationName);

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </td>\r\n                    <td>");

            
            #line 63 "..\..\Processes\Views\ProcessPanelTable.cshtml"
                   Write(item.IsCancellationRequested);

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </td>\r\n\r\n                </tr>\r\n");

            
            #line 67 "..\..\Processes\Views\ProcessPanelTable.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("        </tbody>\r\n    </table>\r\n\r\n    <br />\r\n    <h2>Latest Processes</h2>\r\n\r\n");

WriteLiteral("    ");

            
            #line 74 "..\..\Processes\Views\ProcessPanelTable.cshtml"
Write(Html.SearchControl(new FindOptions(typeof(Signum.Entities.Processes.ProcessEntity))
{
    OrderOptions = { new OrderOption("CreationDate", Signum.Entities.DynamicQuery.OrderType.Descending) },
    ShowFilters = false,
    SearchOnLoad = true,
    Pagination = new Pagination.Firsts(10),
}, new Context(null, "sc")));

            
            #line default
            #line hidden
WriteLiteral("\r\n    <br />\r\n</div>\r\n");

        }
    }
}
#pragma warning restore 1591
