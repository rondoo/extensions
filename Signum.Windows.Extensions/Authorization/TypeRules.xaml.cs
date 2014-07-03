﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Signum.Entities.Authorization;
using Signum.Entities;
using Signum.Services;
using Signum.Utilities;
using System.Windows.Markup;
using System.ComponentModel;
using Signum.Entities.Basics;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

namespace Signum.Windows.Authorization
{
    /// <summary>
    /// Interaction logic for Test.xaml
    /// </summary>
    public partial class TypeRules : Window
    {
        public static Type RuleType = typeof(TypeRuleBuilder);
        public static Type GroupType = typeof(NamespaceNode);
        public static Type ConditionType = typeof(TypeConditionRuleBuilder);

        public static readonly DependencyProperty RoleProperty =
           DependencyProperty.Register("Role", typeof(Lite<RoleDN>), typeof(TypeRules), new UIPropertyMetadata(null));
        public Lite<RoleDN> Role
        {
            get { return (Lite<RoleDN>)GetValue(RoleProperty); }
            set { SetValue(RoleProperty, value); }
        }

        public bool Properties { get; set; }
        public bool Operations { get; set; }
        public bool Queries { get; set; }

       
        public TypeRules()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Test_Loaded);

        }

        ScrollViewer swTree; 

        void Test_Loaded(object sender, RoutedEventArgs e)
        {
            swTree = treeView.Child<ScrollViewer>(WhereFlags.VisualTree);
            grid.Bind(Grid.WidthProperty, swTree.Content, "ActualWidth");
            Load();
        }


        private void Load()
        {
            this.Title = AuthMessage._0RulesFor1.NiceToString().Formato(typeof(TypeDN).NiceName(), Role);

            TypeRulePack trp = Server.Return((ITypeAuthServer s) => s.GetTypesRules(Role));

            DataContext = trp;

            treeView.ItemsSource = (from r in trp.Rules
                                    group r by r.Resource.Namespace into g
                                    orderby g.Key
                                    select new NamespaceNode
                                    {
                                        Name = g.Key,
                                        SubNodes = g.OrderBy(a => a.Resource.ClassName).Select(a=>new TypeRuleBuilder(a)).ToList()
                                    }).ToList();
        }

        public DataTemplateSelector MyDataTemplateSelector;


        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            TypeRulePack trp = (TypeRulePack)DataContext;

            trp.Rules = ((List<NamespaceNode>)treeView.ItemsSource).SelectMany(a => a.SubNodes).Select(a => a.ToRule()).ToMList();

            Server.Execute((ITypeAuthServer s) => s.SetTypesRules(trp));
            Load();
        }

        private void btReload_Click(object sender, RoutedEventArgs e)
        {
            Load(); 
        }

        private void properties_Click(object sender, RoutedEventArgs e)
        {
            TypeRuleBuilder rules = (TypeRuleBuilder)((Button)sender).DataContext;
            var role = Role;
            Navigator.OpenIndependentWindow(() => new PropertyRules
            {
                Type = rules.Resource,
                Role = role
            }); 
        }

        private void operations_Click(object sender, RoutedEventArgs e)
        {
            TypeRuleBuilder rules = (TypeRuleBuilder)((Button)sender).DataContext;
            var role = Role;
            Navigator.OpenIndependentWindow(() => new OperationRules
            {
                Type = rules.Resource,
                Role = role
            }); 
        }

        private void queries_Click(object sender, RoutedEventArgs e)
        {
            TypeRuleBuilder rules = (TypeRuleBuilder)((Button)sender).DataContext;
            var role = Role;
            Navigator.OpenIndependentWindow(() => new QueryRules
            {
                Type = rules.Resource,
                Role = role
            });
        }

        private void treeView_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            swTop.ScrollToHorizontalOffset(e.HorizontalOffset);
        }

        private void addCondition_Click(object sender, RoutedEventArgs e)
        {
            TypeRuleBuilder rules = (TypeRuleBuilder)((Button)sender).DataContext;
            
            TypeConditionSymbol value;
            if (SelectorWindow.ShowDialog<TypeConditionSymbol>(
                rules.AvailableConditions.Except(rules.Conditions.Select(a => a.TypeCondition)).ToArray(), 
                out value, 
                elementIcon: null,
                elementText: v => v.NiceToString(),
                title: "New condition", 
                message: "Select the condition for {0} to add specific authorization rules".Formato(rules.Resource.CleanName), 
                owner: this))
            {
                rules.Conditions.Add(new TypeConditionRuleBuilder(value, rules.Allowed.None ? TypeAllowed.Create : TypeAllowed.None)); 
            }
        }

        private void removeCondition_Click(object sender, RoutedEventArgs e)
        {
            var node = ((Button)sender).VisualParents().OfType<TreeViewItem>().FirstEx();
            var parentNode = node.VisualParents().Skip(1).OfType<TreeViewItem>().FirstEx();

            var rules = (TypeRuleBuilder)parentNode.DataContext;
            var condition = (TypeConditionRuleBuilder)node.DataContext;
            rules.Conditions.Remove(condition); 
        }

        private void btRemove_Click(object sender, RoutedEventArgs e)
        {
            CleanSelection(this.treeView);
        }

        private void CleanSelection(TreeView tree)
        {
            if (tree == null)
                return;
            tbType.Text = "";

            foreach (var item in tree.Items)
            {
                var i = item as NamespaceNode;
                if (i != null)
                {
                    i.Selected = true;
                    i.SelectedFind = false;
                    foreach (var subitem in i.SubNodes)
                    {
                            subitem.Selected = true;
                            subitem.SelectedFind = false;
                    }
                }
            }
        }

        private void Find(TreeView tree, string key)
        {
            if (tree == null)
                return;
            foreach (var item in tree.Items)
            {
                var i = item as NamespaceNode;
                if (i != null)
                {
                    foreach (var subitem in i.SubNodes)
                    {
                        if (subitem.Resource.ClassName.ToUpper().Contains(key.Trim().ToUpper()))
                        {
                            subitem.Selected = true;
                            subitem.SelectedFind = true;
                        }
                        else
                        {
                            subitem.Selected = false;
                             subitem.SelectedFind = false;
                        }
                    }

                    if (i.SubNodes.Any(e => e.SelectedFind))
                    {
                        i.Selected = true;
                        foreach (var si in i.SubNodes)
                        {
                            si.Selected = true;
                        }
                    }
                    else
                        if (i.Name.ToUpper().Contains(key.Trim().ToUpper()))
                        {
                            i.Selected = true;
                            i.SelectedFind = true;
                            foreach (var si in i.SubNodes)
                            {
                                si.Selected = true;
                            }           
                        }
                        else
                        i.Selected = false;
                }
            }
        }

        private void tbType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                CleanSelection(this.treeView);
                e.Handled = true;
            }
            else
                if (e.Key == Key.Enter)
                {
                    Find(this.treeView, tbType.Text);
                    e.Handled = true;
                }
        }

        private void tbType_KeyUp(object sender, KeyEventArgs e)
        {
            if (tbType.Text.IsEmpty())
            {
                CleanSelection(this.treeView);
                return;
            }
            Find(this.treeView, tbType.Text);
        }
    }

    public class NamespaceNode : ModelEntity
    {
        public string Name { get; set; }

        bool selected = true;
        public bool Selected
        {
            get { return selected; }
            set { Set(ref selected, value); }
        }

        bool selectedFind;
        public bool SelectedFind
        {
            get { return selectedFind; }
            set { Set(ref selectedFind, value); }
        }

        public List<TypeRuleBuilder> SubNodes { get; set; } //Will be TypeAccesRule or NamespaceNode
    }

    public class TypeRuleBuilder : ModelEntity
    {
        bool selected = true;
        public bool Selected
        {
            get { return selected; }
            set { Set(ref selected, value); }
        }

        bool selectedFind;
        public bool SelectedFind
        {
            get { return selectedFind; }
            set { Set(ref selectedFind, value); }
        }


        [NotifyChildProperty]
        TypeAllowedBuilder allowed;
        public TypeAllowedBuilder Allowed
        {
            get { return allowed; }
            set { Set(ref allowed, value); }
        }

        TypeAllowedAndConditions allowedBase;
        public TypeAllowedAndConditions AllowedBase
        {
            get { return allowedBase; }
        }

        [NotifyChildProperty, NotifyCollectionChanged]
        MList<TypeConditionRuleBuilder> conditions = new MList<TypeConditionRuleBuilder>();
        public MList<TypeConditionRuleBuilder> Conditions
        {
            get { return conditions; }
            set { Set(ref conditions, value); }
        }

        ReadOnlyCollection<TypeConditionSymbol> availableConditions;
        public ReadOnlyCollection<TypeConditionSymbol> AvailableConditions
        {
            get { return availableConditions; }
            set { Set(ref availableConditions, value); }
        }

        public TypeDN Resource { get; set; }

        public AuthThumbnail? Properties { get; set; }
        public AuthThumbnail? Operations { get; set; }
        public AuthThumbnail? Queries { get; set; }

        public TypeRuleBuilder(TypeAllowedRule rule)
        {
            this.allowed = new TypeAllowedBuilder(rule.Allowed.Fallback);
            this.conditions = rule.Allowed.Conditions.Select(c => new TypeConditionRuleBuilder(c.TypeCondition, c.Allowed)).ToMList();
            this.availableConditions = rule.AvailableConditions;
            this.allowedBase = rule.AllowedBase; 
            this.Resource = rule.Resource;

            this.Properties = rule.Properties;
            this.Operations = rule.Operations;
            this.Queries = rule.Queries;

            this.RebindEvents();
        }

        protected override void ChildPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify(() => Overriden);

            base.ChildPropertyChanged(sender, e);
        }

        protected override void ChildCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            Notify(() => Overriden);
            Notify(() => CanAdd);

            base.ChildCollectionChanged(sender, args);
        }

        public TypeAllowedRule ToRule()
        {
            return new TypeAllowedRule
            {
                Resource = Resource,

                AllowedBase = AllowedBase,
                Allowed = new TypeAllowedAndConditions(Allowed.TypeAllowed, 
                    Conditions.Select(a=>new TypeConditionRule(a.TypeCondition, a.Allowed.TypeAllowed)).ToReadOnly()),

                AvailableConditions = this.AvailableConditions,

                Properties = Properties,
                Operations = Operations,
                Queries = Queries,
            };
        }

    

        public bool Overriden
        {
            get 
            {
                if (!allowedBase.Fallback.Equals(Allowed.TypeAllowed))
                    return true;

                return !allowedBase.Conditions.SequenceEqual(Conditions.Select(a => 
                    new TypeConditionRule(a.TypeCondition, a.Allowed.TypeAllowed)));
            }
        }

        public bool CanAdd
        {
            get { return availableConditions.Except(Conditions.Select(a => a.TypeCondition)).Any(); }
        }
    }

    public class TypeConditionRuleBuilder : ModelEntity
    {
        [NotifyChildProperty]
        TypeAllowedBuilder allowed;
        public TypeAllowedBuilder Allowed
        {
            get { return allowed; }
            private set { Set(ref allowed, value); }
        }

        public TypeConditionSymbol TypeCondition { get; private set; }

        public string ConditionNiceToString { get { return TypeCondition.NiceToString(); } }

        public TypeConditionRuleBuilder(TypeConditionSymbol typeCondition, TypeAllowed allowed)
        {
            this.TypeCondition = typeCondition;
            this.Allowed = new TypeAllowedBuilder(allowed);
        }

        protected override void ChildPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify(() => Allowed);
        }
    }

    public class TypeAllowedBuilder : ModelEntity
    {
        public TypeAllowedBuilder(TypeAllowed typeAllowed)
        {
            this.typeAllowed = typeAllowed;
        }

        TypeAllowed typeAllowed;
        public TypeAllowed TypeAllowed { get { return typeAllowed; } }

        public bool Create
        {
            get { return typeAllowed.IsActive(TypeAllowedBasic.Create); }
            set { Set(TypeAllowedBasic.Create); }
        }

        public bool Modify
        {
            get { return typeAllowed.IsActive(TypeAllowedBasic.Modify); }
            set { Set(TypeAllowedBasic.Modify); }
        }

        public bool Read
        {
            get { return typeAllowed.IsActive(TypeAllowedBasic.Read); }
            set { Set(TypeAllowedBasic.Read); }
        }

        public bool None
        {
            get { return typeAllowed.IsActive(TypeAllowedBasic.None); }
            set { Set(TypeAllowedBasic.None); }
        }

        public int GetNum()
        {
            return (Create ? 1 : 0) + (Modify ? 1 : 0) + (Read ? 1 : 0) + (None ? 1 : 0);
        }

        private void Set(TypeAllowedBasic typeAllowedBasic)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.Shift)
            {
                typeAllowed = TypeAllowedExtensions.Create(typeAllowedBasic, typeAllowedBasic);
            }
            else
            {
                int num = GetNum();
                if (!typeAllowed.IsActive(typeAllowedBasic) && num == 1)
                {
                    var db = typeAllowed.GetDB();
                    typeAllowed = TypeAllowedExtensions.Create(
                        db > typeAllowedBasic ? db : typeAllowedBasic,
                        db < typeAllowedBasic ? db : typeAllowedBasic);
                }
                else if (typeAllowed.IsActive(typeAllowedBasic) && num >= 2)
                {
                    var other = typeAllowed.GetDB() == typeAllowedBasic ? typeAllowed.GetDB() : typeAllowed.GetUI();
                    typeAllowed = TypeAllowedExtensions.Create(other, other);
                }
            }

            Notify(() => Create);
            Notify(() => Modify);
            Notify(() => Read);
            Notify(() => None);
            Notify(() => TypeAllowed);
        }
    }

    public static class TypeRuleConverter
    {
        public static readonly IValueConverter BoolToYellowOrTransparent = ConverterFactory.New(
            (bool value) => value ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF88")): Brushes.Transparent);
    }
}
