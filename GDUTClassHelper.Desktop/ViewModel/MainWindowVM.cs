using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using GDUTClassHelper.Desktop.Common.Bases;
using GDUTClassHelper.Desktop.View.Pages;
using GDUTClassHelper.Desktop.ViewModel.Pages;

namespace GDUTClassHelper.Desktop.ViewModel
{
    public partial class MainWindowVM : ViewModelBase
    {
        [ObservableProperty] public partial string SelectedItem { get; set; }
        partial void OnSelectedItemChanged(string value)
        {
            Type? t = Type.GetType("GDUTClassHelper.Desktop.View.Pages." + value + "Page");
            if (t is not null)
            {
                CurrentPage = (Page)Activator.CreateInstance(t)!;
                if (CurrentPage is CalendarPage)
                {
                    CurrentPage.DataContext = new CalendarPageVM();
                }
                else if (CurrentPage is DataPage)
                {
                    CurrentPage.DataContext = new DataPageVM(this);
                }
            }
        }
        [ObservableProperty] public partial Page CurrentPage { get; set; }
        [ObservableProperty] public partial string StatusBarText { get; private set; } = string.Empty;
        [ObservableProperty] public partial Brush StatusBarColor { get; private set; } = Brushes.Transparent;

        public StatusBarInfo Status 
        {
            set
            {
                StatusBarText = $" [{DateTime.Now:HH:mm:ss.fff}] "+ value.InfoText;
                StatusBarColor = value.StatusType switch
                {
                    StatusBarInfoType.None => Brushes.Transparent,
                    StatusBarInfoType.Succeeded => Brushes.LightSeaGreen,
                    StatusBarInfoType.Warning => Brushes.Gold,
                    StatusBarInfoType.Errored => Brushes.IndianRed,
                    _ => Brushes.Transparent,
                };
            }
        }

        public ObservableCollection<string> Pages { get; set; } = ["Data", "Calendar"];

#pragma warning disable CS9264
        public MainWindowVM()
        {
            SelectedItem = Pages[0];
            Status = new StatusBarInfo();
        }
#pragma warning restore CS9264
    }

    public class StatusBarInfo
    {
        public string InfoText = "就绪";
        public StatusBarInfoType StatusType = StatusBarInfoType.None;
    }

    public enum StatusBarInfoType
    {
        None = 0,
        Succeeded = 1,
        Warning = 2,
        Errored = 3,
    }
}
