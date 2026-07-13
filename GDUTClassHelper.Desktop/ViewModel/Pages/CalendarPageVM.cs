using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using GDUTClassHelper.Core.Common;

namespace GDUTClassHelper.Desktop.ViewModel.Pages
{
    public partial class CalendarPageVM : ObservableObject
    {
        [ObservableProperty] private ObservableCollection<ColInfo> _colInfos = [];
    }

    public partial class ColInfo : ObservableObject
    {
        [ObservableProperty] private DayOfWeek _dayOfWeek;
    }
}
