using System.Net;
using System.Net.Http.Json;
using TravelBooker.Testing.Factories;

namespace TravelBooker.Testing.User.Integration_Tests
{
    public class UserRegistrationTests : IClassFixture<IntegrationTestAppFactory>
    {
        private readonly HttpClient _httpClient;

        private string GenerateUniqueEmail() => $"{Guid.NewGuid()}@mail.com";

        public UserRegistrationTests(IntegrationTestAppFactory factory)
        {
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task Register_ValidPayload_Returns200()
        {
            var payload = new { email = GenerateUniqueEmail(), password = "ValidPassword1!" };
            var response = await _httpClient.PostAsJsonAsync("api/register", payload);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task Register_InvalidEmail_Returns400()
        {
            var payload = new { email = "@badEmail@mail.com", password = "ValidPassword1!" };
            var response = await _httpClient.PostAsJsonAsync("api/register", payload);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task Register_WeakPassword_Returns400()
        {
            var payload = new { email = GenerateUniqueEmail(), password = "WeakPassword!" };
            var response = await _httpClient.PostAsJsonAsync("api/register", payload);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task Register_ExistingEmail_Returns409()
        {
            var payload = new { email = "blovric966@gmail.com", password = "ValidPassword1!" };
            var response = await _httpClient.PostAsJsonAsync("api/register", payload);
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

    }
}
