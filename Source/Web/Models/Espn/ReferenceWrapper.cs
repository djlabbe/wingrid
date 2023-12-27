using System.Text.Json.Serialization;

namespace Wingrid.Models.Espn
{
    public class ReferenceWrapper
    {
        [JsonPropertyName("$ref")]
        public string? Ref { get; set; }
    }
}