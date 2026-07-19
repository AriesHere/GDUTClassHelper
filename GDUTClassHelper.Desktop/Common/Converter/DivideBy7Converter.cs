using System.Globalization;
using System.Windows.Data;

namespace GDUTClassHelper.Desktop.Common.Converter
{
    /// <summary>
    /// Calendar
    /// </summary>
    /// <remarks>Binding to the ActualWidth of Canvas</remarks>
    public class DivideBy7Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double width)
            {
                return width / 7.0;
            }
            else
            {
                return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
