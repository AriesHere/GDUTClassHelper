using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using GDUTClassHelper.Core.Common.Type;

namespace GDUTClassHelper.Desktop.ViewModel.Pages
{
    public partial class CalendarPageVM : ObservableObject
    {
        private LessonCollection _lessons;

        public ObservableCollection<LessonWrapper> ThisWeekLessons { get; private set; } = [];

        [ObservableProperty] public partial int Week { get; set; }

        partial void OnWeekChanged(int value)
        {
            ThisWeekLessons.Clear();
            foreach (var lesson in _lessons)
            {
                if (lesson.Week == value)
                {
                    ThisWeekLessons.Add(new(lesson));
                }
                else if (lesson.Week > value)
                {
                    break;
                }
            }
        }

        public CalendarPageVM(LessonCollection lessons)
        {
            _lessons = lessons;
            Week = 1;
            // Test
            ThisWeekLessons.Add(new(new() { Name = "AAA", Sessions = [2, 3], DayOfWeek = 3 }));
        }
    }

    public partial class LessonWrapper(Lesson lesson) : ObservableObject
    {
        private Lesson _lesson { get; set; } = lesson;

        public string Name => lesson.Name;

        public int DayOfWeek => _lesson.DayOfWeek;

        public List<int> Sessions => _lesson.Sessions;

        [ObservableProperty] public partial bool IsConflict { get; set; } = false;
    }
}
