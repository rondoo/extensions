﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18010
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Signum.Web.Extensions.Calendar.Views
{
    using System;
    using System.Collections.Generic;
    
    #line 1 "..\..\Calendar\Views\Calendar.cshtml"
    using System.Globalization;
    
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.5.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Calendar/Views/Calendar.cshtml")]
    public class Calendar : System.Web.Mvc.WebViewPage<dynamic>
    {
        public Calendar()
        {
        }
        public override void Execute()
        {

WriteLiteral("\r\n");


            
            #line 3 "..\..\Calendar\Views\Calendar.cshtml"
Write(Html.ScriptCss("~/calendar/content/calendar.css"));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");


            
            #line 5 "..\..\Calendar\Views\Calendar.cshtml"
  
    DateTime startDate = ViewBag.StartDate;

    int daysToMove = ((int)startDate.DayOfWeek + 6) % 7;
    startDate = startDate.AddDays(-daysToMove);
    
    DateTime endDate = ViewBag.EndDate;
    Func<DateTime, int, HelperResult> cellTemplate = ViewBag.CellTemplate;


            
            #line default
            #line hidden
WriteLiteral("\r\n");


            
            #line 15 "..\..\Calendar\Views\Calendar.cshtml"
 if (!ViewData.ContainsKey("ShowSlider") || ViewBag.ShowSlider)
{ 

            
            #line default
            #line hidden
WriteLiteral("    <div class=\"sf-annual-calendar-slider\">\r\n    </div>\r\n");


            
            #line 19 "..\..\Calendar\Views\Calendar.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral(@"
<table class=""sf-annual-calendar"">
    <thead>
        <tr>
            <th></th>
            <th>L</th>
            <th>M</th>
            <th>X</th>
            <th>J</th>
            <th>V</th>
            <th>S</th>
            <th>D</th>
        </tr>
    </thead>
    <tbody>
");


            
            #line 35 "..\..\Calendar\Views\Calendar.cshtml"
           int max = (int)(endDate - startDate).TotalDays; 

            
            #line default
            #line hidden

            
            #line 36 "..\..\Calendar\Views\Calendar.cshtml"
         for (int d = 0; d < max; d++)
        {

            
            #line default
            #line hidden
WriteLiteral("            <tr>\r\n                <td class=\"sf-annual-calendar-month\">\r\n");


            
            #line 40 "..\..\Calendar\Views\Calendar.cshtml"
                      
            string month = CultureInfo.CurrentUICulture.DateTimeFormat.AbbreviatedMonthNames[startDate.AddDays(d).Month - 1];
            if (startDate.AddDays(d).Month != startDate.AddDays(d + 6).Month)
            {
                month += " " + CultureInfo.CurrentUICulture.DateTimeFormat.AbbreviatedMonthNames[startDate.AddDays(d).Month];
            }
                    

            
            #line default
            #line hidden
WriteLiteral("                    ");


            
            #line 47 "..\..\Calendar\Views\Calendar.cshtml"
               Write(month);

            
            #line default
            #line hidden
WriteLiteral("\r\n                </td>\r\n");


            
            #line 49 "..\..\Calendar\Views\Calendar.cshtml"
                 for (int dow = 0; dow < 7; dow++)
                {
                    DateTime day = startDate.AddDays(d + dow);

            
            #line default
            #line hidden
WriteLiteral("                    <td data-date=\"");


            
            #line 52 "..\..\Calendar\Views\Calendar.cshtml"
                              Write(day.ToShortDateString());

            
            #line default
            #line hidden
WriteLiteral("\">\r\n                        ");


            
            #line 53 "..\..\Calendar\Views\Calendar.cshtml"
                   Write(cellTemplate(day, d + dow));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </td>\r\n");


            
            #line 55 "..\..\Calendar\Views\Calendar.cshtml"
                }

            
            #line default
            #line hidden

            
            #line 56 "..\..\Calendar\Views\Calendar.cshtml"
                   d += 6; 

            
            #line default
            #line hidden
WriteLiteral("            </tr>    \r\n");


            
            #line 58 "..\..\Calendar\Views\Calendar.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("    </tbody>\r\n</table>\r\n\r\n");


            
            #line 62 "..\..\Calendar\Views\Calendar.cshtml"
Write(Html.ScriptsJs("~/calendar/scripts/calendar.js"));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");


        }
    }
}
#pragma warning restore 1591