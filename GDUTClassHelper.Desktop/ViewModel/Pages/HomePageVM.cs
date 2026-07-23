using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using GDUTClassHelper.Desktop.Common.Bases;

namespace GDUTClassHelper.Desktop.ViewModel.Pages
{
    public partial class HomePageVM : ViewModelBase
    {
        [RelayCommand]
        private void OpenWebsite(string url) => Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
    }
}
