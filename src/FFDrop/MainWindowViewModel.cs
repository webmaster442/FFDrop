using System.IO;

using CommunityToolkit.Mvvm.ComponentModel;

using FFDrop.DomainServices;
using FFDrop.Model;
using FFDrop.Presets;
using FFDrop.Utils;

namespace FFDrop;

internal sealed partial class MainWindowViewModel : ObservableObject
{
    private readonly string _presetsFile;
    private readonly IDialogs _dialogs;
    private readonly PresetLoader _loader;
    private readonly AppLogic _appLogic;

    public PresetsViewModel PresetsModel { get; }

    public SettingsViewModel Settings { get; }

    [ObservableProperty]
    public partial PresetViewModel? SelectedPreset { get; set; }

    public MainWindowViewModel(IDialogs dialogs)
    {
        _dialogs = dialogs;
        _loader = new PresetLoader();
        _presetsFile = Path.Combine(AppContext.BaseDirectory, "Assets", "presets.json");
        if (!_loader.LoadPresets(_presetsFile))
        {
            _dialogs.ErrorMessage($"Failed to load presets from {_presetsFile}. Please reinstall. App will now exit", "Error loading presets");
            Environment.Exit(1);
        }
        PresetsModel = new PresetsViewModel(_loader.GetPresets());
        
        Settings = new SettingsViewModel(_dialogs);
        _appLogic = new AppLogic(Settings, _dialogs, _loader.GetDialogDefinitions());
    }

    public void HandleDrop(string[] files) 
        => _appLogic.FilesHaveBeenDropped(files, SelectedPreset);

    public async void DisplayFFmpegVersion()
    {
        if (!ProgramFinder.TryFindProgramPath("ffmpeg.exe", out var ffmpegPath))
        {
            _dialogs.ErrorMessage("FFmpeg executable not found. Please ensure ffmpeg.exe is available in the system PATH.", "FFmpeg not found");
            return;
        }

        var version = await ProcessEx.GetProcessOutput(ffmpegPath, "-version");
        var msg = $"""
            {version}
            FFMpeg Path: {ffmpegPath}
            """;

        _dialogs.TextDialog(msg, "FFmpeg Version");
    }
}
