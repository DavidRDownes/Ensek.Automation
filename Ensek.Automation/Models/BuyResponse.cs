using System.Text.Json.Serialization;

namespace Ensek.Automation.Models
{
    public class BuyResponse
    {
        [JsonPropertyName("message")]
        public string? Message { get; set; }

    }
}
