﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Signum.Utilities;
using Signum.Entities.Reflection;
using Signum.Entities.Extensions.Properties;
using System.Text.RegularExpressions;
using Signum.Entities.Omnibox;

namespace Signum.Entities.UserQueries
{
    public class UserQueryOmniboxResultGenerator : OmniboxResultGenerator<UserQueryOmniboxResult>
    {
        public int AutoCompleteLimit = 5;

        public override IEnumerable<UserQueryOmniboxResult> GetResults(string rawQuery, List<OmniboxToken> tokens, string tokenPattern)
        {
            if (tokenPattern != "S" || !OmniboxParser.Manager.AllowedType(typeof(UserQueryDN)))
                yield break;

            string ident = OmniboxUtils.CleanCommas(tokens[0].Value);

            var userQueries = OmniboxParser.Manager.AutoComplete(typeof(UserQueryDN), ident, AutoCompleteLimit);

            foreach (var uq in userQueries)
            {
                var match = OmniboxUtils.Contains(uq, uq.ToString(), ident);

                yield return new UserQueryOmniboxResult
                {
                    ToStr = ident,
                    ToStrMatch = match,
                    Distance = match.Distance,
                    UserQuery = (Lite<UserQueryDN>)uq,
                };
            }
        }

        public override List<HelpOmniboxResult> GetHelp()
        {
            var resultType = typeof(UserQueryOmniboxResult);
            var userQuery = Signum.Entities.Extensions.Properties.Resources.Omnibox_UserQuery;
            return new List<HelpOmniboxResult>
            {
                new HelpOmniboxResult { ToStr = "'{0}'".Formato(userQuery), OmniboxResultType = resultType }
            };
        }
    }

    public class UserQueryOmniboxResult : OmniboxResult
    {
        public string ToStr { get; set; }
        public OmniboxMatch ToStrMatch { get; set; }

        public Lite<UserQueryDN> UserQuery { get; set; }

        public override string ToString()
        {
            return "\"{0}\"".Formato(ToStr);
        }
    }
}