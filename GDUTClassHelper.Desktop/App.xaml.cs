using System.IO;
using System.Windows;

namespace GDUTClassHelper.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly string DataFileDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

        App()
        {
            Directory.CreateDirectory(DataFileDir);
        }
    }

}
