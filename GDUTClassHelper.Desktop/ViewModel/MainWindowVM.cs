using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using GDUTClassHelper.Desktop.Common.Bases;
using GDUTClassHelper.Desktop.View;

namespace GDUTClassHelper.Desktop.ViewModel
{
    public partial class MainWindowVM : ViewModelBase
    {
        [ObservableProperty] private Uri _selectedPage = ViewURI.CalendarPage;
    }
}
