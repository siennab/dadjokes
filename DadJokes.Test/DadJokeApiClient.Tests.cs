using System.Net;
using DadJokes.Clients;
using DadJokes.Models;
using Moq;
using Moq.Protected;

namespace DadJokes.Test;
[TestClass]
public class DadJokeApiClientTests
{
    private DadJokeApiClient? _apiClient;
    private Mock<HttpMessageHandler>? _mockHttpMessageHandler;
    private HttpClient? _httpClient;

    [TestInitialize]
    public void Initialize()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        _apiClient = new DadJokeApiClient(_httpClient);
    }

    [TestMethod]
    public async Task GetAsync_ValidUrl_ReturnsDeserializedObject()
    {
        // Arrange
        var expectedResponse = new DadJoke("abc123", "Why couldn't the bicycle stand up by itself? Because it was two-tired.");

        var url = "https://icanhazdadjoke.com/";

        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
        responseMessage.Content = new StringContent(@"{""id"":""abc123"",""joke"":""Why couldn't the bicycle stand up by itself? Because it was two-tired.""}");

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // Act
        var result = await _apiClient!.GetAsync<DadJoke>(url);

        // Assert
        Assert.AreEqual(expectedResponse.Id, result.Id);
        Assert.AreEqual(expectedResponse.Joke, result.Joke);
    }

    [TestMethod]
    public async Task GetAsync_ValidUrlWithParams_ReturnsDeserializedObject()
    {
        // Arrange
        var expectedResponse = new DadJoke("abc123", "Why couldn't the bicycle stand up by itself? Because it was two-tired.");
        var url = "https://icanhazdadjoke.com/";
        var queryParams = new Dictionary<string, string> { { "term", "bicycle ride" } };
        var expectedUrl = $"{url}?term=bicycle%20ride";

        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
        responseMessage.Content = new StringContent(@"{""id"":""abc123"",""joke"":""Why couldn't the bicycle stand up by itself? Because it was two-tired.""}");

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // Act
        var result = await _apiClient!.GetAsync<DadJoke>(url, queryParams);

        // Assert
        Assert.AreEqual(expectedResponse.Id, result.Id);
        Assert.AreEqual(expectedResponse.Joke, result.Joke);
    }

    [TestMethod]
    public async Task GetAsync_InvalidUrl_ThrowsHttpRequestException()
    {
        // Arrange
        var url = "https://icanhazdadjoke.com/";

        var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // Act and Assert
        await Assert.ThrowsExceptionAsync<HttpRequestException>(() => _apiClient!.GetAsync<DadJoke>(url));
    }
}

