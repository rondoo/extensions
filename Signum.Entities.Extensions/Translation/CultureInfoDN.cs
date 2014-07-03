﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Globalization;
using Signum.Utilities;
using System.Reflection;
using Signum.Entities.Authorization;

namespace Signum.Entities.Translation
{
    [Serializable, EntityKind(EntityKind.String, EntityData.Master)]
    public class CultureInfoDN : Entity
    {
        public CultureInfoDN() { }

        public CultureInfoDN(CultureInfo ci)
        {
            Name = ci.Name;
            NativeName = ci.NativeName;
            EnglishName = ci.EnglishName;
        }

        [NotNullable, SqlDbType(Size = 10), UniqueIndex]
        string name;
        [StringLengthValidator(AllowNulls = false, Min = 2, Max = 10)]
        public string Name
        {
            get { return name; }
            set { SetToStr(ref name, value); }
        }

        string nativeName;
        public string NativeName
        {
            get { return nativeName; }
            private set { Set(ref nativeName, value); }
        }

        string englishName;
        public string EnglishName
        {
            get { return englishName; }
            private set { Set(ref englishName, value); }
        }

        protected override string PropertyValidation(PropertyInfo pi)
        {
            if (pi.Is(() => Name) && Name.HasText())
            {
                try
                {
                    CultureInfo.GetCultureInfo(this.Name);
                }
                catch (CultureNotFoundException)
                {
                    return "'{0}' is not a valid culture name".Formato(Name);
                }
            }

            return base.PropertyValidation(pi);
        }

        protected override void PreSaving(ref bool graphModified)
        {
            try
            {
                var ci = CultureInfo.GetCultureInfo(Name);
                EnglishName = ci.EnglishName;
                NativeName = ci.NativeName;
            }
            catch (CultureNotFoundException)
            {
            }

            base.PreSaving(ref graphModified);
        }

        static Expression<Func<CultureInfoDN, string>> ToStringExpression = e => e.EnglishName;
        public override string ToString()
        {
            return ToStringExpression.Evaluate(this);
        }
    }

    public static class CultureInfoOperation
    {
        public static readonly ExecuteSymbol<CultureInfoDN> Save = OperationSymbol.Execute<CultureInfoDN>();
    }

    public static class TranslationPermission
    {
        public static readonly PermissionSymbol TranslateCode = new PermissionSymbol();
        public static readonly PermissionSymbol TranslateInstances = new PermissionSymbol();
    }
}
