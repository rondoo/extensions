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
    
    #line 1 "..\..\Processes\Views\Process.cshtml"
    using Signum.Entities.Processes;
    
    #line default
    #line hidden
    using Signum.Utilities;
    using Signum.Web;
    
    #line 2 "..\..\Processes\Views\Process.cshtml"
    using Signum.Web.Processes;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Processes/Views/Process.cshtml")]
    public partial class Process : System.Web.Mvc.WebViewPage<dynamic>
    {
        public Process()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Processes\Views\Process.cshtml"
Write(Html.ScriptCss("~/processes/Content/Processes.css"));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

            
            #line 5 "..\..\Processes\Views\Process.cshtml"
 using (var e = Html.TypeContext<ProcessDN>())
{
    e.LabelColumns = new BsColumn(4);

            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"col-sm-6\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 10 "..\..\Processes\Views\Process.cshtml"
       Write(Html.ValueLine(e, f => f.State, f => f.ReadOnly = true));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 11 "..\..\Processes\Views\Process.cshtml"
       Write(Html.EntityLine(e, f => f.Algorithm));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 12 "..\..\Processes\Views\Process.cshtml"
       Write(Html.EntityLine(e, f => f.Data, f => f.ReadOnly = true));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 13 "..\..\Processes\Views\Process.cshtml"
       Write(Html.ValueLine(e, f => f.MachineName));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 14 "..\..\Processes\Views\Process.cshtml"
       Write(Html.ValueLine(e, f => f.ApplicationName));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n");

            
            #line 16 "..\..\Processes\Views\Process.cshtml"
        
            
            #line default
            #line hidden
            
            #line 16 "..\..\Processes\Views\Process.cshtml"
          e.LabelColumns = new BsColumn(5); 
            
            #line default
            #line hidden
WriteLiteral("\r\n        <div");

WriteLiteral(" class=\"col-sm-6\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 18 "..\..\Processes\Views\Process.cshtml"
       Write(Html.ValueLine(e, f => f.CreationDate));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 19 "..\..\Processes\Views\Process.cshtml"
       Write(Html.ValueLine(e, f => f.PlannedDate, f => { f.HideIfNull = true; f.ReadOnly = true; }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 20 "..\..\Processes\Views\Process.cshtml"
       Write(Html.ValueLine(e, f => f.CancelationDate, f => { f.HideIfNull = true; f.ReadOnly = true; }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 21 "..\..\Processes\Views\Process.cshtml"
       Write(Html.ValueLine(e, f => f.QueuedDate, f => { f.HideIfNull = true; f.ReadOnly = true; }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 22 "..\..\Processes\Views\Process.cshtml"
       Write(Html.ValueLine(e, f => f.ExecutionStart, f => { f.HideIfNull = true; f.ReadOnly = true; }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 23 "..\..\Processes\Views\Process.cshtml"
       Write(Html.ValueLine(e, f => f.ExecutionEnd, f => { f.HideIfNull = true; f.ReadOnly = true; }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 24 "..\..\Processes\Views\Process.cshtml"
       Write(Html.ValueLine(e, f => f.SuspendDate, f => { f.HideIfNull = true; f.ReadOnly = true; }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 25 "..\..\Processes\Views\Process.cshtml"
       Write(Html.ValueLine(e, f => f.ExceptionDate, f => { f.HideIfNull = true; f.ReadOnly = true; }));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n    </div>\r\n");

            
            #line 28 "..\..\Processes\Views\Process.cshtml"
          e.LabelColumns = new BsColumn(3);
    
    
            
            #line default
            #line hidden
            
            #line 30 "..\..\Processes\Views\Process.cshtml"
Write(Html.EntityLine(e, f => f.Exception, f => { f.HideIfNull = true; f.ReadOnly = true; }));

            
            #line default
            #line hidden
            
            #line 30 "..\..\Processes\Views\Process.cshtml"
                                                                                           

    

            
            #line default
            #line hidden
WriteLiteral("    <h4>");

            
            #line 33 "..\..\Processes\Views\Process.cshtml"
   Write(Html.PropertyNiceName(() => e.Value.Progress));

            
            #line default
            #line hidden
WriteLiteral("</h4>\r\n");

            
            #line 34 "..\..\Processes\Views\Process.cshtml"
    
          var val = ((double?)e.Value.Progress) * 100 ??
                ((e.Value.State == ProcessState.Queued ||
                e.Value.State == ProcessState.Suspended ||
                e.Value.State == ProcessState.Finished ||
                e.Value.State == ProcessState.Error) ? 100 : 0);

          var progressContainerClass =
              e.Value.State == ProcessState.Executing ||
              e.Value.State == ProcessState.Queued ||
              e.Value.State == ProcessState.Suspending ? "progress-striped active" : "";

          var progressClass =
              e.Value.State == ProcessState.Queued ? "progress-bar-info" :
              e.Value.State == ProcessState.Executing ? "" :
              e.Value.State == ProcessState.Finished ? "progress-bar-success" :
              e.Value.State == ProcessState.Suspending ||
              e.Value.State == ProcessState.Suspended ? "progress-bar-warning" :
              e.Value.State == ProcessState.Error ? "progress-bar-danger" :
              "";
    

            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteAttribute("class", Tuple.Create(" class=\"", 2731), Tuple.Create("\"", 2771)
, Tuple.Create(Tuple.Create("", 2739), Tuple.Create("progress", 2739), true)
            
            #line 55 "..\..\Processes\Views\Process.cshtml"
, Tuple.Create(Tuple.Create(" ", 2747), Tuple.Create<System.Object, System.Int32>(progressContainerClass
            
            #line default
            #line hidden
, 2748), false)
);

WriteLiteral(">\r\n        <div");

WriteAttribute("class", Tuple.Create(" class=\"", 2787), Tuple.Create("\"", 2822)
, Tuple.Create(Tuple.Create("", 2795), Tuple.Create("progress-bar", 2795), true)
            
            #line 56 "..\..\Processes\Views\Process.cshtml"
, Tuple.Create(Tuple.Create(" ", 2807), Tuple.Create<System.Object, System.Int32>(progressClass
            
            #line default
            #line hidden
, 2808), false)
);

WriteLiteral("  role=\"progressbar\"");

WriteLiteral(" id=\"progressBar\"");

WriteAttribute("aria-valuenow", Tuple.Create("  aria-valuenow=\"", 2860), Tuple.Create("\"", 2881)
            
            #line 56 "..\..\Processes\Views\Process.cshtml"
                      , Tuple.Create(Tuple.Create("", 2877), Tuple.Create<System.Object, System.Int32>(val
            
            #line default
            #line hidden
, 2877), false)
);

WriteLiteral(" aria-valuemin=\"0\"");

WriteLiteral(" aria-valuemax=\"100\"");

WriteAttribute("style", Tuple.Create(" style=\"", 2920), Tuple.Create("\"", 2940)
, Tuple.Create(Tuple.Create("", 2928), Tuple.Create("width:", 2928), true)
            
            #line 56 "..\..\Processes\Views\Process.cshtml"
                                                                               , Tuple.Create(Tuple.Create(" ", 2934), Tuple.Create<System.Object, System.Int32>(val
            
            #line default
            #line hidden
, 2935), false)
, Tuple.Create(Tuple.Create("", 2939), Tuple.Create("%", 2939), true)
);

WriteLiteral(">\r\n            <span");

WriteLiteral(" class=\"sr-only\"");

WriteLiteral(">45% Complete</span>\r\n        </div>\r\n    </div>\r\n");

            
            #line 60 "..\..\Processes\Views\Process.cshtml"
    
          if (e.Value.State == ProcessState.Executing)
          {

            
            #line default
            #line hidden
WriteLiteral("    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(">\r\n        $(function () {\r\n");

WriteLiteral("            ");

            
            #line 65 "..\..\Processes\Views\Process.cshtml"
        Write(ProcessClient.Module["refreshProgress"](e.Value.Id, e.Prefix, Url.Action("GetProgressExecution", "Process")));

            
            #line default
            #line hidden
WriteLiteral("\r\n        })\r\n    </script>\r\n");

            
            #line 68 "..\..\Processes\Views\Process.cshtml"
          }
          else if (e.Value.State == ProcessState.Queued)
          {

            
            #line default
            #line hidden
WriteLiteral("    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(">\r\n        $(function () {\r\n");

WriteLiteral("            ");

            
            #line 73 "..\..\Processes\Views\Process.cshtml"
        Write(ProcessClient.Module["refreshPage"](e.Prefix));

            
            #line default
            #line hidden
WriteLiteral("\r\n        })\r\n    </script>\r\n");

            
            #line 76 "..\..\Processes\Views\Process.cshtml"
          }

          e.LabelColumns = new BsColumn(3);
    
            
            #line default
            #line hidden
            
            #line 79 "..\..\Processes\Views\Process.cshtml"
Write(Html.CountSearchControl(new FindOptions(typeof(ProcessExceptionLineDN), "Process", e.Value), new Context(e, "errors"), csc =>
        {
            csc.View = true;
        }));

            
            #line default
            #line hidden
            
            #line 82 "..\..\Processes\Views\Process.cshtml"
          
}

            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
