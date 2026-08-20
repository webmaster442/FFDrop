using System.Text.Json;

using FFDrop.Utils.FFProbe;

namespace FFDrop.Utils;

internal class MediaInfo
{
    public static async ValueTask<FFProbeResponse?> GetMediaInfo(string filePath)
    {
        string json = await ProcessEx.GetProcessOutput("ffprobe.exe", $"-v quiet -print_format json -show_format -show_streams \"{filePath}\"");
        return JsonSerializer.Deserialize<FFProbeResponse>(json, FFProbeJsonSerializerContext.Default.FFProbeResponse);
    }
}
