﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Signum.Engine.Authorization;
using Signum.Entities;
using Signum.Entities.UserQueries;
using Signum.Utilities;
using Signum.Web.Omnibox;
using Signum.Web.UserQueries;

namespace Signum.Web.Extensions.UserQueries
{
    public class UserAssetsClient
    {
        internal static void Start()
        {
            if (Navigator.Manager.NotDefined(MethodInfo.GetCurrentMethod()))
            {
                SpecialOmniboxProvider.Register(new SpecialOmniboxAction("ImportUserAssets", () => UserAssetPermission.UserAssetsToXML.IsAuthorized(), url =>
                    url.Action((UserAssetController a)=>a.Import())));
            }
        }

        internal static void RegisterExportAssertLink<T>() where T : IdentifiableEntity, IUserAssetEntity
        {
            LinksClient.RegisterEntityLinks<T>((lite, ctx) => new[]
            {
               new QuickLinkAction(UserAssetMessage.ExportToXml, RouteHelper.New().Action((UserAssetController a)=>a.Export(lite)))
               {
                   IsVisible = UserAssetPermission.UserAssetsToXML.IsAuthorized()
               }
            });
        }
    }
}