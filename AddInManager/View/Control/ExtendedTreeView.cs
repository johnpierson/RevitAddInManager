﻿using System.Windows;

namespace RevitAddinManager.View.Control
{
    public class ExtendedTreeView : System.Windows.Controls.TreeView
    {
        public ExtendedTreeView()
        {
            SelectedItemChanged += ItemChange;
        }

        void ItemChange(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (SelectedItem != null)
            {
                SetValue(SelectedItem_Property, SelectedItem);
            }
        }

        public object SelectedItem_
        {
            get => GetValue(SelectedItem_Property);
            set => SetValue(SelectedItem_Property, value);
        }
        public static readonly DependencyProperty SelectedItem_Property = DependencyProperty.Register("SelectedItem_", typeof(object), typeof(ExtendedTreeView), new UIPropertyMetadata(null));
    }
}
