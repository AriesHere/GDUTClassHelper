using System.Globalization;
using System.Windows.Data;

namespace GDUTClassHelper.Desktop.Common.Converter
{
    /// <summary>
    /// Calendar
    /// </summary>
    public class ColumnPositionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double canvasActualWidth = 0;
            int columnIndex = 0;
            foreach (var item in values)
            {
                if (item is int index)
                {
                    columnIndex = index;
                }
                else if (item is double width)
                {
                    canvasActualWidth = width;
                }
            }
            return columnIndex * (canvasActualWidth / 7);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
