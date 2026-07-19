using System.Globalization;
using System.Windows.Data;

namespace GDUTClassHelper.Desktop.Common.Converter
{
    public class SessionToHeightConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double height = 0.0;
            int c = 0;
            foreach (var item in values)
            {
                if (item is double h)
                {
                    height = h;
                }
                else if (item is List<int> l)
                {
                    c = l.Count;
                }
            }
            return height * c;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class SessionToTopConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double height = 0.0;
            int c = 0;
            foreach (var item in values)
            {
                if (item is double h)
                {
                    height = h;
                }
                else if (item is List<int> l)
                {
                    c = l[0] - 1;
                }
            }
            return height * c;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
