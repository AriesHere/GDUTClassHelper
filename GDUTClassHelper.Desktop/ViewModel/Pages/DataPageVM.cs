using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GDUTClassHelper.Core.Common.Type;
using GDUTClassHelper.Desktop.Common.Bases;
using Microsoft.Win32;

namespace GDUTClassHelper.Desktop.ViewModel.Pages
{
    public partial class DataPageVM : ViewModelBase
    {
        [ObservableProperty] public partial string SaveFileName { get; set; } = string.Empty;
        partial void OnSaveFileNameChanging(string value)
        {
            HintTextVisibility = string.IsNullOrEmpty(value) ? Visibility.Visible : Visibility.Collapsed;
        }
        [ObservableProperty] public partial Visibility HintTextVisibility { get; set; } = Visibility.Visible;

        public ObservableCollection<ItemsControlItem> FileNameList { get; set; } = [];

        private LessonCollection lessons = [];
        private readonly MainWindowVM mainVM;

        public DataPageVM(MainWindowVM mainVM)
        {
            this.mainVM = mainVM;
            RefreshFileNameList();
        }

        [RelayCommand]
        private void BrowseAndAnaylsisFile()
        {
            OpenFileDialog dialog = new();
            if (dialog.ShowDialog() == true)
            {
                var fileName = dialog.FileName;
                if (!string.IsNullOrEmpty(fileName))
                {
                    string f = File.ReadAllText(fileName);
                    List<string> errors = [];
                    bool success = false;
                    if (Path.GetExtension(fileName) == ".json")
                    {
                        try { lessons = LessonCollection.ReadFromJsonWithHeader(f); success = true; }
                        catch (Exception e) { errors.Add(e.Message); }
                        if (!success)
                        {
                            try { lessons = LessonCollection.ReadFromJson(f); success = true; }
                            catch (Exception e) { errors.Add(e.Message); }
                        }
                    }
                    else
                    {
                        try { lessons = LessonCollection.ReadFromText(f); success = true; }
                        catch (Exception e) { errors.Add(e.Message); }
                    }
                    Debug.WriteLine(f);
                    if (success) mainVM.Status = new() { InfoText = "解析完成，请及时保存数据，否则可能会被后续数据覆盖", StatusType = StatusBarInfoType.Warning };
                    else mainVM.Status = new() { InfoText = $"读取时发生错误。{string.Join("; ", errors)}", StatusType = StatusBarInfoType.Errored };
                }
                RefreshFileNameList();
            }
        }

        [RelayCommand]
        private void SaveData(string? path = null)
        {
            try
            {
                lessons.Save(path ?? Path.Combine(App.DataFileDir, SaveFileName));
                mainVM.Status = new() { InfoText = "保存成功", StatusType = StatusBarInfoType.Succeeded };
                RefreshFileNameList();
            }
            catch (Exception e)
            {
                mainVM.Status = new() { InfoText = $"保存失败。{e.Message}", StatusType = StatusBarInfoType.Errored };
            }
        }

        [RelayCommand]
        private void RefreshFileNameList()
        {
            FileNameList.Clear();
            var f = Directory.GetFiles(App.DataFileDir);
            if (f is not null && f.Length > 0)
            {
                var index = App.DataFileDir.Length + 1;
                foreach (var item in f)
                {
                    FileNameList.Add(new(item[index..], LessonCollection.GetStatusFromFile(item)));
                }
            }
        }

        [RelayCommand]
        private void MergeIntoSeletedFile()
        {
            var selected = FileNameList.Where(f => f.IsSelected).ToList();
            Debug.WriteLine(selected.Count);
            if (selected.Count != 1)
            {
                mainVM.Status = new() { InfoText = $"只有在选择一个文件时才可以进行执行操作，当前选择了 {selected.Count} 个", StatusType = StatusBarInfoType.Errored };
                return;
            }
            var path = Path.Combine(App.DataFileDir, selected.FirstOrDefault()!.Name);
            LessonCollection l;
            try { l = LessonCollection.Load(path); }
            catch (Exception e)
            {
                mainVM.Status = new() { InfoText = $"读取时发生错误。{e.Message}", StatusType = StatusBarInfoType.Errored };
                return;
            }
            if (l.Status == Status.Complete)
            {
                mainVM.Status = new() { InfoText = "无法将数据合并进入已完善的文件", StatusType = StatusBarInfoType.Errored };
                return;
            }
            BitArray? d = l.GetReadFlag();
            if (d is not null && lessons.Total != 0)
            {
                if (d.Count != lessons.Total)
                {
                    mainVM.Status = new() { InfoText = "二者数据上限不一致，请检查数据是否有误", StatusType = StatusBarInfoType.Errored };
                }
                foreach (var item in lessons)
                {
                    if (!d[item.Number - 1])
                    {
                        l.Add(item);
                    }
                }
                l.Status = (l.Total == l.Count) ? Status.Complete : Status.Incomplete;
            }
            else if (d is not null && lessons.Total == 0)
            {
                foreach (var item in lessons.Lessons) l.Add(item);
                l.Total = 0;
                l.Status = l.Count == d.Length ? Status.Complete : Status.Indeterminate;
            }
            else
            {
                foreach (var item in lessons.Lessons) l.Add(item);
                l.Total = 0;
                l.Status = Status.Indeterminate;
            }
            lessons = l;
            SaveData(path);
        }

        [RelayCommand]
        private void OpenDataDir()
        {
            if (Directory.Exists(App.DataFileDir))
            {
                try
                {
                    Process.Start(new ProcessStartInfo(App.DataFileDir) { UseShellExecute = true });
                    mainVM.Status = new() {  };
                }
                catch (Exception e)
                {
                    mainVM.Status = new() { InfoText = $"打开失败。{e.Message}", StatusType = StatusBarInfoType.Errored };
                }
            }
            else
            {
                mainVM.Status = new() { InfoText = "目标文件夹不存在", StatusType = StatusBarInfoType.Errored };
            }
        }
    }

    public class ItemsControlItem(string name, Status status = Status.Indeterminate, bool isSelected = false)
    {
        public string Name { get; set; } = name;
        public Status Status { get; set; } = status;
        public bool IsSelected { get; set; } = isSelected;

        public static implicit operator ItemsControlItem(string s) => new(s);
    }
}
