﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Signum.Engine.Maps;
using Signum.Engine.DynamicQuery;
using Signum.Entities.Files;
using System.Reflection;
using System.Linq.Expressions;
using Signum.Utilities.ExpressionTrees;
using Signum.Entities;
using Signum.Utilities;

namespace Signum.Engine.Files
{
    public static class FileLogic
    {
        public static Func<Lite<FileEntity>, string> DownloadFileUrl;

        static Expression<Func<FileEntity, WebImage>> WebImageFileExpression =
            f => new WebImage { FullWebPath = DownloadFileUrl(f.ToLite()) };
        [ExpressionField("WebImageFileExpression")]
        public static WebImage WebImage(this FileEntity f)
        {
            return new WebImage { FullWebPath = DownloadFileUrl(f?.ToLite()) };
        }

        static Expression<Func<FileEntity, WebDownload>> WebDownloadFileExpression =
           f => new WebDownload { FullWebPath = DownloadFileUrl(f.ToLite()) };
        [ExpressionField("WebDownloadFileExpression")]
        public static WebDownload WebDownload(this FileEntity f)
        {
            return new WebDownload { FullWebPath = DownloadFileUrl(f?.ToLite()) };
        }

        public static void Start(SchemaBuilder sb, DynamicQueryManager dqm)
        {
            if (sb.NotDefined(MethodInfo.GetCurrentMethod()))
            {
                sb.Include<FileEntity>();

                dqm.RegisterQuery(typeof(FileEntity), () =>
                    from a in Database.Query<FileEntity>()
                    select new
                    {
                        Entity = a,
                        a.Id,
                        a.FileName,
                    });


                dqm.RegisterExpression((FileEntity f) => f.WebImage(), () => typeof(WebImage).NiceName(), "Image");
                dqm.RegisterExpression((FileEntity f) => f.WebDownload(), () => typeof(WebDownload).NiceName(), "Download");
            }
        }
    }
}
