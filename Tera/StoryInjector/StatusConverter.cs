using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace Tera.StoryInjector;

internal class StatusConverter : JsonConverter<Status>
{
    private readonly ILogger _logger;

    private Dictionary<string, Status> _teraStatuses;

    public StatusConverter(ILogger logger)
    {
        _logger = logger;

        _teraStatuses = typeof(TeraModStatuses).GetFields(BindingFlags.Static | BindingFlags.Public)
            .Where(f => f.FieldType == typeof(Status))
            .ToDictionary(f => f.Name, f => (Status)(f.GetValue(null) ?? 0));
    }

    public override Status Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? val = reader.GetString();

        if (_teraStatuses.TryGetValue(val ?? "", out Status status))
        {
            return status;
        }

        if (!Enum.TryParse<Status>(val, out Status result))
        {
            _logger.LogError("Cannot parse \"{Val}\" to enum {EnumName}", val, typeof(Deck).FullName);
        }
        return result;
    }

    public override void Write(Utf8JsonWriter writer, Status value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}