using FFDrop.Utils.FFProbe;

namespace FFDrop.Utils;

public sealed class MediaInfoModel
{
    public required string Summary { get; init; }
    public required MediaInfoModel[] Details { get; init; }

    public static MediaInfoModel? FromFFProbeResponse(FFProbeResponse? response)
    {
        return response == null
            ? null
            : new MediaInfoModel
        {
            Summary = response.Format.ToString(),
            Details = response.Streams.Select(s => new MediaInfoModel
            {
                Summary = s.ToString(),
                Details = Array.Empty<MediaInfoModel>()
            }).ToArray()
        };
    }
}
