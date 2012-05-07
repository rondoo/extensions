﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Signum.Engine.Maps;
using System.Reflection;
using Signum.Engine.DynamicQuery;
using Signum.Engine.Authorization;
using Signum.Entities.Profiler;
using Signum.Utilities;

namespace Signum.Engine.Profiler
{
    public static class ProfilerLogic
    {
        static Variable<int?> SessionTimeoutVariable = Statics.SessionVariable<int?>("sessionTimeout");
        public static int? SessionTimeout
        {
            get { return SessionTimeoutVariable.Value; }
            set { SessionTimeoutVariable.Value = value; }
        }

        public static void Start(SchemaBuilder sb, DynamicQueryManager dqm, bool timeTracker, bool heavyProfiler, bool overrideSessionTimeout)
        {
            if (sb.NotDefined(MethodInfo.GetCurrentMethod()))
            {
                if (timeTracker)
                    PermissionAuthLogic.RegisterPermissions(ProfilerPermissions.ViewTimeTracker);

                if (heavyProfiler)
                    PermissionAuthLogic.RegisterPermissions(ProfilerPermissions.ViewHeavyProfiler); 

                if(overrideSessionTimeout)
                    PermissionAuthLogic.RegisterPermissions(ProfilerPermissions.OverrideSessionTimeout); 
            }
        }
    }
}
