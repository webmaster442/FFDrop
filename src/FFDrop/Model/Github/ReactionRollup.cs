namespace FFDrop.Model.Github;

using System;
using System.Text.Json.Serialization;

public sealed class ReactionRollup
{
    [JsonPropertyName("+1")]
    public required long The1 { get; set; }

    [JsonPropertyName("-1")]
    public required long ReactionRollup1 { get; set; }

    [JsonPropertyName("confused")]
    public required long Confused { get; set; }

    [JsonPropertyName("eyes")]
    public required long Eyes { get; set; }

    [JsonPropertyName("heart")]
    public required long Heart { get; set; }

    [JsonPropertyName("hooray")]
    public required long Hooray { get; set; }

    [JsonPropertyName("laugh")]
    public required long Laugh { get; set; }

    [JsonPropertyName("rocket")]
    public required long Rocket { get; set; }

    [JsonPropertyName("total_count")]
    public required long TotalCount { get; set; }

    [JsonPropertyName("url")]
    public required Uri Url { get; set; }
}
