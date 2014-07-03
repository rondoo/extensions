﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Signum.Entities;
using Signum.Services;
using System.Reflection;
using Signum.Utilities.Reflection;
using Signum.Utilities;
using Signum.Entities.Basics;

namespace Signum.Web.Basic
{
    public static class BasicClient
    {
        public static string ViewPrefix = "~/basic/Views/{0}.cshtml";

        public static void Start()
        {
            if (Navigator.Manager.NotDefined(MethodInfo.GetCurrentMethod()))
            {
                Navigator.RegisterArea(typeof(BasicClient));

                Navigator.AddSettings(new List<EntitySettings>
                {
                    new EmbeddedEntitySettings<DateSpanDN> { PartialViewName = _ => ViewPrefix.Formato("DateSpan") },
                });
            }
        }
    }
}
