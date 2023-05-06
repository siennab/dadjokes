using DadJokes.Controllers;
using DadJokes.Interface;
using DadJokes.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DadJokes.Test
{
    [TestClass]
    public class RandomJokeControllerTests
    {
        private Mock<IDadJokeService> _mockService = new Mock<IDadJokeService>();
        private RandomJokeController? _controller;

        [TestInitialize]
        public void Setup()
        {
            _controller = new RandomJokeController(_mockService.Object);
        }

        [TestMethod]
        public async Task Index_ReturnsViewWithDadJokeViewModel_WhenServiceReturnsRandomJoke()
        {
            // Arrange
            var dadJoke = new DadJoke("123", "Why did the chicken cross the road?");
            _mockService.Setup(x => x.GetRandomJoke()).ReturnsAsync(dadJoke);

            // Act
            var result = await _controller!.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var viewModel = result.Model as DadJokeViewModel<DadJoke>;
            Assert.IsNotNull(viewModel);
            Assert.IsFalse(viewModel.HasError);
            Assert.AreEqual(dadJoke, viewModel.Result);
        }

        [TestMethod]
        public async Task Index_ReturnsViewWithDadJokeViewModel_WhenServiceThrowsException()
        {
            // Arrange
            var exceptionMessage = "An error occurred";
            _mockService.Setup(x => x.GetRandomJoke()).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller!.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var viewModel = result.Model as DadJokeViewModel<DadJoke>;
            Assert.IsNotNull(viewModel);
            Assert.IsTrue(viewModel.HasError);
            Assert.AreEqual(exceptionMessage, viewModel.ErrorMessage);
        }
    }

}

