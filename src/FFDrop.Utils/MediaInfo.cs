using System.Text.Json;

using FFDrop.Utils.FFProbe;

namespace FFDrop.Utils;

public static class MediaInfo
{
    public static async ValueTask<MediaInfoModel?> GetMediaInfo(string ffprobePath, string filePath)
    {
        string json = await ProcessEx.GetProcessOutput(ffprobePath, $"-v quiet -print_format json -show_format -show_streams \"{filePath}\"");
        FFProbeResponse? model = JsonSerializer.Deserialize(json, FFProbeJsonSerializerContext.Default.FFProbeResponse);
        return MediaInfoModel.FromFFProbeResponse(model);
    }
}
