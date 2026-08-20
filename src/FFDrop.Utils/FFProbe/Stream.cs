using System.Text;
using System.Text.Json.Serialization;

namespace FFDrop.Utils.FFProbe;

public class Stream
{
    [JsonPropertyName("index")]
    public int Index { get; set; }
    [JsonPropertyName("codec_name")]
    public required string CodecName { get; set; }
    [JsonPropertyName("codec_long_name")]
    public required string CodecLongName { get; set; }
    [JsonPropertyName("bit_rate")]
    public long BitsPerSecond { get; set; }

    [JsonPropertyName("width")]
    public int? Width { get; set; }
    [JsonPropertyName("height")]
    public int? Height { get; set; }
    [JsonPropertyName("coded_width")]
    public int? CodedWidth { get; set; }
    [JsonPropertyName("coded_height")]
    public int? CodedHeight { get; set; }
    [JsonPropertyName("sample_aspect_ratio")]
    public string? SampleAspectRatio { get; set; }
    [JsonPropertyName("display_aspect_ratio")]
    public string? DisplayAspectRatio { get; set; }
    [JsonPropertyName("pix_fmt")]
    public string? PixFmt { get; set; }
    [JsonPropertyName("avg_frame_rate")]
    public string? AvgFrameRate { get; set; }

    [JsonPropertyName("sample_fmt")]
    public string? SampleFmt { get; set; }
    [JsonPropertyName("sample_rate")]
    public string? SampleRate { get; set; }
    [JsonPropertyName("channels")]
    public int? Channels { get; set; }
    [JsonPropertyName("channel_layout")]
    public string? ChannelLayout { get; set; }

    [JsonIgnore]
    public bool IsVideoStream 
        => Width.HasValue && Height.HasValue;

    [JsonIgnore]
    public bool IsAudioStream
        => SampleFmt != null && SampleRate != null && Channels.HasValue;

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Index: {Index}");
        sb.AppendLine($"Codec Name: {CodecName}");
        sb.AppendLine($"Codec Long Name: {CodecLongName}");
        sb.AppendLine($"Bit Rate: {BitRate.FromBps(BitsPerSecond).ToString()}");
        if (IsVideoStream)
        {
            sb.AppendLine($"Display size: {Width}x{Height}");
            sb.AppendLine($"Display Aspect ratio: {DisplayAspectRatio}");
            sb.AppendLine($"Coded size: {CodedWidth}x{CodedHeight}");
            sb.AppendLine($"Coded Aspect ratio: {SampleAspectRatio}");
            sb.AppendLine($"Pixel Format: {PixFmt}");
            sb.AppendLine($"Average Frame Rate: {AvgFrameRate}");
        }
        
        if (IsAudioStream)
        {
            sb.AppendLine($"Sample Format: {SampleFmt}");
            sb.AppendLine($"Sample Rate: {SampleRate}");
            sb.AppendLine($"Channels: {Channels}");
            sb.AppendLine($"Channel Layout: {ChannelLayout}");
        }

        return sb.ToString();
    }
}
