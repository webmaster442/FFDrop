using System.Diagnostics;
using System.IO;

namespace FFDrop.DomainServices;

internal sealed class Powershell
{
    private readonly string _powershellExecutable;

    public Powershell(bool updatePathVar)
    {
        _powershellExecutable = ProgramFinder.TryFindProgramPath("pwsh.exe", out var pwshPath) 
            ? pwshPath
            : "powershell.exe";

        if (updatePathVar)
        {
            AddDirectoryToPath(AppContext.BaseDirectory);
        }
    }

    public void RunCommands(IEnumerable<string> commands, bool shellExecute = false)
    {
        string cmd = string.Join(";", commands);

        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = _powershellExecutable,
                Arguments = $"-NoExit -Command \"& {{{cmd}}}",
                UseShellExecute = shellExecute,
            }
        };
        process.Start();
    }

    public void RunScript(string scriptFile, bool noExit = false)
    {
        if (!File.Exists(scriptFile))
        {
            throw new FileNotFoundException("Script file not found", scriptFile);
        }

        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = _powershellExecutable,
                Arguments = noExit
                            ? $"-executionpolicy bypass -noexit -File \"{scriptFile}\""
                            : $"-executionpolicy bypass -File \"{scriptFile}\"",
                UseShellExecute = false,
            }
        };
        process.Start();
    }

    private static void AddDirectoryToPath(string baseDirectory)
    {
        string pathVar = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
        if (!pathVar.Contains(baseDirectory))
        {
            pathVar = $"{baseDirectory};{pathVar}";
            Environment.SetEnvironmentVariable("PATH", pathVar, EnvironmentVariableTarget.Process);
        }
    }
}