using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tera.StoryInjector;

internal class NodeTypeConverter : JsonConverter<NodeType>
{
    public override NodeType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? val = reader.GetString();
        return val switch
        {
            "combat" => NodeType.combat,
            "event" => NodeType.@event,
            "voidShout" => NodeType.voidShout,
            _ => throw new InvalidOperationException(),
        };
    }

    public override void Write(Utf8JsonWriter writer, NodeType value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}