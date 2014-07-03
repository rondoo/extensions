﻿#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Signum.Utilities;
using Signum.Entities.Files;
using Signum.Entities;
using Signum.Web;
using Signum.Entities.Reflection;
using System.Linq.Expressions;
using Signum.Entities.Basics;
using Signum.Engine.Basics;
using Signum.Engine;
using Signum.Web.PortableAreas;
#endregion

namespace Signum.Web.Files
{
    public static class FileLineHelper
    {
        

        public static MvcHtmlString FileLine<T, S>(this HtmlHelper helper, TypeContext<T> tc, Expression<Func<T, S>> property)
            where S : IFile
        {
            return FileLineInternal<T, S>(helper, tc, property, null);
        }

        public static MvcHtmlString FileLine<T, S>(this HtmlHelper helper, TypeContext<T> tc, Expression<Func<T, S>> property, Action<FileLine> settingsModifier)
            where S : IFile
        {
            return FileLineInternal<T, S>(helper, tc, property, settingsModifier);
        }

        public static MvcHtmlString FileLineLite<T, S>(this HtmlHelper helper, TypeContext<T> tc, Expression<Func<T, Lite<S>>> property)
           where S : class, IFile, IIdentifiable 
        {
            return FileLineInternal<T, Lite<S>>(helper, tc, property, null);
        }

        public static MvcHtmlString FileLineLite<T, S>(this HtmlHelper helper, TypeContext<T> tc, Expression<Func<T, Lite<S>>> property, Action<FileLine> settingsModifier)
           where S : class, IFile, IIdentifiable 
        {
            return FileLineInternal<T, Lite<S>>(helper, tc, property, settingsModifier);
        }

        static MvcHtmlString FileLineInternal<T, S>(HtmlHelper helper, TypeContext<T> tc, Expression<Func<T, S>> property, Action<FileLine> settingsModifier)
        {
            TypeContext<S> context = Common.WalkExpression(tc, property);

            FileLine fl = new FileLine(context.Type, context.UntypedValue, context, "", context.PropertyRoute);

            EntityBaseHelper.ConfigureEntityBase(fl, fl.Type.CleanType());

            fl.Download = (context.Type.IsIIdentifiable() || context.Type.IsLite()) ? DownloadBehaviour.View : DownloadBehaviour.None;

            Common.FireCommonTasks(fl);

            if (settingsModifier != null)
                settingsModifier(fl);

            return helper.InternalFileLine(fl);
        }

        internal static MvcHtmlString InternalFileLine(this HtmlHelper helper, FileLine fileLine)
        {
            if (!fileLine.Visible)
                return MvcHtmlString.Empty;

            IFile value = fileLine.GetFileValue();

            HtmlStringBuilder sbg = new HtmlStringBuilder();

            using (sbg.Surround(new HtmlTag("div").Id(fileLine.Prefix).Class("sf-field SF-control-container")))
            {
                sbg.AddLine(new HtmlTag("link").Attrs(new { rel = "stylesheet", type = "text/css", href = RouteHelper.New().Content("~/Files/Content/Files.css") }).ToHtmlSelf());

                if (value != null)
                    sbg.AddLine(helper.Div(fileLine.Compose(EntityBaseKeys.Entity), null, "", new Dictionary<string, object> { { "style", "display:none" } }));

                fileLine.ValueHtmlProps.AddCssClass("form-control");

                bool hasEntity = value != null && value.FileName.HasText();

                using (sbg.Surround(new HtmlTag("div", fileLine.Compose("DivOld")).Attr("style", "display:" + (hasEntity ? "block" : "none"))))
                {
                    HtmlStringBuilder sb = new HtmlStringBuilder();
                    using (sb.Surround(new HtmlTag("div", fileLine.Compose("inputGroup")).Class("input-group")))
                    {
                        if (fileLine.Download != DownloadBehaviour.None)
                        {
                            sb.AddLine(helper.Href(fileLine.Compose(EntityBaseKeys.Link),
                                value.Try(f => f.FileName),
                                hasEntity ? FilesClient.GetDownloadPath(value) : null,
                                "Download",
                                "form-control",
                                fileLine.Download == DownloadBehaviour.View ? null :
                                new Dictionary<string, object> { { "download", value.Try(f => f.FileName) } }));
                        }
                        else
                        {
                            sb.AddLine(helper.Span(fileLine.Compose(EntityBaseKeys.ToStr), value.Try(f => f.FileName) ?? "", "form-control", null));
                        }

                        if (fileLine.Type.IsEmbeddedEntity())
                            sb.AddLine(helper.Hidden(fileLine.Compose(EntityBaseKeys.EntityState), value.Try(f => Navigator.Manager.SerializeEntity((ModifiableEntity)f))));
                        
                        using (sb.Surround(new HtmlTag("span", fileLine.Compose("shownButton")).Class("input-group-btn")))
                        {
                            sb.AddLine(EntityButtonHelper.Remove(helper, fileLine, btn: true));
                        }
                    }

                    sbg.AddLine(helper.FormGroup(fileLine,
                        fileLine.Download == DownloadBehaviour.None ? fileLine.Compose(EntityBaseKeys.Link) : fileLine.Compose(EntityBaseKeys.ToStr),
                        fileLine.LabelText, sb.ToHtml()));
                }

                using (sbg.Surround(new HtmlTag("div", fileLine.Compose("DivNew"))
                    .Class("sf-file-line-new")
                    .Attr("style", "display:" + (hasEntity ? "none" : "block"))))
                {

                    HtmlStringBuilder sb = new HtmlStringBuilder();
                    sb.AddLine(helper.HiddenRuntimeInfo(fileLine));
                    sb.AddLine(MvcHtmlString.Create("<input type='file' onchange=\"{0}\" id='{1}' name='{1}' class='form-control'/>".Formato(fileLine.SFControlThen("onChanged()"), fileLine.Compose(FileLineKeys.File))));
                    sb.AddLine(MvcHtmlString.Create("<img src='{0}' id='{1}_loading' alt='loading' style='display:none'/>".Formato(RouteHelper.New().Content("~/Files/Images/loading.gif"), fileLine.Prefix)));


                    sbg.AddLine(helper.FormGroup(fileLine,
                        fileLine.Compose(FileLineKeys.File),
                        fileLine.LabelText, sb.ToHtml()));
                }

                sbg.AddLine(fileLine.ConstructorScript(FilesClient.Module, "FileLine"));
            }

            return sbg.ToHtml();
        }
    }
}
