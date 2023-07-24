using Ensek.Automation.Helpers;
using Ensek.Automation.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TechTalk.SpecFlow;

namespace Ensek.Automation.StepDefinitions
{
    [Binding]
    public class ResetSteps
    {
        private HttpClient _httpClient;
        private HttpResponseMessage _response;
        private LoginResponse _loginResponse;

        [BeforeScenario]
        public void SetupHttpClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://qacandidatetest.ensek.io/");
        }

        [Given(@"I send valid credentials to the Login endpoint")]
        public async Task GivenISetLoginTheRequestBody(Table credentialsTable)
        {
            const string endpoint = "ENSEK/login";

            // Passwords should not be stored in plain text and a secrets manager implementation should be added
            var credentials = credentialsTable.Rows[0];

            var authHelper = new AuthHelper(_httpClient);
            _loginResponse = await authHelper.GetToken(
                endpoint, 
                credentials["Username"], 
                credentials["Password"]);
        }

        [When(@"I make a POST request to the reset endpoint")]
        public async Task GivenIMakeAPostRequestToReset()
        {
            const string endpoint = "ENSEK/reset";

            var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _loginResponse.AccessToken);

            _response = await _httpClient.SendAsync(request);
        }

        [Then(@"I get a (\d+) response")]
        public void ThenIGetAResponse(int expectedStatusCode)
        {
            var actualStatusCode = _response.StatusCode;
            Assert.That(actualStatusCode, Is.EqualTo((HttpStatusCode)expectedStatusCode));

        }

        [Then(@"I get a response message")]
        public async Task ThenIGetAResponseMessage()
        {
           var response = await _response.Content.ReadFromJsonAsync<ResetResponse>(); 
           Assert.NotNull(response);
           Assert.That(response.Message, Is.EqualTo("Success"));
        }

        [AfterScenario]
        public void CleanupHttpClient()
        {
            _httpClient.Dispose();
        }
    }
}
