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
    
    #line 1 "..\..\Profiler\Views\ProfilerButtons.cshtml"
    using Signum.Web.Profiler;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Profiler/Views/ProfilerButtons.cshtml")]
    public partial class ProfilerButtons : System.Web.Mvc.WebViewPage<dynamic>
    {
        public ProfilerButtons()
        {
        }
        public override void Execute()
        {
WriteLiteral("<a");

WriteAttribute("href", Tuple.Create(" href=\"", 31), Tuple.Create("\"", 90)
            
            #line 2 "..\..\Profiler\Views\ProfilerButtons.cshtml"
, Tuple.Create(Tuple.Create("", 38), Tuple.Create<System.Object, System.Int32>(Url.Action((ProfilerController pc) => pc.Disable())
            
            #line default
            #line hidden
, 38), false)
);

WriteLiteral(" class=\"btn btn-default active\"");

WriteAttribute("style", Tuple.Create(" style=\"", 122), Tuple.Create("\"", 186)
            
            #line 2 "..\..\Profiler\Views\ProfilerButtons.cshtml"
                       , Tuple.Create(Tuple.Create("", 130), Tuple.Create<System.Object, System.Int32>(HeavyProfiler.Enabled ? "" : "display:none"
            
            #line default
            #line hidden
, 130), false)
, Tuple.Create(Tuple.Create("", 176), Tuple.Create(";color:red", 176), true)
);

WriteLiteral(" id=\"sfProfileDisable\"");

WriteLiteral(">\r\nDisable\r\n</a>\r\n<a");

WriteAttribute("href", Tuple.Create(" href=\"", 229), Tuple.Create("\"", 287)
            
            #line 5 "..\..\Profiler\Views\ProfilerButtons.cshtml"
, Tuple.Create(Tuple.Create("", 236), Tuple.Create<System.Object, System.Int32>(Url.Action((ProfilerController pc) => pc.Enable())
            
            #line default
            #line hidden
, 236), false)
);

WriteLiteral(" class=\"btn btn-default\"");

WriteAttribute("style", Tuple.Create("  style=\"", 312), Tuple.Create("\"", 368)
            
            #line 5 "..\..\Profiler\Views\ProfilerButtons.cshtml"
                , Tuple.Create(Tuple.Create("", 321), Tuple.Create<System.Object, System.Int32>(!HeavyProfiler.Enabled ? "" : "display:none"
            
            #line default
            #line hidden
, 321), false)
);

WriteLiteral(" id=\"sfProfileEnable\"");

WriteLiteral(">\r\nEnable\r\n</a>\r\n<a");

WriteAttribute("href", Tuple.Create(" href=\"", 409), Tuple.Create("\"", 466)
            
            #line 8 "..\..\Profiler\Views\ProfilerButtons.cshtml"
, Tuple.Create(Tuple.Create("", 416), Tuple.Create<System.Object, System.Int32>(Url.Action((ProfilerController pc) => pc.Clean())
            
            #line default
            #line hidden
, 416), false)
);

WriteLiteral(" class=\"btn btn-default\"");

WriteLiteral(" id=\"sfProfileClear\"");

WriteLiteral(">\r\nClean\r\n</a>");

        }
    }
}
#pragma warning restore 1591
