using System.Text.Json.Serialization;

namespace FFDrop.Model;

internal sealed class Preset
{
    public const string InputPlaceHolder = "{input}";
    public const string OutputPlaceHolder = "{output}";

    [JsonPropertyName("name")]
    public required string Name { get; init; }
    [JsonPropertyName("path")]
    public required string Path { get; init; }
    [JsonPropertyName("commandLine")]
    public required string CommandLine { get; init; }
    [JsonPropertyName("extension")]
    public required string Extension { get; init; }
    [JsonPropertyName("description")]
    public required string Description { get; init; }
}
