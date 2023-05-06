using System.Text;
using System.Text.RegularExpressions;
using DadJokes.Interface;
using DadJokes.Models;
using Microsoft.AspNetCore.Mvc;

namespace DadJokes.Controllers
{

    public partial class JokeSearchController : Controller
    {
        private readonly IDadJokeService _dadJokeService;

        public JokeSearchController(IDadJokeService dadJokeService)
        {
            _dadJokeService = dadJokeService;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index(string term)
        {
            var response = new DadJokeViewModel<DadJokeSearchResults>();

            // Validate input
            if (term != null && !ValidTermInput().IsMatch(term))
            {
                response.HasError = true;
                response.ErrorMessage = "The input you provided contains characters that are not allowed. Please only use letters (A-Z, a-z) and numbers (0-9) in your input.";
                return View(response);
            }


            // Get results
            try
            {
                if (term != null)
                {
                    response.Result = await _dadJokeService.SearchJokes(term);
                    HighlightTerm(response.Result, term);
                }
            }
            catch (Exception e)
            {
                response.HasError = true;
                response.ErrorMessage = e.Message;
            }



            ViewBag.term = term;
            return View(response);
        }
        /// <summary>
        /// Put [] around search terms
        /// Treat each word as a separate term
        /// Since that is how the API performs the search
        /// </summary>
        /// <param name="results"></param>
        /// <param name="term"></param>
        private static void HighlightTerm(DadJokeSearchResults results, string term)
        {
            foreach (var item in results.Results)
            {
                var splitTerm = term.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                var stringBuilder = new StringBuilder(item.Joke);

                foreach (var word in splitTerm)
                {
                    var regex = new Regex(word, RegexOptions.IgnoreCase);
                    var matches = regex.Matches(item.Joke);

                    for (int i = matches.Count - 1; i >= 0; i--)
                    {
                        stringBuilder.Remove(matches[i].Index, matches[i].Length);
                        stringBuilder.Insert(matches[i].Index, $"[{matches[i]}]");
                    }
                }

                item.Joke = stringBuilder.ToString();
            }
        }

        [GeneratedRegex("^[a-zA-Z0-9\\s]*$")]
        private static partial Regex ValidTermInput();
    }
}

