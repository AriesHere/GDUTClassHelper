using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using CommunityToolkit.Mvvm.Collections;
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
        partial void OnSaveFileNameChanged(string value)
        {
            IsShowHintText = string.IsNullOrEmpty(value) ? Visibility.Visible : Visibility.Collapsed;
        }
        [ObservableProperty] public partial Visibility IsShowHintText { get; set; } = Visibility.Visible;
        [ObservableProperty] public partial List<string> FileNameList { get; set; } = [];

        public ObservableCollection<string> SelectedFileNameList { get; set; } = [];

        private LessonCollection lessons = [];
        private MainWindowVM mainVM;

        public DataPageVM(MainWindowVM mainVM)
        {
            this.mainVM = mainVM;
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
                    mainVM.StatusBarText = lessons.Count > 0 ? "解析完成，请及时保存数据，否则可能会被后续数据覆盖" : $"读取时发生错误。{error}";
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
            }
            catch (Exception e)
            {
                mainVM.StatusBarText = $"保存失败。{e.Message}";
            }
        }

        [RelayCommand]
        private void RefreshFileNameList()
        {
            FileNameList.Clear();
            FileNameList = [..Directory.GetFiles(App.DataFileDir)];
        }
    }
}
