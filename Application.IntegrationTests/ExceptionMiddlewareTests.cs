using System.Net;
using System.Text.Json;

namespace Application.IntegrationTests
{
    public class ExceptionMiddlewareTests : IClassFixture<TestApplicationFactory>
    {
        private readonly HttpClient _client;

        public ExceptionMiddlewareTests(TestApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ThrowsValidationException_ReturnsBadRequest()
        {
            // Given
            var url = "/api/test/files/throw-validation";

            // When
            var response = await _client.GetAsync(url);

            // Then
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var json = JsonSerializer.Deserialize<JsonElement>(content);

            Assert.Equal("Validation error", json.GetProperty("error").GetString());
            Assert.Equal((int)HttpStatusCode.BadRequest, json.GetProperty("statusCode").GetInt32());
        }

        [Fact]
        public async Task ThrowsException_ReturnsInternalServerError()
        {
            // Given
            var url = "/api/test/files/throw-exception";

            // When
            var response = await _client.GetAsync(url);

            // Then
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var json = JsonSerializer.Deserialize<JsonElement>(content);

            Assert.Equal("Internal server error", json.GetProperty("error").GetString());
            Assert.Equal((int)HttpStatusCode.InternalServerError, json.GetProperty("statusCode").GetInt32());
        }

        [Fact]
        public async Task NormalRequest_ReturnsOkAsync()
        {
            // Given
            var url = "/api/files/health";
            // When
            var response = await _client.GetAsync(url);
            // Then
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}