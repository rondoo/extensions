﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Signum.Entities.Processes;
using Signum.Windows.Processes;
using Signum.Windows.Operations;
using System.Windows.Media.Imaging;
using Signum.Entities;
using Signum.Services;
using System.Reflection;
using Signum.Utilities.Reflection;
using System.Windows;
using Signum.Utilities;
using System.Windows.Controls;
using Signum.Entities.Basics;

namespace Signum.Windows.Processes
{
    public static class ProcessClient
    {
        public static void AsserIsStarted()
        {
            Navigator.Manager.AssertDefined(ReflectionTools.GetMethodInfo(() => Start(true, true)));
        }

        public static void Start(bool package, bool packageOperation)
        {
            if (Navigator.Manager.NotDefined(MethodInfo.GetCurrentMethod()))
            {
                Navigator.AddSetting(new EntitySettings<ProcessAlgorithmSymbol> { View = e => new ProcessAlgorithm(), Icon = Image("processAlgorithm.png") });
                Navigator.AddSetting(new EntitySettings<ProcessDN> { View = e => new ProcessUI(), Icon = Image("process.png") });


                Server.SetSymbolIds<ProcessAlgorithmSymbol>();

                OperationClient.AddSettings(new List<OperationSettings>()
                {
                    new EntityOperationSettings(ProcessOperation.Plan){ Icon = Image("plan.png"), Click = ProcessOperation_Plan },
                    new EntityOperationSettings(ProcessOperation.Cancel){ Icon = Image("stop.png") },
                    new EntityOperationSettings(ProcessOperation.Execute){ Icon = Image("play.png") },
                    new EntityOperationSettings(ProcessOperation.Suspend){ Icon = Image("pause.png") },
                });

                if (packageOperation || package)
                    Navigator.AddSetting(new EntitySettings<PackageLineDN> { View = e => new PackageLine(), Icon = Image("packageLine.png") });

                if (package)
                    Navigator.AddSetting(new EntitySettings<PackageDN> { View = e => new Package(), Icon = Image("package.png") });

                if (packageOperation)
                {
                    Navigator.AddSetting(new EntitySettings<PackageOperationDN> { View = e => new PackageOperation(), Icon = Image("package.png") });

                    SearchControl.GetContextMenuItems += SearchControl_GetContextMenuItems;
                }

                if (MixinDeclarations.IsDeclared(typeof(ProcessDN), typeof(UserProcessSessionMixin)))
                {
                    Navigator.EntitySettings<ProcessDN>().OverrideView((p, c) =>
                    {
                        using (Common.DelayRoutes())
                            c.Child<EntityLine>("Algorithm").After(new EntityLine().Set(Common.RouteProperty, "[UserProcessSessionMixin].User"));
                        return c;
                    }); 
                }
            }
        }


        static IEnumerable<MenuItem> SearchControl_GetContextMenuItems(SearchControl sc)
        {
            if (!Navigator.IsViewable(typeof(PackageOperationDN)))
                return Enumerable.Empty<MenuItem>();

            if (sc.SelectedItems.IsNullOrEmpty() || sc.SelectedItems.Length == 1)
                return null;

            if (sc.Implementations.IsByAll)
                return null;

            var types = sc.SelectedItems.Select(a => a.EntityType).Distinct().ToList();

            var result = (from t in sc.Implementations.Types
                          from oi in OperationClient.Manager.OperationInfos(t)
                          where oi.IsEntityOperation
                          group new { t, oi } by oi.OperationSymbol into g
                          let oi = g.First().oi
                          let os = OperationClient.Manager.GetSettings<EntityOperationSettings>(g.Key)
                          let coc = new ContextualOperationContext
                          {
                              Entities = sc.SelectedItems,
                              SearchControl = sc,
                              OperationInfo = oi,
                              OperationSettings = os.Try(a => a.ContextualFromMany),
                              CanExecute = OperationSymbol.NotDefinedForMessage(g.Key, types.Except(g.Select(a => a.t))),
                          }
                          where os == null ? oi.Lite == true && oi.OperationType != OperationType.ConstructorFrom :
                              os.ContextualFromMany.IsVisible == null ? (oi.Lite == true && os.IsVisible == null && oi.OperationType != OperationType.ConstructorFrom && (os.Click == null || os.ContextualFromMany.Click != null)) :
                              os.ContextualFromMany.IsVisible(coc)
                          select coc).ToList();

            if (result.IsEmpty())
                return null;

            var cleanKeys = result
                .Where(cod => cod.CanExecute == null && cod.OperationInfo.HasStates == true)
                .Select(cod => cod.OperationInfo.OperationSymbol).ToList();

            if (cleanKeys.Any())
            {
                Dictionary<OperationSymbol, string> canExecutes = Server.Return((IOperationServer os) => os.GetContextualCanExecute(sc.SelectedItems, cleanKeys));
                foreach (var cod in result)
                {
                    var ce = canExecutes.TryGetC(cod.OperationInfo.OperationSymbol);
                    if (ce.HasText())
                        cod.CanExecute = ce;
                }
            }

            return result.Select(coc=>PackageOperationMenuItemConsturctor.Construct(coc));
        }


        static ProcessDN ProcessOperation_Plan(EntityOperationContext args)
        {
            DateTime plan = TimeZoneManager.Now;
            if (ValueLineBox.Show(ref plan, "Choose planned date", "Please, choose the date you want the process to start", "Planned date", null, null, Window.GetWindow(args.SenderButton)))
            {
                return  ((ProcessDN)args.Entity).ToLite().ExecuteLite(ProcessOperation.Plan, plan); 
            }
            return null; 
        }

        static BitmapSource Image(string name)
        {
            return ImageLoader.LoadIcon(PackUriHelper.Reference("Images/" + name, typeof(ProcessClient)));
        }
    }
}
