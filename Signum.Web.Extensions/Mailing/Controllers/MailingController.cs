﻿using Signum.Engine;
using Signum.Engine.Basics;
using Signum.Engine.DynamicQuery;
using Signum.Engine.Mailing;
using Signum.Engine.Operations;
using Signum.Entities;
using Signum.Entities.DynamicQuery;
using Signum.Entities.Mailing;
using Signum.Utilities;
using Signum.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Signum.Web.Operations;

namespace Signum.Web.Mailing
{
    public class MailingController : Controller
    {
        [HttpPost]
        public ContentResult NewSubTokensCombo(string webQueryName, string tokenName, string prefix)
        {
            object queryName = Navigator.ResolveQueryName(webQueryName);
            QueryDescription qd = DynamicQueryManager.Current.QueryDescription(queryName);
            var token = QueryUtils.Parse(tokenName, qd, canAggregate: false);

            var combo = FinderController.CreateHtmlHelper(this)
                .QueryTokenBuilderOptions(token, new Context(null, prefix), MailingClient.GetQueryTokenBuilderSettings(qd));

            return Content(combo.ToHtmlString());
        }

        [HttpPost]
        public ActionResult RemoveRecipientsExecute()
        {
            var deliveries = this.ParseLiteKeys<NewsletterDeliveryDN>();

            var newsletter = this.ExtractEntity<NewsletterDN>();
            
            newsletter.Execute(NewsletterOperation.RemoveRecipients, deliveries);

            return this.DefaultExecuteResult(newsletter);
        }

        [HttpPost]
        public ActionResult CreateMailFromTemplateAndEntity()
        {
            var entity = Lite.Parse(Request["keys"]).Retrieve();

            var emailMessage = this.ExtractEntity<EmailTemplateDN>()
                .ConstructFrom(EmailMessageOperation.CreateMailFromTemplate, entity);

            return this.DefaultConstructResult(emailMessage);
        }
    }
}
