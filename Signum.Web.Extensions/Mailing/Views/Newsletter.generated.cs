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
    
    #line 4 "..\..\Mailing\Views\Newsletter.cshtml"
    using Signum.Engine.Basics;
    
    #line default
    #line hidden
    
    #line 3 "..\..\Mailing\Views\Newsletter.cshtml"
    using Signum.Engine.DynamicQuery;
    
    #line default
    #line hidden
    using Signum.Entities;
    
    #line 6 "..\..\Mailing\Views\Newsletter.cshtml"
    using Signum.Entities.DynamicQuery;
    
    #line default
    #line hidden
    
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
    
    #line 5 "..\..\Mailing\Views\Newsletter.cshtml"
    using Signum.Web.Mailing;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Mailing/Views/Newsletter.cshtml")]
    public partial class _Mailing_Views_Newsletter_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {
        public _Mailing_Views_Newsletter_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 7 "..\..\Mailing\Views\Newsletter.cshtml"
Write(Html.ScriptCss("~/Mailing/Content/Mailing.css"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 8 "..\..\Mailing\Views\Newsletter.cshtml"
 using (var nc = Html.TypeContext<NewsletterEntity>())
{  
    using(var tabs = Html.Tabs(nc)) 
    {
        tabs.Tab("emTabMain", typeof(NewsletterEntity).NiceName(), 
            
            #line default
            #line hidden
item => new System.Web.WebPages.HelperResult(__razor_template_writer => {

WriteLiteralTo(__razor_template_writer, "\r\n");

WriteLiteralTo(__razor_template_writer, "    ");

            
            #line 13 "..\..\Mailing\Views\Newsletter.cshtml"
WriteTo(__razor_template_writer, Html.ValueLine(nc, n => n.Name));

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "\r\n");

WriteLiteralTo(__razor_template_writer, "    ");

            
            #line 14 "..\..\Mailing\Views\Newsletter.cshtml"
WriteTo(__razor_template_writer, Html.ValueLine(nc, n => n.State, vl => vl.ReadOnly = true));

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "\r\n\r\n");

WriteLiteralTo(__razor_template_writer, "    ");

            
            #line 16 "..\..\Mailing\Views\Newsletter.cshtml"
WriteTo(__razor_template_writer, Html.ValueLine(nc, n => n.From));

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "\r\n");

WriteLiteralTo(__razor_template_writer, "    ");

            
            #line 17 "..\..\Mailing\Views\Newsletter.cshtml"
WriteTo(__razor_template_writer, Html.ValueLine(nc, n => n.DisplayFrom));

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "\r\n\r\n");

WriteLiteralTo(__razor_template_writer, "    ");

            
            #line 19 "..\..\Mailing\Views\Newsletter.cshtml"
WriteTo(__razor_template_writer, Html.EntityLine(nc, e => e.Query));

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "\r\n\r\n");

            
            #line 21 "..\..\Mailing\Views\Newsletter.cshtml"
    
            
            #line default
            #line hidden
            
            #line 21 "..\..\Mailing\Views\Newsletter.cshtml"
     if (nc.Value.State == NewsletterState.Sent)
            {
        
            
            #line default
            #line hidden
            
            #line 23 "..\..\Mailing\Views\Newsletter.cshtml"
WriteTo(__razor_template_writer, Html.ValueLine(nc, n => n.Subject, vl => vl.ReadOnly = true));

            
            #line default
            #line hidden
            
            #line 23 "..\..\Mailing\Views\Newsletter.cshtml"
                                                                     

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "        <fieldset>\r\n            <legend>Message</legend>\r\n            <div");

WriteLiteralTo(__razor_template_writer, " class=\"sf-email-htmlbody\"");

WriteLiteralTo(__razor_template_writer, ">\r\n");

WriteLiteralTo(__razor_template_writer, "                ");

            
            #line 27 "..\..\Mailing\Views\Newsletter.cshtml"
WriteTo(__razor_template_writer, Html.Raw(nc.Value.Text));

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "\r\n            </div>\r\n        </fieldset>\r\n");

            
            #line 30 "..\..\Mailing\Views\Newsletter.cshtml"
            }
            else if (nc.Value.IsNew)
            {

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "        <div");

WriteLiteralTo(__razor_template_writer, " style=\"display: none\"");

WriteLiteralTo(__razor_template_writer, ">\r\n");

WriteLiteralTo(__razor_template_writer, "            ");

            
            #line 34 "..\..\Mailing\Views\Newsletter.cshtml"
WriteTo(__razor_template_writer, Html.ValueLine(nc, e => e.Subject));

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "\r\n");

