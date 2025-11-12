using System.Diagnostics;
using System.Text;

namespace FFDrop.Utils;

public static class ProcessEx
{
    public static async ValueTask<string> GetProcessOutput(string processfileName, string arguments)
    {
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = processfileName,
                Arguments = arguments,
                RedirectStandardOutput = true,
                StandardOutputEncoding  = Encoding.UTF8,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        var outputBuilder = new StringBuilder();
        process.OutputDataReceived += (sender, e) =>
        {
            if (e.Data != null)
            {
                outputBuilder.AppendLine(e.Data);
            }
        };

        await Task.Run(() =>
        {
            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();
        });

        return outputBuilder.ToString();
    }
}
