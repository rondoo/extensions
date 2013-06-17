﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18046
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Signum.Web.Extensions.Mailing.Views
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
    
    #line 4 "..\..\Mailing\Views\Newsletter.cshtml"
    using Signum.Engine.Basics;
    
    #line default
    #line hidden
    
    #line 3 "..\..\Mailing\Views\Newsletter.cshtml"
    using Signum.Engine.DynamicQuery;
    
    #line default
    #line hidden
    using Signum.Entities;
    
    #line 1 "..\..\Mailing\Views\Newsletter.cshtml"
    using Signum.Entities.Mailing;
    
    #line default
    #line hidden
    
    #line 2 "..\..\Mailing\Views\Newsletter.cshtml"
    using Signum.Entities.Reflection;
    
    #line default
    #line hidden
    using Signum.Utilities;
    using Signum.Web;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Mailing/Views/Newsletter.cshtml")]
    public partial class Newsletter : System.Web.Mvc.WebViewPage<dynamic>
    {
        public Newsletter()
        {
        }
        public override void Execute()
        {





            
            #line 5 "..\..\Mailing\Views\Newsletter.cshtml"
Write(Html.ScriptCss("~/Mailing/Content/SF_Mailing.css"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");


            
            #line 6 "..\..\Mailing\Views\Newsletter.cshtml"
Write(Html.ScriptsJs("~/Scripts/ckeditor/ckeditor.js"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");


            
            #line 7 "..\..\Mailing\Views\Newsletter.cshtml"
Write(Html.ScriptsJs("~/Mailing/Scripts/SF_Mailing.js"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");


            
            #line 8 "..\..\Mailing\Views\Newsletter.cshtml"
 using (var nc = Html.TypeContext<NewsletterDN>())
{  

            
            #line default
            #line hidden
WriteLiteral("    <div class=\"");


            
            #line 10 "..\..\Mailing\Views\Newsletter.cshtml"
            Write(nc.Value.IsNew ? "" : "sf-tabs");

            
            #line default
            #line hidden
WriteLiteral("\">\r\n        <fieldset id=\"emTabMain\">\r\n            <legend>Newsletter</legend>\r\n " +
"           ");


            
            #line 13 "..\..\Mailing\Views\Newsletter.cshtml"
       Write(Html.ValueLine(nc, n => n.Name));

            
            #line default
            #line hidden
WriteLiteral("\r\n            ");


            
            #line 14 "..\..\Mailing\Views\Newsletter.cshtml"
       Write(Html.ValueLine(nc, n => n.State, vl => vl.ReadOnly = true));

            
            #line default
            #line hidden
WriteLiteral("     \r\n            \r\n            ");


            
            #line 16 "..\..\Mailing\Views\Newsletter.cshtml"
       Write(Html.EntityCombo(nc, n => n.SmtpConfig));

            
            #line default
            #line hidden
WriteLiteral("\r\n            ");


            
            #line 17 "..\..\Mailing\Views\Newsletter.cshtml"
       Write(Html.ValueLine(nc, n => n.From));

            
            #line default
            #line hidden
WriteLiteral("\r\n            ");


            
            #line 18 "..\..\Mailing\Views\Newsletter.cshtml"
       Write(Html.ValueLine(nc, n => n.DisplayFrom));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n            ");


            
            #line 20 "..\..\Mailing\Views\Newsletter.cshtml"
       Write(Html.EntityLine(nc, e => e.Query));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");


            
            #line 22 "..\..\Mailing\Views\Newsletter.cshtml"
             if (nc.Value.State == NewsletterState.Sent)
            {
                
            
            #line default
            #line hidden
            
            #line 24 "..\..\Mailing\Views\Newsletter.cshtml"
           Write(Html.ValueLine(nc, n => n.Subject, vl => vl.ReadOnly = true));

            
            #line default
            #line hidden
            
            #line 24 "..\..\Mailing\Views\Newsletter.cshtml"
                                                                             

            
            #line default
            #line hidden
WriteLiteral("                <fieldset>\r\n                    <legend>Message</legend>\r\n       " +
"             <div class=\"sf-email-htmlbody\">\r\n                        ");


            
            #line 28 "..\..\Mailing\Views\Newsletter.cshtml"
                   Write(Html.Raw(nc.Value.Text));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </div>\r\n                </fieldset>\r\n");


            
            #line 31 "..\..\Mailing\Views\Newsletter.cshtml"
            }
            else if (!nc.Value.IsNew)
            {   
                object queryName = QueryLogic.ToQueryName(nc.Value.Query.Key);
                var queryDescription = DynamicQueryManager.Current.QueryDescription(queryName); //To be use inside query token controls
                    

            
            #line default
            #line hidden
WriteLiteral(@"                <div class=""sf-email-replacements-container"">
                    <div class=""sf-email-replacements-token-container"">
                        <input type=""button"" class=""sf-button sf-email-togglereplacementspanel"" value=""Toggle replacements panel"" />
                        <fieldset class=""sf-email-replacements-panel"" style=""display: none;"">
                            <legend>Replacements</legend>
                            ");


            
            #line 42 "..\..\Mailing\Views\Newsletter.cshtml"
                       Write(Html.QueryTokenBuilder(null, nc, queryDescription));

            
            #line default
            #line hidden
WriteLiteral("\r\n                            <input type=\"button\" class=\"sf-button sf-email-inse" +
"rttoken\" data-prefix=\"");


            
            #line 43 "..\..\Mailing\Views\Newsletter.cshtml"
                                                                                                Write(nc.ControlID);

            
            #line default
            #line hidden
WriteLiteral("\" value=\"");


            
            #line 43 "..\..\Mailing\Views\Newsletter.cshtml"
                                                                                                                       Write(EmailTemplateViewMessage.Insert.NiceToString());

            
            #line default
            #line hidden
WriteLiteral("\" />\r\n                            <input type=\"button\" class=\"sf-button sf-email-" +
"inserttoken\" data-prefix=\"");


            
            #line 44 "..\..\Mailing\Views\Newsletter.cshtml"
                                                                                                Write(nc.ControlID);

            
            #line default
            #line hidden
WriteLiteral("\" data-block=\"if\" value=\"");


            
            #line 44 "..\..\Mailing\Views\Newsletter.cshtml"
                                                                                                                                       Write(EmailTemplateViewMessage.If.NiceToString());

            
            #line default
            #line hidden
WriteLiteral("\" />\r\n                            <input type=\"button\" class=\"sf-button sf-email-" +
"inserttoken\" data-prefix=\"");


            
            #line 45 "..\..\Mailing\Views\Newsletter.cshtml"
                                                                                                Write(nc.ControlID);

            
            #line default
            #line hidden
WriteLiteral("\" data-block=\"foreach\" value=\"");


            
            #line 45 "..\..\Mailing\Views\Newsletter.cshtml"
                                                                                                                                            Write(EmailTemplateViewMessage.Foreach.NiceToString());

            
            #line default
            #line hidden
WriteLiteral("\" />\r\n                        </fieldset>\r\n                    </div>\r\n          " +
"          ");


            
            #line 48 "..\..\Mailing\Views\Newsletter.cshtml"
               Write(Html.ValueLine(nc, e => e.Subject, vl => vl.ValueHtmlProps["class"] = "sf-email-inserttoken-target"));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    ");


            
            #line 49 "..\..\Mailing\Views\Newsletter.cshtml"
               Write(Html.ValueLine(nc, e => e.Text, vl =>
                    {
                        vl.ValueLineType = ValueLineType.TextArea;
                        vl.ValueHtmlProps["style"] = "width:100%; height:180px;";
                        vl.ValueHtmlProps["class"] = "sf-rich-text-editor";
                    }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    <script>\r\n                        $(function () {\r\n        " +
"                    SF.Mailing.initHtmlEditorWithTokens(\"");


            
            #line 57 "..\..\Mailing\Views\Newsletter.cshtml"
                                                            Write(nc.SubContext(e => e.Text).ControlID);

            
            #line default
            #line hidden
WriteLiteral("\");\r\n                        });\r\n                    </script>\r\n                " +
"</div>\r\n");


            
            #line 61 "..\..\Mailing\Views\Newsletter.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("        </fieldset>\r\n");


            
            #line 63 "..\..\Mailing\Views\Newsletter.cshtml"
         if (!nc.Value.IsNew)
        {

            
            #line default
            #line hidden
WriteLiteral("            <fieldset id=\"emTabSend\">\r\n                <legend>Deliveries</legend" +
">\r\n                <fieldset>\r\n                    <legend>Email recipients</leg" +
"end>\r\n                    ");


            
            #line 69 "..\..\Mailing\Views\Newsletter.cshtml"
               Write(Html.SearchControl(new FindOptions(typeof(NewsletterDeliveryDN))
                    {
                        FilterOptions = { new FilterOption("Newsletter", nc.Value) { Frozen = true } },
                        SearchOnLoad = true,
                    }, new Context(nc, "ncSent")));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </fieldset>\r\n            </fieldset>\r\n");


            
            #line 76 "..\..\Mailing\Views\Newsletter.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("    </div>    \r\n");


            
            #line 78 "..\..\Mailing\Views\Newsletter.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("<script>\r\n    $(function () {\r\n        SF.Mailing.initReplacements();\r\n    });\r\n<" +
"/script>\r\n");


        }
    }
}
#pragma warning restore 1591