WriteLiteralTo(__razor_template_writer, "            ");

            
            #line 35 "..\..\Mailing\Views\Newsletter.cshtml"
WriteTo(__razor_template_writer, Html.ValueLine(nc, e => e.Text, vl => vl.ValueLineType = ValueLineType.TextArea));

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "\r\n        </div>\r\n");

            
            #line 37 "..\..\Mailing\Views\Newsletter.cshtml"
            }
            else
            {
                object queryName = QueryLogic.ToQueryName(nc.Value.Query.Key);
                var queryDescription = DynamicQueryManager.Current.QueryDescription(queryName); //To be use inside query token controls
                    

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "        <div");

WriteLiteralTo(__razor_template_writer, " class=\"sf-email-replacements-container\"");

WriteLiteralTo(__razor_template_writer, ">\r\n            <div");

WriteLiteralTo(__razor_template_writer, " class=\"sf-template-message-insert-container\"");

WriteLiteralTo(__razor_template_writer, ">\r\n                <fieldset");

WriteLiteralTo(__razor_template_writer, " class=\"sf-email-replacements-panel\"");

WriteLiteralTo(__razor_template_writer, ">\r\n                    <legend>Replacements</legend>\r\n");

            
            #line 47 "..\..\Mailing\Views\Newsletter.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 47 "..\..\Mailing\Views\Newsletter.cshtml"
                     using (var sc = new Context(nc, "qtb"))
                    {
                        
            
            #line default
            #line hidden
            
            #line 49 "..\..\Mailing\Views\Newsletter.cshtml"
WriteTo(__razor_template_writer, Html.QueryTokenBuilder(null, sc, MailingClient.GetQueryTokenBuilderSettings(queryDescription, SubTokensOptions.CanAnyAll | SubTokensOptions.CanElement)));

            
            #line default
            #line hidden
            
            #line 49 "..\..\Mailing\Views\Newsletter.cshtml"
                                                                                                                                                                                 

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "                        <input");

WriteLiteralTo(__razor_template_writer, " type=\"button\"");

WriteLiteralTo(__razor_template_writer, " disabled=\"disabled\"");

WriteLiteralTo(__razor_template_writer, " class=\"btn btn-default btn-sm sf-button sf-email-inserttoken sf-email-inserttoke" +
"n-basic\"");

WriteLiteralTo(__razor_template_writer, " data-prefix=\"");

            
            #line 50 "..\..\Mailing\Views\Newsletter.cshtml"
                                                                                                                                       WriteTo(__razor_template_writer, sc.Prefix);

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "\"");

WriteAttributeTo(__razor_template_writer, "value", Tuple.Create(" value=\"", 2209), Tuple.Create("\"", 2266)
            
            #line 50 "..\..\Mailing\Views\Newsletter.cshtml"
                                                                                                           , Tuple.Create(Tuple.Create("", 2217), Tuple.Create<System.Object, System.Int32>(EmailTemplateViewMessage.Insert.NiceToString()
            
            #line default
            #line hidden
, 2217), false)
);

WriteLiteralTo(__razor_template_writer, " />\r\n");

WriteLiteralTo(__razor_template_writer, "                        <input");

WriteLiteralTo(__razor_template_writer, " type=\"button\"");

WriteLiteralTo(__razor_template_writer, " disabled=\"disabled\"");

WriteLiteralTo(__razor_template_writer, " class=\"btn btn-default btn-sm sf-button sf-email-inserttoken sf-email-inserttoke" +
"n-if\"");

WriteLiteralTo(__razor_template_writer, " data-prefix=\"");

            
            #line 51 "..\..\Mailing\Views\Newsletter.cshtml"
                                                                                                                                    WriteTo(__razor_template_writer, sc.Prefix);

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "\"");

WriteLiteralTo(__razor_template_writer, " data-block=\"if\"");

WriteLiteralTo(__razor_template_writer, " value=\"if\"");

WriteLiteralTo(__razor_template_writer, " />\r\n");

WriteLiteralTo(__razor_template_writer, "                        <input");

