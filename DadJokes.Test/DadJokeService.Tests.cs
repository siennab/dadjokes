using DadJokes.AppSettings;
using DadJokes.Interface;
using DadJokes.Models;
using DadJokes.Services;
using Microsoft.Extensions.Options;
using Moq;

namespace DadJokes.Test;

[TestClass]
public class DadJokeServiceTests
{
    private Mock<IDadJokeApiClient> _mockApiClient = new Mock<IDadJokeApiClient>();
    private Mock<IOptions<DadJokeAppSettings>> _mockAppSettings = new Mock<IOptions<DadJokeAppSettings>>();
    private DadJokeService? _dadjokeService;
    private readonly string url = "https://test.com/";

    [TestInitialize]
    public void Initialize()
    {
        // Arrange
        _mockAppSettings.Setup(x => x.Value).Returns(
            new DadJokeAppSettings
            {
                RandomJokeEndpointUrl = url,
                SearchEndpointUrl = url,
                DefaultSearchLimit = "30"
            }
        );
        _dadjokeService = new DadJokeService(_mockApiClient.Object, _mockAppSettings.Object);
    }

    [TestMethod]
    public async Task GetRandomJoke_ShouldReturnDadJoke()
    {
        // Arrange
        var expectedJoke = new DadJoke("abc123", "Why don't scientists trust atoms? Because they make up everything.");
        _mockApiClient
            .Setup(x => x.GetAsync<DadJoke>(url))
            .ReturnsAsync(expectedJoke);

        // Act
        var result = await _dadjokeService!.GetRandomJoke();

        // Assert
        Assert.AreEqual(expectedJoke.Joke, result.Joke);
    }

    [TestMethod]
    public async Task SearchJokes_ShouldReturnDadJokeSearchResults()
    {
        // Arrange
        var expectedSearchResults = new DadJokeSearchResults
        {
            CurrentPage = 1,
            NextPage = 1,
            PreviousPage = 1,
            Limit = 30,
            Results = new List<DadJoke>
            {
                new DadJoke("abc123",  "Why did the coffee file a police report? It got mugged."),
                new DadJoke ("124bce", "Why are ghosts such bad liars? Because they are easy to see through." )
            }
        };
        _mockApiClient
            .Setup(x => x.GetAsync<DadJokeSearchResults>(url, new Dictionary<string, string> {
                    { "term", "coffee" },
                    { "limit", "30" }
                }))
               .ReturnsAsync(expectedSearchResults);

        // Act
        var result = await _dadjokeService!.SearchJokes("coffee");

        // Assert
        Assert.AreEqual(expectedSearchResults.Results.Count(), result.Results.Count());
        for (int i = 0; i < expectedSearchResults.Results.Count(); i++)
        {
            Assert.AreEqual(expectedSearchResults.Results.ElementAt(i).Joke, result.Results.ElementAt(i).Joke);
        }
    }
}
