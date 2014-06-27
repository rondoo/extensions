﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Signum.Engine.Authorization;
using Signum.Engine.Translation;
using Signum.Entities;
using Signum.Entities.Translation;
using Signum.Utilities;
using Signum.Utilities.ExpressionTrees;
using Signum.Web.Extensions.Translation.Views;
using Signum.Web.Omnibox;
using Signum.Web.PortableAreas;
using Signum.Web.Translation.Controllers;

namespace Signum.Web.Translation
{
    public static class TranslationClient
    {
        public static string ViewPrefix = "~/Translation/Views/{0}.cshtml";
        public static JsModule Module = new JsModule("Extensions/Signum.Web.Extensions/Translation/Scripts/Translation");

        public static ITranslator Translator; 


        /// <param name="copyTranslationsToRootFolder">avoids Web Application restart when translations change</param>
        public static void Start(ITranslator translator, bool translatorUser, bool translationReplacement, bool instanceTranslator, bool copyNewTranslationsToRootFolder = true)
        {
            if (Navigator.Manager.NotDefined(MethodInfo.GetCurrentMethod()))
            {

                Translator = translator;

                Navigator.RegisterArea(typeof(TranslationClient));
                Navigator.AddSettings(new List<EntitySettings>
                {   
                    new EntitySettings<CultureInfoDN>{ PartialViewName = t=>ViewPrefix.Formato("CultureInfoView")},
                });

                if (translatorUser)
                {
                    Navigator.AddSettings(new List<EntitySettings>
                    {
                        new EntitySettings<TranslatorUserDN>{ PartialViewName = t=>ViewPrefix.Formato("TranslatorUser")},
                        new EmbeddedEntitySettings<TranslatorUserCultureDN>{ PartialViewName = t=>ViewPrefix.Formato("TranslatorUserCulture")},
                    });
                }

                if (translationReplacement)
                {
                    FileRepositoryManager.Register(new LocalizedJavaScriptRepository(typeof(TranslationJavascriptMessage), "translation"));

                    Navigator.AddSettings(new List<EntitySettings>
                    {
                        new EntitySettings<TranslationReplacementDN>{ PartialViewName = t=>ViewPrefix.Formato("TranslationReplacement")},
                    });
                }

                SpecialOmniboxProvider.Register(new SpecialOmniboxAction("TranslateCode",
                    () => TranslationPermission.TranslateCode.IsAuthorized(),
                    uh => uh.Action((TranslationController tc) => tc.Index())));

                if (instanceTranslator)
                {
                    SpecialOmniboxProvider.Register(new SpecialOmniboxAction("TranslateInstances",
                        () => TranslationPermission.TranslateInstances.IsAuthorized(),
                        uh => uh.Action((TranslatedInstanceController tic) => tic.Index())));
                }

                if (copyNewTranslationsToRootFolder)
                {
                    string path = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(new Uri(typeof(DescriptionManager).Assembly.CodeBase).LocalPath)), "Translations");

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    var existingFiles = Directory.GetFiles(path).ToHashSet();

                    foreach (string fromFile in Directory.GetFiles(DescriptionManager.TranslationDirectory))
                    {
                        string toFile = Path.Combine(path, Path.GetFileName(fromFile));

                        if (!existingFiles.Contains(toFile) || File.GetLastWriteTime(toFile) < File.GetLastWriteTime(fromFile))
                        {
                            File.Copy(fromFile, toFile, overwrite: true);
                        }
                    }

                    DescriptionManager.TranslationDirectory = path;
                }
            }
        }


        public static SelectListItem ToTranslatedSelectListItem<T>(this Lite<T> lite, Lite<T> selected, Expression<Func<T, string>> toStringField) where T : IdentifiableEntity
        {
            return new SelectListItem { Text = lite.TranslatedField(toStringField, lite.ToString()), Value = lite.Id.ToString(), Selected = lite.Equals(selected) };
        }
    }
}