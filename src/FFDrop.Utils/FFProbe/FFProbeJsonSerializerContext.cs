using System.Text.Json.Serialization;

namespace FFDrop.Utils.FFProbe;

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString)]
[JsonSerializable(typeof(FFProbeResponse))]
internal sealed partial class FFProbeJsonSerializerContext : JsonSerializerContext
{
}
