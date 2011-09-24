﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.237
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
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel.DataAnnotations;
    using System.Configuration;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.Caching;
    using System.Web.DynamicData;
    using System.Web.SessionState;
    using System.Web.Profile;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using System.Web.UI.HtmlControls;
    using System.Xml.Linq;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using Signum.Utilities;
    using Signum.Entities;
    using Signum.Web;
    using Signum.Entities.DynamicQuery;
    using Signum.Entities.Reflection;
    using Signum.Web.Properties;
    using Signum.Engine;
    using Signum.Entities.Chart;
    using Signum.Web.Chart;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MvcRazorClassGenerator", "1.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Chart/Views/ChartResults.cshtml")]
    public class _Page_Chart_Views_ChartResults_cshtml : System.Web.Mvc.WebViewPage<TypeContext<ChartRequest>>
    {


        public _Page_Chart_Views_ChartResults_cshtml()
        {
        }
        protected System.Web.HttpApplication ApplicationInstance
        {
            get
            {
                return ((System.Web.HttpApplication)(Context.ApplicationInstance));
            }
        }
        public override void Execute()
        {










WriteLiteral("\r\n");


   
    ResultTable queryResult = (ResultTable)ViewData[ViewDataKeys.Results];
    bool viewable = (bool)ViewData[ViewDataKeys.View];
    var formatters = (Dictionary<int, Func<HtmlHelper, object, MvcHtmlString>>)ViewData[ViewDataKeys.Formatters];


WriteLiteral("\r\n");


 if (queryResult == null)
{ 
    
Write(Resources.Signum_noResults);

                               
}
else
{

WriteLiteral("    <div class=\"sf-tabs\">\r\n        <ul>\r\n            <li><a href=\"");


                     Write("#" + Model.Compose("sfChartContainer"));

WriteLiteral("\">Chart</a></li>\r\n            <li><a href=\"");


                     Write("#" + Model.Compose("sfChartData"));

WriteLiteral("\">Data</a></li>\r\n            <li><a href=\"");


                     Write("#" + Model.Compose("sfChartCode"));

WriteLiteral("\">Code</a></li>\r\n        </ul>\r\n        <div id=\"");


            Write(Model.Compose("sfChartContainer"));

WriteLiteral("\">\r\n            <div class=\"sf-chart-container\"></div>\r\n        </div>\r\n        <" +
"div id=\"");


            Write(Model.Compose("sfChartData"));

WriteLiteral("\">\r\n            <table id=\"");


                  Write(Model.Compose("tblResults"));

WriteLiteral("\" class=\"sf-search-results\">\r\n                <thead class=\"ui-widget-header ui-c" +
"orner-top\">\r\n                    <tr>\r\n");


                         if (!Model.Value.Chart.GroupResults && viewable)
                        {

WriteLiteral("                            <th class=\"ui-state-default th-col-entity\">\r\n        " +
"                    </th>\r\n");


                        }


                         if (queryResult.Columns.Any())
                        {
                            foreach (ResultColumn col in queryResult.Columns)
                            {

WriteLiteral("                                <th class=\"ui-state-default\">\r\n                  " +
"                  <input type=\"hidden\" value=\"");


                                                           Write(col.Column.Name);

WriteLiteral("\" />\r\n                                    ");


                               Write(col.Column.DisplayName);

WriteLiteral("\r\n                                </th>\r\n");


                            }
                        }

WriteLiteral("                    </tr>\r\n                </thead>\r\n                <tbody class" +
"=\"ui-widget-content\">\r\n");


                     if (!queryResult.Rows.Any())
                    {

WriteLiteral("                        <tr>\r\n                            <td colspan=\"");


                                     Write(queryResult.Columns.Count() + (viewable ? 1 : 0));

WriteLiteral("\">");


                                                                                         Write(Resources.Signum_noResults);

WriteLiteral("</td>\r\n                        </tr>\r\n");


                    }
                    else
                    {
                        foreach (var row in queryResult.Rows)
                        {
                            if (Model.Value.Chart.GroupResults)
                            {

WriteLiteral("                                <tr>\r\n");


                                     foreach (var col in queryResult.Columns)
                                    {

WriteLiteral("                                        <td>\r\n                                   " +
"         ");


                                       Write(formatters[col.Index](Html, row[col]));

WriteLiteral("\r\n                                        </td>\r\n");


                                    }   

WriteLiteral("                                </tr>\r\n");


                            }
                            else
                            {
                                Lite entityField = row.Entity;

WriteLiteral("                                <tr data-entity=\"");


                                            Write(entityField.Key());

WriteLiteral("\">\r\n");


                                     if (entityField != null && viewable)
                                    {

WriteLiteral("                                        <td>\r\n                                   " +
"         ");


                                       Write(QuerySettings.EntityFormatRules.Last(fr => fr.IsApplyable(entityField)).Formatter(Html, entityField));

WriteLiteral("\r\n                                        </td>\r\n");


                                    }


                                     foreach (var col in queryResult.Columns)
                                    {

WriteLiteral("                                        <td>\r\n                                   " +
"         ");


                                       Write(formatters[col.Index](Html, row[col]));

WriteLiteral("\r\n                                        </td>\r\n");


                                    }

WriteLiteral("                                </tr>\r\n");


                            }
                        }
                    }

WriteLiteral("                </tbody>\r\n                <tfoot>\r\n                </tfoot>\r\n    " +
"        </table>\r\n        </div>\r\n        <div id=\"");


            Write(Model.Compose("sfChartCode"));

WriteLiteral("\" class=\"sf-chart-code\">\r\n            <div class=\"sf-chart-code-container\">\r\n    " +
"            <textarea rows=\"60\"></textarea>\r\n            </div>\r\n");


               MvcHtmlString divSelector = MvcHtmlString.Create("#" + Model.Compose("sfChartContainer") + " > .sf-chart-container"); 

WriteLiteral("            <script type=\"text/javascript\">\r\n\r\n                $(\'");


              Write(divSelector);

WriteLiteral("\').closest(\'.sf-tabs\').bind( \"tabsshow\", function(event, ui) {\r\n                 " +
"   var myChart = SF.Chart.Factory.getGraphType(\'");


                                                            Write(Model.Value.Chart.ChartType.ToString());

WriteLiteral("\');\r\n                \r\n                    var $chartContainer = $(\'");


                                        Write(divSelector);

WriteLiteral("\');\r\n                    var width = $chartContainer.width();\r\n                  " +
"  var height = $chartContainer.height();\r\n\r\n                    var data = ");


                          Write(Html.Json(ChartClient.DataJson(Model.Value, queryResult)));

WriteLiteral(";\r\n\r\n                    var code = SF.Chart.Factory.createChartSVG(\'");


                                                           Write(divSelector);

WriteLiteral("\') + \r\n                        myChart.paintChart();\r\n\r\n                    $(\'#");


                    Write(Model.Compose("sfChartCode"));

WriteLiteral(" textarea\').val(code);\r\n\r\n                    SF.log(code);\r\n                    " +
"eval(code);\r\n                });\r\n\r\n            </script>\r\n        </div>\r\n    <" +
"/div>\r\n");


}

WriteLiteral("\r\n\r\n\r\n");


        }
    }
}
