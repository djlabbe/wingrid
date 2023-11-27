using System.Text.Json.Serialization;

namespace Wingrid.Services.EventAPI.Models.Espn
{
    public class ReferenceWrapper
    {
        [JsonPropertyName("$ref")]
        public string? Ref { get; set; }
    }
}