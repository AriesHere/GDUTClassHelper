using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace GDUTClassHelper.Desktop.Common.AttachedProperty
{
    class ValueTypesHelper
    {
        public static readonly DependencyProperty IntProperty =
            DependencyProperty.RegisterAttached("Int", typeof(int), typeof(ValueTypesHelper), new PropertyMetadata(0));

        public static int GetInt(DependencyObject obj) => (int)obj.GetValue(IntProperty);

        public static void SetInt(DependencyObject obj, int value) => obj.SetValue(IntProperty, value);

        public static readonly DependencyProperty DoubleProperty =
            DependencyProperty.RegisterAttached("Double", typeof(double), typeof(ValueTypesHelper), new PropertyMetadata(0d));

        public static double GetDouble(DependencyObject obj) => (double)obj.GetValue(DoubleProperty);

        public static void SetDouble(DependencyObject obj, double value) => obj.SetValue(DoubleProperty, value);
    }
}
