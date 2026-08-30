using System.IO;
using System.Text;
using System.Text.Json;

using FFDrop.CustomDialogs;
using FFDrop.DomainServices;
using FFDrop.Model;
using FFDrop.Utils;
using FFDrop.Utils.Playlists;

namespace FFDrop;

internal sealed class AppLogic
{
    private readonly SettingsViewModel _settings;
    private readonly IDialogs _dialogs;
    private readonly Dialogdefinition[] _dialogdefinitions;

    public AppLogic(SettingsViewModel settings, IDialogs dialogs, Dialogdefinition[] dialogdefinitions)
    {
        _settings = settings;
        _dialogs = dialogs;
        _dialogdefinitions = dialogdefinitions;
    }

    public void FilesHaveBeenDropped(string[] files, PresetViewModel? selectedPreset)
    {

        Powershell powershell = new(updatePathVar: false);

        if (selectedPreset == null
            || selectedPreset.AssociatedPreset == null)
        {
            _dialogs.ErrorMessage("Please select a preset first", "No preset selected");
            return;
        }

        if (!ProgramFinder.TryFindProgramPath("ffmpeg.exe", out var ffmpegPath))
        {
            _dialogs.ErrorMessage("FFmpeg executable not found. Please ensure ffmpeg.exe is available in the system PATH.", "FFmpeg not found");
            return;
        }


        if (!ProgramFinder.TryFindProgramPath("ffprobe.exe", out var ffprobePath))
        {
            _dialogs.ErrorMessage("FFprobe executable not found. Please ensure ffprobe.exe is available in the system PATH.", "FFprobe not found");
            return;
        }

        var scriptFile = Path.Combine(_settings.OutputDirectory,
                                      Path.ChangeExtension(Path.GetFileName(_settings.OutputDirectory), ".ps1"));

        List<string> filesToConvert = new();
        List<string> filesToSkip = new();

        SortFilesToConvert(filesToConvert, filesToSkip, files);

        var title = Path.GetFileName(scriptFile);

        List<string> skipped = new();

        var builder = new PowershellBuilder()
            .WithUtf8Enabled()
            .WithWindowTitle(title)
            .WithClear()
            .WithVariable("ffmpeg", ffmpegPath)
            .WithVariable("ffprobe", ffprobePath)
            .WithTerminalProgress(0, 1);

        int current = 1;

        if (!TryGetCustomSelectorValues(selectedPreset, out List<(string key, string value)>? additionals))
        {
            return;
        }

        List<string> playlistItems = new();

        foreach (var file in filesToConvert)
        {
            string outputFileName = GetOutputFileName(file, selectedPreset);

            builder
                .WithCommand(CreateCommandLine(file, outputFileName, "& $ffmpeg", selectedPreset, additionals))
                .WithTerminalProgress(current, filesToConvert.Count);

            ++current;

            playlistItems.Add(outputFileName);
        }

        builder
            .WithInfoMessageBox("Conversion finished", title)
            .WithTerminalProgrssHidden()
            .WithFolderOpenedInExplorer(_settings.OutputDirectory)
            .WithExit();

        File.WriteAllText(scriptFile, builder.Build());

        if (skipped.Count > 0)
        {
            _dialogs.WarningMessage($"Skipped files:\r\n{string.Join("\r\n", skipped)}", "Skipped files");
        }

        if (_settings.CreatePls)
        {
            var plsFile = Path.Combine(_settings.OutputDirectory, Path.ChangeExtension(Path.GetFileName(_settings.OutputDirectory), ".pls"));
            Playlist.SavePls(playlistItems, plsFile);
        }

        if (_settings.CreateM3U)
        {
            var m3uFile = Path.Combine(_settings.OutputDirectory, Path.ChangeExtension(Path.GetFileName(_settings.OutputDirectory), ".m3u"));
            Playlist.SaveM3u(playlistItems, m3uFile);
        }

        if (_settings.CreateShellConfirmRun)
        {
            bool result = _dialogs.ConfirmMessage("Script created successfully. Do you want to run it now?", "Run script");
            if (result)
            {
                powershell.RunScript(scriptFile, noExit: true);
            }
        }
        else if (_settings.CreateShellRun)
        {
            powershell.RunScript(scriptFile, noExit: true);
        }
        else if (_settings.CreateShell)
        {
            _dialogs.InfoMessage($"Script created successfully at:\r\n{scriptFile}", "Script created");
        }

    }

    private static void SortFilesToConvert(List<string> filesToConvert, List<string> filesToSkip, in string[] files)
    {
        foreach (var file in files)
        {
            if (File.Exists(file))
            {
                if (FileRecognizer.IsDropConvertSupported(file))
                {
                    filesToConvert.Add(file);

                }
                else if (FileRecognizer.IsPlaylistFile(file)
                    && PlaylistUtils.TryLoadPlaylistItems(file, out var playlistitems))
                {
                    foreach (var item in playlistitems)
                    {
                        if (File.Exists(item)
                            && FileRecognizer.IsDropConvertSupported(item))
                        {
                            filesToConvert.Add(item);
                        }
                        else
                        {
                            filesToSkip.Add(Path.GetFileName(item));
                        }
                    }
                }
                else
                {
                    filesToSkip.Add(Path.GetFileName(file));
                }
            }
        }
    }

    private string GetOutputFileName(string file, PresetViewModel selectedPreset)
        => Path.Combine(_settings.OutputDirectory, Path.ChangeExtension(Path.GetFileName(file), selectedPreset.AssociatedPreset?.Extension));

    private bool TryGetCustomSelectorValues(PresetViewModel? selectedPreset, out List<(string key, string value)> output)
    {
        output = new();
        if (selectedPreset == null
            || selectedPreset.AssociatedPreset == null)
        {
            throw new InvalidOperationException("No preset selected");
        }

        foreach (var dialogDef in _dialogdefinitions)
        {
            if (selectedPreset.AssociatedPreset.CommandLine.Contains($"{{{dialogDef.Name}}}")
                || selectedPreset.AssociatedPreset.CommandLine.Contains($"{{{dialogDef.Name}."))
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

    private static string CreateCommandLine(string file,
                                            string outputFile,
                                            string ffmpegPath,
                                            PresetViewModel? selectedPreset,
                                            IEnumerable<(string key, string value)> values)
    {
        if (selectedPreset == null
            || selectedPreset.AssociatedPreset == null)
        {
            throw new InvalidOperationException("No preset selected");
        }

        StringBuilder buffer = new StringBuilder();
        buffer
            .Append(ffmpegPath)
            .Append(' ')
            .Append(selectedPreset.AssociatedPreset.CommandLine.Trim())
            .Replace(Preset.InputPlaceHolder, $"\"{file}\"")
            .Replace(Preset.OutputPlaceHolder, $"\"{outputFile}\"");

        foreach (var (key, value) in values)
        {
            buffer.Replace(key, value);
        }

        return buffer.ToString();
    }
}
