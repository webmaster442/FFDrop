using System.Text.Json.Serialization;

namespace FFDrop.Model;

public class Dialogdefinition
{
    [JsonPropertyName("title")]
    public required string Title { get; set; }

    /// <summary>
    /// dialog description
    /// </summary>
    [JsonPropertyName("description")]
    public required string Description { get; set; }

    /// <summary>
    /// array of selectable values for selector dialog
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("values")]
    public string[]? Values { get; set; }

    [JsonPropertyName("defaultValue")]
    public string? DefaultValue { get; set; }

    [JsonPropertyName("dialogtype")]
    public Dialogtype Dialogtype { get; set; }

    /// <summary>
    /// dialog definition name
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }
}
