using System.Text.Json.Serialization;

namespace FFDrop.Utils.FFProbe;

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString)]
[JsonSerializable(typeof(FFProbeResponse), GenerationMode =JsonSourceGenerationMode.Serialization)]
internal sealed partial class FFProbeJsonSerializerContext : JsonSerializerContext
{
}
