using System.Text.Json.Serialization;

namespace Domain.Espn
{
    public class ReferenceWrapper
    {
        [JsonPropertyName("$ref")]
        public string? Ref { get; set; }
    }
}