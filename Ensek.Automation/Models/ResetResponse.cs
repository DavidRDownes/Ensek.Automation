using System.Text.Json.Serialization;


namespace Ensek.Automation.Models
{
    public class ResetResponse
    {
        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }
}
