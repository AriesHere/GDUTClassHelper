using System;
using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using GDUTClassHelper.Core.Common.Type;
using GDUTClassHelper.Desktop.Common.Bases;

namespace GDUTClassHelper.Desktop.ViewModel.Pages
{
    public partial class CalendarPageVM : ViewModelBase
    {
        private LessonCollection _lessons = [];

        public ObservableCollection<LessonWrapper> ThisWeekLessons { get; private set; } = [];

        [ObservableProperty] public partial int Week { get; set; }
        partial void OnWeekChanged(int value) => UpdateThisWeekLessons(value);

        [ObservableProperty] public partial string CurrentFilePath { get; set; } = string.Empty;
        partial void OnCurrentFilePathChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string f = File.ReadAllText(value);
                _lessons = [];
                if (Path.GetExtension(value) == ".json")
                {
                    try { _lessons = LessonCollection.ReadFromJsonWithHeader(f); }
                    catch
                    {
                        try { _lessons = LessonCollection.ReadFromJson(f); }
                        catch { }
                    }
                }
                else
                {
                    try { _lessons = LessonCollection.ReadFromText(f); }
                    catch { }
                }
                if (_lessons.Count > 0) UpdateThisWeekLessons(1);
            }
        }

        private void UpdateThisWeekLessons(int week)
        {
            ThisWeekLessons.Clear();
            foreach (var lesson in _lessons)
            {
                if (lesson.Week == week)
                {
                    ThisWeekLessons.Add(new(lesson));
                }
                else if (lesson.Week > week)
                {
                    break;
                }
            }
        }
    }

    public partial class LessonWrapper(Lesson lesson) : ObservableObject
    {
#pragma warning disable CS9124
        private Lesson _lesson { get; set; } = lesson;
#pragma warning restore CS9124

        public string Name => lesson.Name;

        public int DayOfWeek => _lesson.DayOfWeek - 1;

        public string Teacher => _lesson.Teacher;

        public List<int> Sessions => _lesson.Sessions;

        [ObservableProperty] public partial bool IsConflict { get; set; } = false;
    }
}
