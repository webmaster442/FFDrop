using System.Text;

namespace FFDrop.DomainServices;

internal sealed class PowershellBuilder
{
    private readonly StringBuilder _scriptBuilder;

    public PowershellBuilder()
    {
        _scriptBuilder = new StringBuilder(4096);
        _scriptBuilder.Append("""
            function Show-InfoMessageBox {
                param (
                    [Parameter(Mandatory=$true)]
                    [string]$Text,

                    [Parameter(Mandatory=$true)]
                    [string]$Title
                )

                Add-Type -AssemblyName System.Windows.Forms
                [System.Windows.Forms.MessageBox]::Show($Text, $Title, 'OK', 'Information')
            }

            """);
    }

    public string Build()
    {
        return _scriptBuilder.ToString();
    }

    public PowershellBuilder WithVariable(string name, string value)
    {
        _scriptBuilder.AppendLine($"${name} = \"{value}\"");
        return this;
    }

    public PowershellBuilder WithClear()
    {
        _scriptBuilder.AppendLine("Clear-Host");
        return this;
    }

    public PowershellBuilder WithCommandIfFileNotExists(string filePath, string command)
    {
        _scriptBuilder.AppendLine($"if (-not (Test-Path '{filePath}')) {{");
        _scriptBuilder.AppendLine($"    {command}");
        _scriptBuilder.AppendLine("}");
        return this;
    }

    public PowershellBuilder WithUtf8Enabled()
    {
        _scriptBuilder.AppendLine("[Console]::OutputEncoding = [System.Text.Encoding]::UTF8");
        return this;
    }

    public PowershellBuilder WithWindowTitle(string title)
    {
        _scriptBuilder.AppendLine($"$Host.UI.RawUI.WindowTitle = \"{title}\"");
        return this;
    }

    public PowershellBuilder WithCommand(string command)
    {
        _scriptBuilder.AppendLine(command);
        return this;
    }

    public PowershellBuilder WithInfoMessageBox(string message, string title)
    {
        _scriptBuilder.AppendLine($"Show-InfoMessageBox -Text \"{message}\" -Title \"{title}\"");
        _scriptBuilder.AppendLine($"Write-Host \"{title}: {message}\"");
        return this;
    }

    public PowershellBuilder WithTerminalProgress(int count, int total)
    {
        int percent = (int)Math.Round((double)count / total * 100.0);
        _scriptBuilder.AppendLine($"Write-Host -NoNewline (\"`e]9;4;1;{percent}`a\")");
        return this;
    }

    public PowershellBuilder WithTerminalProgrssHidden()
    {
        _scriptBuilder.AppendLine("Write-Host -NoNewline (\"`e]9;4;0;0`a\")");
        return this;
    }

    public PowershellBuilder WithFolderOpenedInExplorer(string folder)
    {
        _scriptBuilder.AppendLine($"Invoke-Item -Path \"{folder}\"");
        return this;
    }

    public PowershellBuilder WithExit()
    {
        _scriptBuilder.AppendLine($"exit");
        return this;
    }
}