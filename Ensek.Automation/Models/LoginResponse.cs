using System.Text.Json.Serialization;


namespace Ensek.Automation.Models
{
    public class LoginResponse
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("message")]
        public string? SuccessMessage { get; set; }
    }
}
