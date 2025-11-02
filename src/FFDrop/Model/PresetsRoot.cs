using System.Text.Json.Serialization;

namespace FFDrop.Model;

internal sealed class PresetsRoot
{
    /// <summary>
    /// An array of preset items
    /// </summary>
    [JsonPropertyName("presets")]
    public required Preset[] Presets { get; init; }

    /// <summary>
    /// An array of dialog definitions
    /// </summary>
    [JsonPropertyName("dialogdefinitions")]
    public required Dialogdefinition[] Dialogdefinitions { get; set; }
}
