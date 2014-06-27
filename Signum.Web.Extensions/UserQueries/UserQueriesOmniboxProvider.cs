﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Signum.Entities.UserQueries;
using Signum.Web.Omnibox;
using Signum.Entities.Omnibox;
using Signum.Engine.DynamicQuery;
using System.Web.Mvc;
using Signum.Utilities;
using Signum.Engine.UserQueries;

namespace Signum.Web.UserQueries
{
    public class UserQueriesOmniboxProvider : OmniboxClient.OmniboxProvider<UserQueryOmniboxResult>
    {
        public override OmniboxResultGenerator<UserQueryOmniboxResult> CreateGenerator()
        {
            return new UserQueryOmniboxResultGenerator(UserQueryLogic.Autocomplete);
        }

        public override MvcHtmlString RenderHtml(UserQueryOmniboxResult result)
        {
            MvcHtmlString html = result.ToStrMatch.ToHtml();

            html = html.Concat(Icon());
                
            return html;
        }

        public override string GetUrl(UserQueryOmniboxResult result)
        {
            return RouteHelper.New().Action<UserQueriesController>(uqc => uqc.View(result.UserQuery, null, null));
        }

        public override MvcHtmlString Icon()
        {
            return ColoredSpan(" ({0})".Formato(typeof(UserQueryDN).NiceName()), "dodgerblue");
        }
    }
}