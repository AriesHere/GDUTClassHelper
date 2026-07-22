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
                    string error = string.Empty;
                    if (Path.GetExtension(fileName).CompareTo(".json") == 0)
                    {
                        try { lessons = LessonCollection.ReadFromJsonWithHeader(f); }
                        catch
                        {
                            try { lessons = LessonCollection.ReadFromJson(f); }
                            catch (Exception e) { error = e.Message; }
                        }
                    }
                    else
                    {
                        try { lessons = LessonCollection.ReadFromText(f); }
                        catch (Exception e) { error = e.Message; }
                    }
                    
                    if (lessons.Count > 0) mainVM.Status = new("解析完成，请及时保存数据，否则可能会被后续数据覆盖", StatusBarInfoType.Succeeded);
                    else mainVM.Status = new($"读取时发生错误。{error}", StatusBarInfoType.Errored);
                }
            }
            RefreshFileNameList();
        }

        [RelayCommand]
        private void SaveData()
        {
            try
            {
                lessons.Save(Path.Combine(App.DataFileDir, SaveFileName));
                mainVM.Status = new($"保存成功", StatusBarInfoType.Succeeded);
                RefreshFileNameList();
            }
            catch (Exception e)
            {
                mainVM.Status = new($"保存失败。{e.Message}", StatusBarInfoType.Errored);
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
                    using FileStream fileStream = new(item, FileMode.Open, FileAccess.Read, FileShare.Read, sizeof(int));
                    using BinaryReader reader = new(fileStream);
                    FileNameList.Add(new(item[index..], (Status)reader.ReadInt32()));
                }
            }
        }

        [RelayCommand]
        private void OpenDataDir()
        {
            if (Directory.Exists(App.DataFileDir))
            {
                try
                {
                    Process.Start(new ProcessStartInfo(App.DataFileDir) { UseShellExecute = true });
                    mainVM.Status = new("", StatusBarInfoType.None);
                }
                catch (Exception e)
                {
                    mainVM.Status = new($"打开失败。{e.Message}", StatusBarInfoType.Errored);
                }
            }
            else
            {
                mainVM.Status = new("目标文件夹不存在", StatusBarInfoType.Errored);
            }
        }
    }

    public struct ItemsControlItem(string name, Status status = Status.Indeterminate, bool isSelected = false)
    {
        public string Name { get; set; } = name;
        public Status Status { get; set; } = status;
        public bool IsSelected { get; set; } = isSelected;

        public static implicit operator ItemsControlItem(string s) => new(s);
    }
}
