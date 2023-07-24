using Ensek.Automation.Models;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Ensek.Automation.Helpers
{
    public class AuthHelper
    {
        private readonly HttpClient _httpClient;

        public AuthHelper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResponse> GetToken(
            string endpoint,
            string username, 
            string password)
        {
            var content = new StringContent(JsonSerializer.Serialize(new LoginRequest
            {
                Username = username,
                Password = password
            }), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);
            var token = await response.Content.ReadFromJsonAsync<LoginResponse>();
            
            if (token == null) 
            {
                throw new NullReferenceException("No token found");
            }

            return token;
        }
    }
}
