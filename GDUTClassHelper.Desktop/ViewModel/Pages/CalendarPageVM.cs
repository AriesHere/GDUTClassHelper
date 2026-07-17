using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using GDUTClassHelper.Core.Common;
using GDUTClassHelper.Core.Common.Type;

namespace GDUTClassHelper.Desktop.ViewModel.Pages
{
    public partial class CalendarPageVM : ObservableObject
    {
        [ObservableProperty] private LessonCollection _lessons = [];
    }
}
