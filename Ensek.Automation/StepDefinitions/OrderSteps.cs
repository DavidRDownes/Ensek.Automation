using Ensek.Automation.Models;
using System.Net;
using TechTalk.SpecFlow;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Ensek.Automation.StepDefinitions
{
    [Binding]
    public class OrderSteps
    {
        private HttpClient? _httpClient;
        private HttpResponseMessage? _response;
        private Guid _orderId;

        [BeforeScenario]
        public void SetupHttpClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://qacandidatetest.ensek.io/");
        }

        [Given(@"I buy (.*) units of energy type (.*)")]
        public async Task GivenIBuyEnergy(int orderQuantity, int energyType)
        {
            //Change values to variables 
            var orderEndpoint = $"/ENSEK/buy/{energyType}/{orderQuantity}";

            var request = new HttpRequestMessage(HttpMethod.Put, orderEndpoint);

            _response = await _httpClient.SendAsync(request);

            if (_response.StatusCode == HttpStatusCode.OK)
            {
                //The poor setup of API responses led to complex step definitions for obtaining the required information in tests.
                var responseContent = await _response.Content.ReadAsStringAsync();
               
                var guidPattern = @"[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}";

                Assert.That(Regex.IsMatch(responseContent, guidPattern), Is.True, "No GUID generated");

                // Use Regex.Match to extract the GUID
                var match = Regex.Match(responseContent, guidPattern);
                if (match.Success)
                {
                    var extractedGuid = match.Value;
                    _orderId = Guid.Parse(extractedGuid);
                }
            }
        }

        [When(@"I call the orders endpoint")]
        public async Task WhenICallOrdersEndpoint()
        {
            //Change values to variables 
            var orderEndpoint = "/ENSEK/orders";

            var request = new HttpRequestMessage(HttpMethod.Get, orderEndpoint);

            _response = await _httpClient.SendAsync(request);
        }

        [Then(@"the order is present")]
        public async Task ThenOrderIsPresent()
        {
            var response = await _response.Content.ReadAsStringAsync();

            // The api seems to respond with inconsistent property casing
            var orders = JsonSerializer.Deserialize<List<OrdersResponse>>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (orders == null)
            {
                Assert.Fail("No orders were deserialised");
            }

            Assert.That(orders.Any(order => order.Id == _orderId), Is.True, $"Order with ID {_orderId} not found.");
        }

        [Then(@"I verify there are (.*) previous orders")]
        public async Task ThenIverifyNumberOfPreviousOrders(int noOfOrders)
        {
            // Get today's date and time in UTC
            var todayUtc = DateTime.UtcNow.Date;

            var response = await _response.Content.ReadAsStringAsync();

            var orders = JsonSerializer.Deserialize<List<OrdersResponse>>(response);

            if (orders == null)
            {
                Assert.Fail("No orders were deserialised");
            }

            // Use LINQ to filter the orders and count how many have an entry time before todayUtc
            int ordersBeforeToday = orders.Count(order =>
            {
                DateTime entryTime;
                return DateTime.TryParseExact(order.Time, new string[] { "ddd, d MMM yyyy HH:mm:ss 'GMT'", "ddd, dd MMM yyyy HH:mm:ss 'GMT'" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal, out entryTime)
                    && entryTime < todayUtc;
            });

            // Assert that the count of orders before today matches the expected count
            Assert.That(noOfOrders, Is.EqualTo(ordersBeforeToday));
        }

        [Then(@"My purchase was successful")]
        public async Task ThenPurchaseSuccessful()
        {
            // API lacks structured responses which is why we use regex here
            var expectedFailureText = @"There is no.*";
            var expectedPassText = @"You have purchased.*";

            var response = await _response.Content.ReadAsStringAsync();

            if (Regex.IsMatch(response, expectedFailureText))
            {
                Assert.Fail($"Test failed due to lack of available quantity");
            }
            else if (Regex.IsMatch(response, expectedPassText))
            {
                Console.WriteLine($"Test passed");
            }
            else
            {
                Assert.Fail("Unexpected response");
            }
        }

        [AfterScenario]
        public void CleanupHttpClient()
        {
            _httpClient?.Dispose();
        }
    }
}
