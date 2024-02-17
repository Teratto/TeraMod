using System.Text.Json.Serialization;

namespace Tera.StoryInjector;

public class InjectedItemContainer
{
    [JsonPropertyName("items")]
    public InjectedItem[] Items;
}