using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GDUTClassHelper.Core.Common.Type;
using GDUTClassHelper.Desktop.Common.Bases;
using Microsoft.Extensions.DependencyInjection;

namespace GDUTClassHelper.Desktop.ViewModel.Pages
{
    public partial class CalendarPageVM : ViewModelBase
    {
        public MainWindowVM mainVM;
        public List<(string FileName, LessonCollection Lessons)> LessonsCollectionList = [];

        public ObservableCollection<LessonWrapper> ThisWeekLessonsL { get; private set; } = [];
        public ObservableCollection<LessonWrapper> ThisWeekLessonsR { get; private set; } = [];

        [ObservableProperty] public partial int Week { get; set; }
        [ObservableProperty] public partial int TotalConflict { get; set; }
        partial void OnWeekChanged(int value) => UpdateThisWeekLessons(value);

        public CalendarPageVM()
        {
            this.mainVM = App.ServiceProvider.GetRequiredService<MainWindowVM>();
            RefreshLessons();
        }

        [RelayCommand]
        private void PrevWeek() { if (Week > 1) Week--; }

        [RelayCommand]
        private void NextWeek() => Week++;

        public void RefreshLessons()
        {
            try { LessonsCollectionList = App.ServiceProvider.GetRequiredService<DataPageVM>().GetLessonsList(); }
            catch (Exception e) { mainVM.Status = new() { InfoText = $"读取数据时发生错误。{e.Message}", StatusType = StatusBarInfoType.Errored }; }
            if (LessonsCollectionList.Count == 2)
            {
                var listA = LessonsCollectionList[0].Lessons;
                var listB = LessonsCollectionList[1].Lessons;
                var groupsA = listA.GroupBy(l => l.Week).ToDictionary(g => g.Key);
                var groupsB = listB.GroupBy(l => l.Week).ToDictionary(g => g.Key);
                int total = 0;
                foreach (var week in groupsA.Keys.Intersect(groupsB.Keys))
                {
                    var lessonsInA = groupsA[week];
                    var lessonsInB = groupsB[week];
                    foreach (var lessonA in lessonsInA)
                    {
                        foreach (var lessonB in lessonsInB)
                        {
                            if (lessonA.DayOfWeek == lessonB.DayOfWeek
                                && lessonA.Sessions.Intersect(lessonB.Sessions).Any())
                            {
                                total++;
                            }
                        }
                    }
                }
                TotalConflict = total;
            }
            else
            {
                TotalConflict = 0;
            }
            UpdateThisWeekLessons();
        }

        private void UpdateThisWeekLessons(int week = 1)
        {
            var count = LessonsCollectionList.Count;
            if (count > 0)
            {
                ThisWeekLessonsL.Clear();
                foreach (var lesson in LessonsCollectionList[0].Lessons)
                {
                    if (lesson.Week == week)
                        ThisWeekLessonsL.Add(new(lesson));
                    else if (lesson.Week > week)
                        break;
                }
            }
            if (count > 1)
            {
                ThisWeekLessonsR.Clear();
                foreach (var lesson in LessonsCollectionList[1].Lessons)
                {
                    if (lesson.Week == week)
                        ThisWeekLessonsR.Add(new(lesson));
                    else if (lesson.Week > week)
                        break;
                }
                foreach (var itemL in ThisWeekLessonsL)
                {
                    foreach (var itemR in ThisWeekLessonsR)
                    {
                        if (itemL.DayOfWeek == itemR.DayOfWeek
                            && itemL.Sessions.Intersect(itemR.Sessions).Any())
                        {
                            itemL.IsConflict = itemR.IsConflict = true;
                        }
                    }
                }
            }
            if (count > 2)
            {
                mainVM.Status = new() { InfoText = "在 Data 页面中选择了超过两个数据，此处最多只能显示两个，建议只选择两个数据", StatusType = StatusBarInfoType.Warning };
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
