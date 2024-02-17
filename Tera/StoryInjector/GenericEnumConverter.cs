using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace Tera.StoryInjector;

internal class GenericEnumConverter<T> : JsonConverter<T> where T : struct
{
    private readonly ILogger _logger;

    public GenericEnumConverter(ILogger logger)
    {
        _logger = logger;
    }

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? val = reader.GetString();
        if (!Enum.TryParse<T>(val, out T result))
        {
            _logger.LogError("Cannot parse \"{Val}\" to enum {EnumName}", val, typeof(T).FullName);
        }
        return result;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}