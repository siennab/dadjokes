using DadJokes.Interface;
using DadJokes.Models;
using Microsoft.AspNetCore.Mvc;

namespace DadJokes.Controllers
{
    public class RandomJokeController : Controller
    {

        private readonly IDadJokeService _dadJokeService;

        public RandomJokeController(IDadJokeService dadJokeService)
        {
            _dadJokeService = dadJokeService;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var response = new DadJokeViewModel<DadJoke>();
            try
            {
                var randomJoke = await _dadJokeService.GetRandomJoke();
                response.Result = randomJoke;
            }
            catch (Exception e)
            {
                response.HasError = true;
                response.ErrorMessage = e.Message;
            }

            return View(response);
        }
    }
}

