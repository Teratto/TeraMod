using System.Text.Json.Serialization;

namespace Tera.StoryInjector;

public class InjectedItem
{
    [JsonPropertyName("event_name")]
    public string EventName;

    [JsonPropertyName("indices")]
    public int[] Indices = Array.Empty<int>();

    [JsonPropertyName("who")]
    public string Who;

    [JsonPropertyName("what")]
    public string What;

    [JsonPropertyName("loop_tag")]
    public string LoopTag;

    /// <summary>
    /// If this item is for a say switch, and is intending to add a new arm to the switch.
    /// When true, the last index in Indices will be ignored, and this item will instead be appended to the end of the
    /// SaySwitch's lines. 
    /// </summary>
    [JsonPropertyName("is_inserted")]
    public bool IsInserted;

}