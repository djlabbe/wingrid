using System.Text.Json.Serialization;

namespace Wingrid.Collector.Models.Espn
{
    public class ReferenceWrapper
    {
        [JsonPropertyName("$ref")]
        public string? Ref { get; set; }
    }
}