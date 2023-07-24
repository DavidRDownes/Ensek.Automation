using System.Text.Json.Serialization;

namespace Ensek.Automation.Models
{
    public class OrdersResponse
    {
        [JsonPropertyName("fuel")]
        public string? Fuel { get; set; }

        [JsonPropertyName("id")]
        public Guid? Id { get; set; }

        [JsonPropertyName("quantity")]
        public int? Quantity { get; set; }

        [JsonPropertyName("time")]
        public string? Time { get; set; }
    }
}
