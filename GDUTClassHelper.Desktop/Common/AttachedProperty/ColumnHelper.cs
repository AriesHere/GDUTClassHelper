using System.Windows;

namespace GDUTClassHelper.Desktop.Common.AttachedProperty;

public class ColumnHelper
{
    public static readonly DependencyProperty ColumnProperty =
        DependencyProperty.RegisterAttached("Column", typeof(int), typeof(ColumnHelper), new PropertyMetadata(0));

    public static int GetColumn(DependencyObject obj) => (int)obj.GetValue(ColumnProperty);

    public static void SetColumn(DependencyObject obj, int value) => obj.SetValue(ColumnProperty, value);
}
