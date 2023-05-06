using DadJokes.AppSettings;
using DadJokes.Models;
using Microsoft.Extensions.Options;
using Moq;

namespace DadJokes.Test
{
    [TestClass]
    public class DadJokeSearchResultsTests
    {
        [TestMethod]
        public void TestGroupedResults()
        {


            // Arrange
            var results = new List<DadJoke>
            {
                new DadJoke("abc", "Why did the chicken cross the road? To get to the other side."),
                new DadJoke("abc2", "What do you call a fake noodle? An impasta." ),
                new DadJoke("abc4", "Why don't oysters give to charity? Because they're shellfish." ),
                new DadJoke("abc5", "Why couldn't the bicycle stand up by itself? Because it was two-tired! See, the bicycle was \\\"too tired\\\" as in it had two tires, but also \\\"too tired\\\" as in exhausted, which is why it couldn't stand up by itself. You know, I once tried to invent a bicycle that could stand up by itself, but it was a complete failure." ),
            };

            var searchResults = new DadJokeSearchResults()
            {
                Results = results,
            };

            // Act
            var groupedResults = searchResults.GroupedResults;

            // Assert
            Assert.AreEqual(3, groupedResults.Count());
            Assert.AreEqual(2, groupedResults.First(g => g.Key == "Short").Count());
            Assert.AreEqual(1, groupedResults.First(g => g.Key == "Medium").Count());
            Assert.AreEqual(1, groupedResults.First(g => g.Key == "Long").Count());
        }
    }
}

