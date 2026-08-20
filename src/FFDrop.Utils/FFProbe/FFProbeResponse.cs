using System.Text.Json.Serialization;

namespace FFDrop.Utils.FFProbe;

public class FFProbeResponse
{
    [JsonPropertyName("streams")]
    public required Stream[] Streams { get; set; }
    [JsonPropertyName("format")]
    public required Format Format { get; set; }
}
