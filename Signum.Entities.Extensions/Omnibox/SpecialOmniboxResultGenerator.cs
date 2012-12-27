﻿using Signum.Entities.Omnibox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Signum.Entities.Omnibox
{
    public class SpecialOmniboxResult : OmniboxResult
    {
        public OmniboxMatch Match { get; set; }

        public override string ToString()
        {
            return "!" + ((ISpecialOmniboxAction)Match.Value).Key;
        }
    }

    public interface ISpecialOmniboxAction
    {
        string Key { get; }
    }

    public class SpecialOmniboxGenerator<T> : OmniboxResultGenerator<SpecialOmniboxResult>
    {
        public Dictionary<string, T> Actions;

        Regex regex = new Regex(@"^!I?$", RegexOptions.ExplicitCapture);

        public override IEnumerable<SpecialOmniboxResult> GetResults(string rawQuery, List<OmniboxToken> tokens, string tokenPattern)
        {
            if (!regex.IsMatch(tokenPattern))
                return Enumerable.Empty<SpecialOmniboxResult>();

            string ident = tokens.Count == 1 ? "" : tokens[1].Value;

            bool isPascalCase = OmniboxUtils.IsPascalCasePattern(ident);

            return OmniboxUtils.Matches(Actions, null, ident, isPascalCase)
                .Select(m => new SpecialOmniboxResult { Match = m, Distance = m.Distance });
        }

        public override List<HelpOmniboxResult> GetHelp()
        {
            return new List<HelpOmniboxResult>
            {
                new HelpOmniboxResult { Text = "!SpecialFunction", OmniboxResultType = typeof(SpecialOmniboxResult) },
            };
        }
    }
}