﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Signum.Engine;
using Signum.Engine.Basics;
using Signum.Engine.Translation;
using Signum.Entities;
using Signum.Entities.Authorization;
using Signum.Entities.Basics;
using Signum.Entities.Translation;
using Signum.Utilities;

namespace Signum.Web.Translation.Controllers
{
    [ValidateInputAttribute(false)]
    public class TranslatedInstanceController : Controller
    {
        public ActionResult Index()
        {
            var cultures = TranslationLogic.CurrentCultureInfos(TranslatedInstanceLogic.DefaultCulture);

            var list = TranslatedInstanceLogic.TranslationInstancesStatus();

            return base.View(TranslationClient.ViewPrefix.Formato("IndexInstance"), list.AgGroupToDictionary(a => a.Type, gr => gr.ToDictionary(a => a.CultureInfo)));
        }

        [HttpGet]
        public new ActionResult View(string type, string culture, bool searchPressed, string filter)
        {
            Type t = TypeLogic.GetType(type);
            ViewBag.Type = t;
            var c = culture == null ? null : CultureInfo.GetCultureInfo(culture);
            ViewBag.Culture = c;

            ViewBag.Filter = filter;

            if (!searchPressed)
                return base.View(TranslationClient.ViewPrefix.Formato("ViewInstance"));

            Dictionary<LocalizedInstanceKey, string> master = TranslatedInstanceLogic.FromEntities(t);

            ViewBag.Master = master;

            Dictionary<CultureInfo, Dictionary<LocalizedInstanceKey, TranslatedInstanceDN>> support = TranslatedInstanceLogic.TranslationsForType(t, culture: c);

            return base.View(TranslationClient.ViewPrefix.Formato("ViewInstance"), support);
        }

        public FileContentResult ViewFile(string type, string culture)
        {
             Type t = TypeLogic.GetType(type);
             var c = CultureInfo.GetCultureInfo(culture);

            var bytes = TranslatedInstanceLogic.GetExcelFile(t, c);

            var fileName = "{0}.{1}.View.xlsx".Formato(type, c.Name);

            return File(bytes, MimeType.FromFileName(fileName), fileName);  
        }

        [HttpPost]
        public ActionResult SaveView(string type, string culture, string filter)
        {
            Type t = TypeLogic.GetType(type);

            var records = GetTranslationRecords(t);

            var c = culture == null ? null : CultureInfo.GetCultureInfo(culture);

             TranslatedInstanceLogic.SaveRecords(records, t, c);

            return RedirectToAction("View", new { type = type, culture = culture, filter = filter, searchPressed = true });
        }


        static Regex regex = new Regex(@"^(?<lang>[^#]+)#(?<instance>[^#]+)#(?<route>[^#]+)$");

        private List<TranslationRecord> GetTranslationRecords(Type type)
        {
            var list = (from k in Request.Form.AllKeys
                        let m = regex.Match(k)
                        where m.Success
                        select new TranslationRecord
                        {
                            Culture = CultureInfo.GetCultureInfo(m.Groups["lang"].Value),
                            Key = new LocalizedInstanceKey(
                                PropertyRoute.Parse(type, m.Groups["route"].Value), 
                                Lite.Parse(m.Groups["instance"].Value)),
                            TranslatedText = Request.Form[k].DefaultText(null),
                        }).ToList();

            var master = list.Extract(a => a.Culture.Name == TranslatedInstanceLogic.DefaultCulture).ToDictionary(a=>a.Key);

            list.ForEach(r => r.OriginalText = master.GetOrThrow(r.Key).TranslatedText);

            return list;
        }

      

        public ActionResult Sync(string type, string culture)
        {
            Type t = TypeLogic.GetType(type);

            var c = CultureInfo.GetCultureInfo(culture);

            int totalInstances; 
            var changes = TranslatedInstanceSynchronizer.GetTypeInstanceChangesTranslated(TranslationClient.Translator, t, c, out totalInstances);

            ViewBag.TotalInstances = totalInstances; 
            ViewBag.Culture = c;
            return base.View(TranslationClient.ViewPrefix.Formato("SyncInstance"), changes);
        }

        public FileContentResult SyncFile(string type, string culture)
        {
            Type t = TypeLogic.GetType(type);

            CultureInfo c = CultureInfo.GetCultureInfo(culture);

            CultureInfo master = CultureInfo.GetCultureInfo(TranslatedInstanceLogic.DefaultCulture);

            var changes = TranslatedInstanceLogic.GetInstanceChanges(t, c, new List<CultureInfo> { master });

            byte[] bytes = TranslatedInstanceLogic.GetSyncExcelFile(changes, master, c);

            string fileName = "{0}.{1}.Sync.xlsx".Formato(type, c.Name);

            return File(bytes, MimeType.FromFileName(fileName), fileName);  
        }

        [HttpPost]
        public ActionResult SaveSync(string type, string culture)
        {
            Type t = TypeLogic.GetType(type);

            var c = CultureInfo.GetCultureInfo(culture);

            List<TranslationRecord> records = GetTranslationRecords(t);

            TranslatedInstanceLogic.SaveRecords(records, t, c);

            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadFile()
        {
            HttpPostedFileBase hpf = Request.Files[Request.Files.Cast<string>().Single()];

            var type = TypeLogic.GetType(hpf.FileName.Before('.'));
            var culture = CultureInfo.GetCultureInfo(hpf.FileName.After('.').Before('.'));

            TranslatedInstanceLogic.SaveExcelFile(hpf.InputStream, type, culture);

            return RedirectToAction("View", new { type = TypeLogic.GetCleanName(type), culture = culture.Name, searchPressed = false });
        }

    }
}
