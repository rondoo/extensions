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

namespace Signum.Web.Extensions.Profiler.Views
{
    using System;
    using System.Collections.Generic;
    
    #line 1 "..\..\Profiler\Views\TimeTable.cshtml"
    using System.Configuration;
    
    #line default
    #line hidden
    
    #line 2 "..\..\Profiler\Views\TimeTable.cshtml"
    using System.Drawing;
    
    #line default
    #line hidden
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
    using Signum.Utilities;
    using Signum.Web;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Profiler/Views/TimeTable.cshtml")]
    public partial class TimeTable : System.Web.Mvc.WebViewPage<dynamic>
    {
        public TimeTable()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Profiler\Views\TimeTable.cshtml"
    
    ViewBag.Title = "Times table";

            
            #line default
            #line hidden
WriteLiteral("\r\n<h3>Times table</h3>\r\n");

            
            #line 7 "..\..\Profiler\Views\TimeTable.cshtml"
Write(Html.ActionLink("Clear", (Signum.Web.Profiler.ProfilerController a) => a.ClearTimesTable(), new { @class = "btn btn-default" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n<table");

WriteLiteral(" class=\"table table-nonfluid\"");

WriteLiteral(@">
    <thead>
        <tr>
            <th>
                Name
            </th>
            <th>
                Entity
            </th>
            <th>
                Executions
            </th>
            <th>
                Last Time
            </th>
            <th>
                Min
            </th>
            <th>
                Average
            </th>
            <th>
                Max
            </th>
            <th>
                total
            </th>
        </tr>
    </thead>
");

            
            #line 37 "..\..\Profiler\Views\TimeTable.cshtml"
    
            
            #line default
            #line hidden
            
            #line 37 "..\..\Profiler\Views\TimeTable.cshtml"
       var times = TimeTracker.IdentifiedElapseds;
            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 38 "..\..\Profiler\Views\TimeTable.cshtml"
    
            
            #line default
            #line hidden
            
            #line 38 "..\..\Profiler\Views\TimeTable.cshtml"
     if (times.Count > 0)
    {

        Func<float, string> getColor = f => ColorTranslator.ToHtml(ColorExtensions.Interpolate(Color.FromArgb(255, 255, 255), f, Color.FromArgb(255, 0, 0)));

        var max = new
        {
            Count = (float)times.Values.Max(a => a.Count),
            LastTime = (float)times.Values.Max(a => a.LastTime),
            MinTime = (float)times.Values.Max(a => a.MinTime),
            Average = (float)times.Values.Max(a => a.Average),
            MaxTime = (float)times.Values.Max(a => a.MaxTime),
            TotalTime = (float)times.Values.Max(a => a.TotalTime),
        };

            
            #line default
            #line hidden
WriteLiteral("        <tbody>\r\n");

            
            #line 53 "..\..\Profiler\Views\TimeTable.cshtml"
            
            
            #line default
            #line hidden
            
            #line 53 "..\..\Profiler\Views\TimeTable.cshtml"
             foreach (var pair in times)
            {

            
            #line default
            #line hidden
WriteLiteral("                <tr");

WriteLiteral(" style=\"background: #FFFFFF\"");

WriteLiteral(">\r\n                    <td>\r\n                        <span");

WriteLiteral(" class=\"processName\"");

WriteLiteral(">");

            
            #line 57 "..\..\Profiler\Views\TimeTable.cshtml"
                                              Write(pair.Key.TryBefore(' ') ?? pair.Key);

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n                    </td>\r\n                    <td>\r\n");

            
            #line 60 "..\..\Profiler\Views\TimeTable.cshtml"
                        
            
            #line default
            #line hidden
            
            #line 60 "..\..\Profiler\Views\TimeTable.cshtml"
                         if (pair.Key.TryAfter(' ') != null)
                        {
            
            #line default
            #line hidden
WriteLiteral("<span");

WriteLiteral(" class=\"entityName\"");

WriteLiteral(">");

            
            #line 61 "..\..\Profiler\Views\TimeTable.cshtml"
                                             Write(pair.Key.After(' '));

            
            #line default
            #line hidden
WriteLiteral("</span>");

            
            #line 61 "..\..\Profiler\Views\TimeTable.cshtml"
                                                                             }

            
            #line default
            #line hidden
WriteLiteral("                    </td>\r\n                    <td");

WriteAttribute("style", Tuple.Create(" style=\"", 2004), Tuple.Create("\"", 2083)
, Tuple.Create(Tuple.Create("", 2012), Tuple.Create("text-align:center;", 2012), true)
, Tuple.Create(Tuple.Create(" ", 2030), Tuple.Create("background:", 2031), true)
            
            #line 63 "..\..\Profiler\Views\TimeTable.cshtml"
, Tuple.Create(Tuple.Create("", 2042), Tuple.Create<System.Object, System.Int32>(getColor(pair.Value.Count / max.Count)
            
            #line default
            #line hidden
, 2042), false)
, Tuple.Create(Tuple.Create("", 2081), Tuple.Create(";", 2081), true)
, Tuple.Create(Tuple.Create(" ", 2082), Tuple.Create("", 2082), true)
);

WriteLiteral(">");

            
            #line 63 "..\..\Profiler\Views\TimeTable.cshtml"
                                                                                                   Write(pair.Value.Count);

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </td>\r\n                    <td");

WriteAttribute("style", Tuple.Create(" style=\"", 2154), Tuple.Create("\"", 2234)
, Tuple.Create(Tuple.Create("", 2162), Tuple.Create("text-align:right;", 2162), true)
, Tuple.Create(Tuple.Create(" ", 2179), Tuple.Create("background:", 2180), true)
            
            #line 65 "..\..\Profiler\Views\TimeTable.cshtml"
, Tuple.Create(Tuple.Create("", 2191), Tuple.Create<System.Object, System.Int32>(getColor(pair.Value.LastTime/max.LastTime)
            
            #line default
            #line hidden
, 2191), false)
);

WriteLiteral(">");

            
            #line 65 "..\..\Profiler\Views\TimeTable.cshtml"
                                                                                                    Write(pair.Value.LastTime);

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </td>\r\n                    <td");

WriteAttribute("style", Tuple.Create(" style=\"", 2308), Tuple.Create("\"", 2388)
, Tuple.Create(Tuple.Create("", 2316), Tuple.Create("text-align:right;", 2316), true)
, Tuple.Create(Tuple.Create(" ", 2333), Tuple.Create("background:", 2334), true)
            
            #line 67 "..\..\Profiler\Views\TimeTable.cshtml"
, Tuple.Create(Tuple.Create("", 2345), Tuple.Create<System.Object, System.Int32>(getColor(pair.Value.MinTime / max.MinTime)
            
            #line default
            #line hidden
, 2345), false)
);

WriteLiteral(">");

            
            #line 67 "..\..\Profiler\Views\TimeTable.cshtml"
                                                                                                    Write(pair.Value.MinTime);

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </td>\r\n                    <td");

WriteAttribute("style", Tuple.Create(" style=\"", 2461), Tuple.Create("\"", 2548)
, Tuple.Create(Tuple.Create("", 2469), Tuple.Create("text-align:right;", 2469), true)
, Tuple.Create(Tuple.Create(" ", 2486), Tuple.Create("background:", 2487), true)
            
            #line 69 "..\..\Profiler\Views\TimeTable.cshtml"
, Tuple.Create(Tuple.Create("", 2498), Tuple.Create<System.Object, System.Int32>(getColor((float)pair.Value.Average / max.Average)
            
            #line default
            #line hidden
, 2498), false)
);

WriteLiteral(">");

            
            #line 69 "..\..\Profiler\Views\TimeTable.cshtml"
                                                                                                           Write(pair.Value.Average);

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </td>\r\n                    <td");

WriteAttribute("style", Tuple.Create(" style=\"", 2621), Tuple.Create("\"", 2683)
, Tuple.Create(Tuple.Create("", 2629), Tuple.Create("background:", 2629), true)
            
            #line 71 "..\..\Profiler\Views\TimeTable.cshtml"
, Tuple.Create(Tuple.Create("", 2640), Tuple.Create<System.Object, System.Int32>(getColor(pair.Value.MaxTime / max.MaxTime)
            
            #line default
            #line hidden
, 2640), false)
);

WriteLiteral(">");

            
            #line 71 "..\..\Profiler\Views\TimeTable.cshtml"
                                                                                  Write(pair.Value.MaxTime);

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </td>\r\n                    <td");

WriteAttribute("style", Tuple.Create(" style=\"", 2756), Tuple.Create("\"", 2840)
, Tuple.Create(Tuple.Create("", 2764), Tuple.Create("text-align:right;", 2764), true)
, Tuple.Create(Tuple.Create(" ", 2781), Tuple.Create("background:", 2782), true)
            
            #line 73 "..\..\Profiler\Views\TimeTable.cshtml"
, Tuple.Create(Tuple.Create("", 2793), Tuple.Create<System.Object, System.Int32>(getColor(pair.Value.TotalTime / max.TotalTime)
            
            #line default
            #line hidden
, 2793), false)
);

WriteLiteral(">");

            
            #line 73 "..\..\Profiler\Views\TimeTable.cshtml"
                                                                                                        Write(pair.Value.TotalTime);

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </td>\r\n                </tr>\r\n");

            
            #line 76 "..\..\Profiler\Views\TimeTable.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("        </tbody>\r\n");

            
            #line 78 "..\..\Profiler\Views\TimeTable.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("</table>\r\n");

        }
    }
}
#pragma warning restore 1591
