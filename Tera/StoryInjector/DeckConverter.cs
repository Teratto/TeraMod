using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace Tera.StoryInjector;

internal class DeckConverter : JsonConverter<Deck>
{
    private readonly ILogger _logger;

    public DeckConverter(ILogger logger)
    {
        _logger = logger;
    }

    public override Deck Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? val = reader.GetString();

        if (val == "tera" && ModManifest.TeraDeck!.Id.HasValue)
        {
            return (Deck)ModManifest.TeraDeck!.Id.Value;
        }

        if (!Enum.TryParse<Deck>(val, out Deck result))
        {
            _logger.LogError("Cannot parse \"{Val}\" to enum {EnumName}", val, typeof(Deck).FullName);
        }
        return result;
    }

    public override void Write(Utf8JsonWriter writer, Deck value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
