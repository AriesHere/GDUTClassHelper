using System.IO;
using System.Windows;
using GDUTClassHelper.Desktop.ViewModel;
using GDUTClassHelper.Desktop.ViewModel.Pages;
using Microsoft.Extensions.DependencyInjection;

namespace GDUTClassHelper.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly string DataFileDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

        public static ServiceProvider ServiceProvider;

        App()
        {
            Directory.CreateDirectory(DataFileDir);

            ServiceCollection sc = new();
            sc.AddSingleton<MainWindowVM>();
            sc.AddSingleton<CalendarPageVM>();
            sc.AddSingleton<DataPageVM>();
            sc.AddSingleton<HomePageVM>();
            ServiceProvider = sc.BuildServiceProvider();
        }
    }

}
