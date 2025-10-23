using System.Text;

namespace FFDrop.DomainServices;

internal class PowershellBuilder
{
    private readonly StringBuilder _scriptBuilder;

    public PowershellBuilder()
    {
        _scriptBuilder = new StringBuilder(4096);
    }

    public string Build()
    {
        return _scriptBuilder.ToString();
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

    public PowershellBuilder WithMessage(string msg)
    {
        _scriptBuilder.AppendLine($"Write-Host \"{msg}\"");
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
}