WriteLiteralTo(__razor_template_writer, " type=\"button\"");

WriteLiteralTo(__razor_template_writer, " disabled=\"disabled\"");

WriteLiteralTo(__razor_template_writer, " class=\"btn btn-default btn-sm sf-button sf-email-inserttoken sf-email-inserttoke" +
"n-foreach\"");

WriteLiteralTo(__razor_template_writer, " data-prefix=\"");

            
            #line 52 "..\..\Mailing\Views\Newsletter.cshtml"
                                                                                                                                         WriteTo(__razor_template_writer, sc.Prefix);

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "\"");

WriteLiteralTo(__razor_template_writer, " data-block=\"foreach\"");

WriteLiteralTo(__razor_template_writer, " value=\"foreach\"");

WriteLiteralTo(__razor_template_writer, " />\r\n");

WriteLiteralTo(__razor_template_writer, "                        <input");

WriteLiteralTo(__razor_template_writer, " type=\"button\"");

WriteLiteralTo(__razor_template_writer, " disabled=\"disabled\"");

WriteLiteralTo(__razor_template_writer, " class=\"btn btn-default btn-sm sf-button sf-email-inserttoken sf-email-inserttoke" +
"n-any\"");

WriteLiteralTo(__razor_template_writer, " data-prefix=\"");

            
            #line 53 "..\..\Mailing\Views\Newsletter.cshtml"
                                                                                                                                     WriteTo(__razor_template_writer, sc.Prefix);

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "\"");

WriteLiteralTo(__razor_template_writer, " data-block=\"any\"");

WriteLiteralTo(__razor_template_writer, " value=\"any\"");

WriteLiteralTo(__razor_template_writer, " />\r\n");

            
            #line 54 "..\..\Mailing\Views\Newsletter.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "                </fieldset>\r\n            </div>\r\n");

WriteLiteralTo(__razor_template_writer, "            ");

            
            #line 57 "..\..\Mailing\Views\Newsletter.cshtml"
WriteTo(__razor_template_writer, Html.ValueLine(nc, e => e.Subject, vl => vl.ValueHtmlProps["class"] = "sf-email-inserttoken-target"));

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "\r\n");

WriteLiteralTo(__razor_template_writer, "            ");

            
            #line 58 "..\..\Mailing\Views\Newsletter.cshtml"
WriteTo(__razor_template_writer, Html.ValueLine(nc, e => e.Text, vl =>
                    {
                        vl.ValueLineType = ValueLineType.TextArea;
                        vl.ValueHtmlProps["style"] = "width:100%; height:180px;";
                        vl.ValueHtmlProps["class"] = "sf-rich-text-editor";
                    }));

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "\r\n            <script");

WriteLiteralTo(__razor_template_writer, " type=\"text/javascript\"");

WriteLiteralTo(__razor_template_writer, ">\r\n                 $(function () {\r\n");

WriteLiteralTo(__razor_template_writer, "                    ");

            
            #line 66 "..\..\Mailing\Views\Newsletter.cshtml"
WriteTo(__razor_template_writer, MailingClient.Module["initHtmlEditorWithTokens"](nc.SubContext(e => e.Text).Prefix, UICulture));

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, ";\r\n                });\r\n            </script>\r\n        </div>\r\n");

            
            #line 70 "..\..\Mailing\Views\Newsletter.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "    ");

})
            
            #line 71 "..\..\Mailing\Views\Newsletter.cshtml"
           );
        if (!nc.Value.IsNew)
        {
            tabs.Tab("emTabSend", typeof(NewsletterDeliveryEntity).NiceName(), Html.SearchControl(new FindOptions(typeof(NewsletterDeliveryEntity))
               {
                   FilterOptions = { new FilterOption("Newsletter", nc.Value) { Frozen = true } },
                   SearchOnLoad = true,
               }, new Context(nc, "ncSent")));
        }  
    }
    

            
            #line default
            #line hidden
WriteLiteral("<script>\r\n     $(function () {\r\n");

WriteLiteral("        ");

            
            #line 84 "..\..\Mailing\Views\Newsletter.cshtml"
    Write(MailingClient.Module["initReplacements"]());

            
            #line default
            #line hidden
WriteLiteral(";\r\n    });\r\n</script>\r\n");

            
            #line 87 "..\..\Mailing\Views\Newsletter.cshtml"
}

            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
