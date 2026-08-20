using System.IO;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FFDrop.Properties;

namespace FFDrop;

internal partial class SettingsViewModel : ObservableObject
{
    private readonly IDialogs _dialogs;

    [ObservableProperty]
    public partial string OutputDirectory { get; set; }

    partial void OnOutputDirectoryChanged(string value)
    {
        Settings.Default.OutputDirectory = value;
        Settings.Default.Save();
    }

    [ObservableProperty]
    public partial bool CreateShellConfirmRun { get; set; }

    partial void OnCreateShellConfirmRunChanged(bool value)
    {
        Settings.Default.CreateShellConfirmRun = value;
        Settings.Default.Save();
    }

    [ObservableProperty]
    public partial bool CreateShellRun { get; set; }

    partial void OnCreateShellRunChanged(bool value)
    {
        Settings.Default.CreateShellRun = value;
        Settings.Default.Save();
    }

    [ObservableProperty]
    public partial bool CreateShell { get; set; }

    partial void OnCreateShellChanged(bool value)
    {
        Settings.Default.CreateShell = value;
        Settings.Default.Save();
    }

    [ObservableProperty]
    public partial bool AllwaysOnTop { get; set; }

    partial void OnAllwaysOnTopChanged(bool value)
    {
        Settings.Default.AllwaysOnTop = value;
        Settings.Default.Save();
    }

    [ObservableProperty]
    public partial bool PresetsExpanded { get; set; }

    partial void OnPresetsExpandedChanged(bool value)
    {
        Settings.Default.PresetsExpanded = value;
        Settings.Default.Save();
    }

    [ObservableProperty]
    public partial bool CreateM3U { get; set; }

    partial void OnCreateM3UChanged(bool value)
    {
        Settings.Default.CreateM3U = value;
        Settings.Default.Save();
    }

    [ObservableProperty]
    public partial bool CreatePls { get; set; }

    partial void OnCreatePlsChanged(bool value)
    {
        Settings.Default.CreatePls = value;
        Settings.Default.Save();
    }

    public SettingsViewModel(IDialogs dialogs)
    {
        _dialogs = dialogs;

        OutputDirectory = !Directory.Exists(Settings.Default.OutputDirectory) 
            ? Environment.CurrentDirectory 
            : Settings.Default.OutputDirectory;

        CreateShellConfirmRun = Settings.Default.CreateShellConfirmRun;
        CreateShellRun = Settings.Default.CreateShellRun;
        CreateShell = Settings.Default.CreateShell;
        AllwaysOnTop = Settings.Default.AllwaysOnTop;
        PresetsExpanded = Settings.Default.PresetsExpanded;
        CreateM3U = Settings.Default.CreateM3U;
        CreatePls = Settings.Default.CreatePls;
    }

    [RelayCommand]
    private void Browse()
    {
        string? selection = _dialogs.SelectFolderDialog(OutputDirectory);
        if (!string.IsNullOrEmpty(selection))
        {
            OutputDirectory = selection;
        }
    }
}
