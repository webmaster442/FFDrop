using System.Text;
using System.Text.Json.Serialization;

namespace FFDrop.Utils.FFProbe;

public sealed class Format
{
    [JsonPropertyName("filename")]
    public required string Filename { get; set; }

    [JsonPropertyName("nb_streams")]
    public int StreamCount { get; set; }

    [JsonPropertyName("format_name")]
    public required string FormatName { get; set; }

    [JsonPropertyName("format_long_name")]
    public required string FormatLongName { get; set; }

    [JsonPropertyName("start_time")]
    public double StartTime { get; set; }

    [JsonPropertyName("duration")]
    public double Duration { get; set; }

    [JsonPropertyName("size")]
    public long Size { get; set; }

    [JsonPropertyName("bit_rate")]
    public long BitRate { get; set; }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Filename: {Filename}");
        sb.AppendLine($"Stream Count: {StreamCount}");
        sb.AppendLine($"Format Name: {FormatName}");
        sb.AppendLine($"Format Long Name: {FormatLongName}");
        sb.AppendLine($"Start Time: {TimeSpan.FromSeconds(StartTime)}");
        sb.AppendLine($"Duration: {TimeSpan.FromSeconds(Duration)}");
        sb.AppendLine($"Size: {FileSize.FromBytes(Size)}");
        sb.AppendLine($"Bit Rate: {BitRate}");
        return sb.ToString();
    }
}
