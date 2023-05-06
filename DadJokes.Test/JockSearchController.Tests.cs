using DadJokes.Controllers;
using DadJokes.Interface;
using DadJokes.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DadJokes.Test
{
    [TestClass]
    public class JokeSearchControllerTests
    {
        private Mock<IDadJokeService> _mockService = new Mock<IDadJokeService>();
        private JokeSearchController? _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            _controller = new JokeSearchController(_mockService.Object);
        }

        [TestMethod]
        public async Task Index_ReturnsViewWithViewModel_WhenTermIsNotNull()
        {
            // Arrange
            var term = "test";
            var results = new DadJokeSearchResults
            {
                Results = new List<DadJoke>
            {
                new DadJoke("abc123", "This is a test joke")
            }
            };
            _mockService.Setup(x => x.SearchJokes(term)).ReturnsAsync(results);

            // Act
            var result = await _controller!.Index(term) as ViewResult;
            var model = result!.Model as DadJokeViewModel<DadJokeSearchResults>;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
            Assert.AreEqual(model.Result, results);
            Assert.AreEqual(model!.Result!.Results.ElementAt(0).Joke, "This is a [test] joke");
        }

        [TestMethod]
        public async Task Index_SetsErrorFlagAndErrorMessage_WhenServiceThrowsException()
        {
            // Arrange
            var term = "test";
            var exception = new Exception("Test exception");
            _mockService.Setup(x => x.SearchJokes(term)).ThrowsAsync(exception);

            // Act
            var result = await _controller!.Index(term) as ViewResult;
            var model = result!.Model as DadJokeViewModel<DadJokeSearchResults>;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
            Assert.IsTrue(model.HasError);
            Assert.AreEqual(model.ErrorMessage, exception.Message);
        }
    }

}

