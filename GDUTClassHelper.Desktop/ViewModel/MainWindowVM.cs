using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using GDUTClassHelper.Core.Common.Type;
using GDUTClassHelper.Desktop.Common.Bases;
using GDUTClassHelper.Desktop.View.Pages;
using GDUTClassHelper.Desktop.ViewModel.Pages;

namespace GDUTClassHelper.Desktop.ViewModel
{
    public partial class MainWindowVM : ViewModelBase
    {
        [ObservableProperty] public partial Page SelectedPage { get; set; }

        public MainWindowVM()
        {
            SelectedPage = new CalendarPage();
        }

        partial void OnSelectedPageChanging(Page value)
        {
            if (value is CalendarPage)
            {
                value.DataContext = new CalendarPageVM();
            }
        }
    }
}
