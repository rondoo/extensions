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

namespace Signum.Web.Extensions.Disconnected.Views
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
    
    #line 1 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
    using Signum.Entities.Disconnected;
    
    #line default
    #line hidden
    using Signum.Utilities;
    using Signum.Web;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Disconnected/Views/DisconnectedExport.cshtml")]
    public partial class DisconnectedExport : System.Web.Mvc.WebViewPage<dynamic>
    {
        public DisconnectedExport()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n");

            
            #line 3 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
 using (var dc = Html.TypeContext<DisconnectedExportDN>())
{
    
            
            #line default
            #line hidden
            
            #line 5 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
Write(Html.ValueLine(dc, d => d.CreationDate));

            
            #line default
            #line hidden
            
            #line 5 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
                                            
    
            
            #line default
            #line hidden
            
            #line 6 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
Write(Html.EntityLine(dc, d => d.Machine));

            
            #line default
            #line hidden
            
            #line 6 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
                                        	
    
            
            #line default
            #line hidden
            
            #line 7 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
Write(Html.ValueLine(dc, d => d.State));

            
            #line default
            #line hidden
            
            #line 7 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
                                     

    
            
            #line default
            #line hidden
            
            #line 9 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
Write(Html.ValueLine(dc, d => d.Lock));

            
            #line default
            #line hidden
            
            #line 9 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
                                    
    
            
            #line default
            #line hidden
            
            #line 10 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
Write(Html.ValueLine(dc, d => d.CreateDatabase));

            
            #line default
            #line hidden
            
            #line 10 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
                                              
    
            
            #line default
            #line hidden
            
            #line 11 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
Write(Html.ValueLine(dc, d => d.CreateSchema));

            
            #line default
            #line hidden
            
            #line 11 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
                                            
    
            
            #line default
            #line hidden
            
            #line 12 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
Write(Html.ValueLine(dc, d => d.DisableForeignKeys));

            
            #line default
            #line hidden
            
            #line 12 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
                                                  
    

            
            #line default
            #line hidden
WriteLiteral("    <fieldset>\r\n        <legend>");

            
            #line 15 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
           Write(Html.PropertyNiceName(() => dc.Value.Copies));

            
            #line default
            #line hidden
WriteLiteral("</legend>\r\n        <table>\r\n            <thead>\r\n                <tr>\r\n          " +
"          <td>");

            
            #line 19 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
                   Write(Html.PropertyNiceName((DisconnectedExportTableDN de) => de.Type));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                    <td>");

            
            #line 20 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
                   Write(Html.PropertyNiceName((DisconnectedExportTableDN de) => de.CopyTable));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                </tr>\r\n            </thead>\r\n");

            
            #line 23 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
            
            
            #line default
            #line hidden
            
            #line 23 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
             foreach (var dtc in dc.TypeElementContext(a => a.Copies))
            {
                dtc.FormGroupStyle = FormGroupStyle.None;

            
            #line default
            #line hidden
WriteLiteral("                <tr>\r\n                    <td>\r\n");

WriteLiteral("                        ");

            
            #line 28 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
                   Write(Html.EntityLine(dtc, d => d.Type));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </td>\r\n                    <td>\r\n");

WriteLiteral("                        ");

            
            #line 31 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
                   Write(Html.ValueLine(dtc, d => d.CopyTable));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </td>\r\n                </tr>\r\n");

            
            #line 34 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("        </table>\r\n    </fieldset>\r\n");

            
            #line 37 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
    
            
            #line default
            #line hidden
            
            #line 37 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
Write(Html.ValueLine(dc, d => d.EnableForeignKeys));

            
            #line default
            #line hidden
            
            #line 37 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
                                                 
    
            
            #line default
            #line hidden
            
            #line 38 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
Write(Html.ValueLine(dc, d => d.ReseedIds));

            
            #line default
            #line hidden
            
            #line 38 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
                                         
    
            
            #line default
            #line hidden
            
            #line 39 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
Write(Html.ValueLine(dc, d => d.BackupDatabase));

            
            #line default
            #line hidden
            
            #line 39 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
                                              
    
            
            #line default
            #line hidden
            
            #line 40 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
Write(Html.ValueLine(dc, d => d.DropDatabase));

            
            #line default
            #line hidden
            
            #line 40 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
                                            
    
            
            #line default
            #line hidden
            
            #line 41 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
Write(Html.EntityLine(dc, d => d.Exception));

            
            #line default
            #line hidden
            
            #line 41 "..\..\Disconnected\Views\DisconnectedExport.cshtml"
                                          
}

            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
