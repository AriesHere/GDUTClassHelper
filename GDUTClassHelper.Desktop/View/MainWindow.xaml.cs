using System.Windows;
using GDUTClassHelper.Desktop.ViewModel;

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
            this.DataContext = new MainWindowVM();
        }
    }
}