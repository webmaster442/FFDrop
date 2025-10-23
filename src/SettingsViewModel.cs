using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace FFDrop;

internal partial class SettingsViewModel : ObservableObject
{
    private readonly IDialogs _dialogs;

    [ObservableProperty]
    public partial string OutputDirectory { get; set; }

    [ObservableProperty]
    public partial bool CreateShellConfirmRun { get; set; }

    [ObservableProperty]
    public partial bool CreateShellRun { get; set; }

    [ObservableProperty]
    public partial bool CreateShell { get; set; }

    public SettingsViewModel(IDialogs dialogs)
    {
        OutputDirectory = Environment.CurrentDirectory;
        CreateShellConfirmRun = true;
        CreateShellRun = false;
        CreateShell = false;
        _dialogs = dialogs;
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
