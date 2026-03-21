using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.Json;

using CommunityToolkit.Mvvm.ComponentModel;

using FFDrop.CustomDialogs;
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
    private readonly Dialogdefinition[] _dialogdefinitions;

    public ObservableCollection<PresetViewModel> Presets { get; }

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
        Presets = new ObservableCollection<PresetViewModel>(_loader.GetPresets());
        Settings = new SettingsViewModel(_dialogs);
        _dialogdefinitions = _loader.GetDialogDefinitions();
    }

    public void HandleDrop(string[] files)
    {
        Powershell powershell = new(updatePathVar: false);

        if (SelectedPreset == null
            || SelectedPreset.AssociatedPreset == null)
        {
            _dialogs.ErrorMessage("Please select a preset first", "No preset selected");
            return;
        }

        if (!ProgramFinder.TryFindProgramPath("ffmpeg.exe", out var ffmpegPath))
        {
            _dialogs.ErrorMessage("FFmpeg executable not found. Please ensure ffmpeg.exe is available in the system PATH.", "FFmpeg not found");
            return;
        }

        var scriptFile = Path.Combine(Settings.OutputDirectory,
                                      Path.ChangeExtension(Path.GetFileName(Settings.OutputDirectory), ".ps1"));

        var title = Path.GetFileName(scriptFile);

        List<string> skipped = new();

        var builder = new PowershellBuilder()
            .WithUtf8Enabled()
            .WithWindowTitle(title)
            .WithClear()
            .WithVariable("ffmpeg", ffmpegPath)
            .WithTerminalProgress(0, 1);

        int current = 1;

        if (!TryGetCustomSelectorValues(out var additonals))
        {
            return;
        }

        foreach (var file in files)
        {
            if (File.Exists(file))
            {
                if (FileRecognizer.IsDropConvertSupported(file))
                {
                    builder
                        .WithCommand(CreateCommandLine(file, "& $ffmpeg", additonals))
                        .WithTerminalProgress(current, files.Length);
                    ++current;
                }
                else if (FileRecognizer.IsPlaylistFile(file)
                    && PlaylistUtils.TryLoadPlaylistItems(file, out var playlistitems))
                {
                    foreach (var item in playlistitems)
                    {
                        if (File.Exists(item)
                            && FileRecognizer.IsDropConvertSupported(item))
                        {
                            builder
                                .WithCommand(CreateCommandLine(item, ffmpegPath, additonals))
                                .WithTerminalProgress(current, files.Length);
                            ++current;
                        }
                        else
                        {
                            skipped.Add(Path.GetFileName(item));
                        }
                    }
                }
                else
                {
                    skipped.Add(Path.GetFileName(file));
                }
            }
            else
            {
                skipped.Add(Path.GetFileName(file));
            }
        }

        builder
            .WithInfoMessageBox("Conversion finished", title)
            .WithTerminalProgrssHidden()
            .WithFolderOpenedInExplorer(Settings.OutputDirectory)
            .WithExit();

        File.WriteAllText(scriptFile, builder.Build());

        if (skipped.Count > 0)
        {
            _dialogs.WarningMessage($"Skipped files:\r\n{string.Join("\r\n", skipped)}", "Skipped files");
        }

        if (Settings.CreateShellConfirmRun)
        {
            bool result = _dialogs.ConfirmMessage("Script created successfully. Do you want to run it now?", "Run script");
            if (result)
            {
                powershell.RunScript(scriptFile, noExit: true);
            }
        }
        else if (Settings.CreateShellRun)
        {
            powershell.RunScript(scriptFile, noExit: true);
        }
        else if (Settings.CreateShell)
        {
            _dialogs.InfoMessage($"Script created successfully at:\r\n{scriptFile}", "Script created");
        }
    }

    private bool TryGetCustomSelectorValues(out List<(string key, string value)> output)
    {
        output = new();
        if (SelectedPreset == null
            || SelectedPreset.AssociatedPreset == null)
        {
            throw new InvalidOperationException("No preset selected");
        }

        foreach (var dialogDef in _dialogdefinitions)
        {
            if (SelectedPreset.AssociatedPreset.CommandLine.Contains($"{{{dialogDef.Name}}}")
                || SelectedPreset.AssociatedPreset.CommandLine.Contains($"{{{dialogDef.Name}."))
            {
                var dialog = CustomDialogFactory.Create(dialogDef);
                if (dialog.TryGetCommandLineValue(out var commandLine))
                {
                    if (commandLine.StartsWith('{')
                        && commandLine.EndsWith('}'))
                    {
                        // It's a JSON object with multiple values
                        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(commandLine) ?? new Dictionary<string, string>();
                        foreach (var setting in dict)
                        {
                            output.Add(($"{{{dialogDef.Name}.{setting.Key}}}", setting.Value));
                        }
                    }
                    else
                    {
                        output.Add(($"{{{dialogDef.Name}}}", commandLine));
                    }
                }
                else
                {
                    output.Clear();
                    return false;
                }
            }
        }

        return true;
    }

    private string CreateCommandLine(string file,
                                     string ffmpegPath,
                                     IEnumerable<(string key, string value)> values)
    {
        if (SelectedPreset == null
            || SelectedPreset.AssociatedPreset == null)
        {
            throw new InvalidOperationException("No preset selected");
        }

        var outputFile = Path.Combine(Settings.OutputDirectory, Path.ChangeExtension(Path.GetFileName(file), SelectedPreset.AssociatedPreset.Extension));

        StringBuilder buffer = new StringBuilder();
        buffer
            .Append(ffmpegPath)
            .Append(' ')
            .Append(SelectedPreset.AssociatedPreset.CommandLine.Trim())
            .Replace(Preset.InputPlaceHolder, $"\"{file}\"")
            .Replace(Preset.OutputPlaceHolder, $"\"{outputFile}\"");

        foreach (var (key, value) in values)
        {
            buffer.Replace(key, value);
        }

        return buffer.ToString();
    }

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
