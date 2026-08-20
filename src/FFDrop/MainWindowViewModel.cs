using System.IO;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FFDrop.DomainServices;
using FFDrop.Model;
using FFDrop.Presets;
using FFDrop.Utils;
using FFDrop.Utils.FFProbe;

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

    [RelayCommand]
    public async Task DisplayFFmpegVersion()
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

    [RelayCommand]
    public async Task GetFileMediaInfo()
    {
        if (!ProgramFinder.TryFindProgramPath("ffprobe.exe", out var ffprobePath))
        {
            _dialogs.ErrorMessage("FFProbe executable not found. Please ensure ffprobe.exe is available in the system PATH.", "FFProbe not found");
            return;
        }

        if (_dialogs.SelectMediaFile("Select a media file", out string selectedFile))
        {
            MediaInfoModel? model = await MediaInfo.GetMediaInfo(ffprobePath, selectedFile);
            if (model == null)
            {
                _dialogs.ErrorMessage("Failed to retrieve media info. Please ensure the selected file is a valid media file.", "Media Info Error");
                return;
            }

            _dialogs.ShowFileInfoDialog(model);
        }

    }
}
