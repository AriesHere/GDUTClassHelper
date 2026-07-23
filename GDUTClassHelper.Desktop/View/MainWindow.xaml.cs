using System.Windows;
using GDUTClassHelper.Desktop.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace GDUTClassHelper.Desktop.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = App.ServiceProvider.GetRequiredService<MainWindowVM>();
        }
    }
}