﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Signum.Entities;
using System.Reflection;
using Signum.Utilities.Reflection;
using Signum.Entities.Authorization;
using System.Windows.Controls;
using Signum.Utilities;
using Signum.Services;
using System.Windows;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using Signum.Entities.Basics;

namespace Signum.Windows.Authorization
{
    public static class TypeAuthClient
    {
        static DefaultDictionary<Type, TypeAllowedAndConditions> typeRules; 

        public static bool Started { get; private set; }

        internal static void Start()
        {
            Started = true;

            MenuManager.Tasks += new Action<MenuItem>(MenuManager_TasksTypes);

            var manager = Navigator.Manager;

            manager.IsCreable += manager_IsCreable;

            manager.IsReadOnly += manager_IsReadOnly;

            manager.IsViewable += manager_IsViewable;

            Server.SetSymbolIds<TypeConditionSymbol>();

            LinksClient.RegisterEntityLinks<RoleDN>((r, c) =>
            {
                bool authorized = BasicPermission.AdminRules.IsAuthorized();
                return new QuickLink[]
                {
                    new QuickLinkAction(AuthAdminMessage.TypeRules, () => 
                        Navigator.OpenIndependentWindow(()=> new TypeRules 
                        { 
                            Role = r, 
                            Properties = PropertyAuthClient.Started, 
                            Operations = OperationAuthClient.Started,
                            Queries = QueryAuthClient.Started
                        }))
                    { 
                        IsVisible = authorized
                    },
                 };
            }); 

            AuthClient.UpdateCacheEvent += new Action(AuthClient_UpdateCacheEvent);
        }

        static bool manager_IsViewable(Type type, ModifiableEntity entity)
        {
            IdentifiableEntity ident = entity as IdentifiableEntity;

            if (ident == null || ident.IsNew)
                return GetAllowed(type).MaxUI() >= TypeAllowedBasic.Read;

            return ident.IsAllowedFor(TypeAllowedBasic.Read);
        }

        static bool manager_IsCreable(Type type)
        {
            return GetAllowed(type).MaxUI() == TypeAllowedBasic.Create;
        }

        static bool manager_IsReadOnly(Type type, ModifiableEntity entity)
        {
            IdentifiableEntity ident = entity as IdentifiableEntity;

            if (ident == null || ident.IsNew)
                return GetAllowed(type).MaxUI() < TypeAllowedBasic.Modify;
            else
                return !ident.IsAllowedFor(TypeAllowedBasic.Modify);
        }

        public static bool IsAllowedFor(this Lite<IIdentifiable> lite, TypeAllowedBasic requested)
        {
            TypeAllowedAndConditions tac = GetAllowed(lite.EntityType);

            if (requested <= tac.MinUI())
                return true;

            if (tac.MaxUI() < requested)
                return false;

            return Server.Return((ITypeAuthServer s) => s.IsAllowedForInUserInterface(lite, requested));
        }

        public static bool IsAllowedFor(this IdentifiableEntity entity, TypeAllowedBasic requested)
        {
            TypeAllowedAndConditions tac = GetAllowed(entity.GetType());

            if (requested <= tac.MinUI())
                return true;

            if (tac.MaxUI() < requested)
                return false;

            return Server.Return((ITypeAuthServer s) => s.IsAllowedForInUserInterface(entity.ToLite(), requested));
        }

        public static TypeAllowedAndConditions GetAllowed(Type type)
        {
            if (!typeof(IdentifiableEntity).IsAssignableFrom(type))
                return new TypeAllowedAndConditions(TypeAllowed.Create);

            TypeAllowedAndConditions tac = typeRules.GetAllowed(type);
            return tac;
        }

        static void AuthClient_UpdateCacheEvent()
        {
            typeRules = Server.Return((ITypeAuthServer s) => s.AuthorizedTypes());
        }

        static void MenuManager_TasksTypes(MenuItem menuItem)
        {
            if (menuItem.NotSet(MenuItem.VisibilityProperty))
            {
                object tag = menuItem.Tag;

                if (tag == null)
                    return;

                Type type = tag as Type;

                if (type != null && Navigator.Manager.EntitySettings.ContainsKey(type))
                {
                    if (GetAllowed(type).MaxUI() < TypeAllowedBasic.Read)
                        menuItem.Visibility = Visibility.Collapsed;
                }
            }
        }
    }

    public class VisibleIfNotContainsExtension : MarkupExtension, IValueConverter
    {
        object Element { get; set; }

        public VisibleIfNotContainsExtension(object element)
        {
            this.Element = element;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || !value.GetType().IsArray || value.GetType().GetElementType().UnNullify() != Element.GetType().UnNullify())
                return DependencyProperty.UnsetValue;

            return Array.IndexOf((Array)value, Element) == -1 ? Visibility.Visible : Visibility.Hidden; /*bool*/
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class BoolExtension : MarkupExtension
    {
        public BoolExtension() { }

        public BoolExtension(string value)
        {
            Value = value;
        }

        string Value { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return bool.Parse(Value);
        }
    }
}

