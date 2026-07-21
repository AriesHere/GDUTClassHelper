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
    public partial class DataPageVM(MainWindowVM mainVM) : ViewModelBase
    {
        [ObservableProperty] public partial string SaveFileName { get; set; } = string.Empty;
        partial void OnSaveFileNameChanging(string value)
        {
            HintTextVisibility = string.IsNullOrEmpty(value) ? Visibility.Visible : Visibility.Collapsed;
        }
        [ObservableProperty] public partial Visibility HintTextVisibility { get; set; } = Visibility.Visible;
        public ObservableCollection<string> FileNameList { get; set; } = [];

        public ObservableCollection<string> SelectedFileNameList { get; set; } = [];

        private LessonCollection lessons = [];
        private readonly MainWindowVM mainVM = mainVM;

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
                StreamWriter writer = new(Path.Combine(App.DataFileDir, SaveFileName));
                writer.Write(lessons);
                mainVM.Status = new($"保存成功", StatusBarInfoType.Succeeded);
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
                    FileNameList.Add(item[index..]);
                }
            }
        }
    }
}
