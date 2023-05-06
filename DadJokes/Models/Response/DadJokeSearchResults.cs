using System.Text.Json.Serialization;

namespace DadJokes.Models
{
    public class DadJokeSearchResults
    {

        public DadJokeSearchResults()
        {
        }

        [JsonPropertyName("current_page")]
        public int CurrentPage { get; set; }

        [JsonPropertyName("next_page")]
        public int NextPage { get; set; }

        [JsonPropertyName("previous_page")]
        public int PreviousPage { get; set; }

        public int Limit { get; set; }

        public IEnumerable<DadJoke> Results { get; set; } = new List<DadJoke>();

        /// <summary>
        /// Group jokes by their word count 
        /// </summary>
        [JsonIgnore]
        public IEnumerable<IGrouping<string, DadJoke>> GroupedResults
        {
            get
            {
                return this.Results.GroupBy((r) =>
                {
                    var words = r.Joke.Split(
                        new char[] { ' ', '\t', '\n', '\r' },
                        StringSplitOptions.RemoveEmptyEntries);

                    if (words.Length < 10)
                    {
                        return "Short";
                    }
                    else if (words.Length < 20)
                    {
                        return "Medium";
                    }
                    else
                    {
                        return "Long";
                    }
                });

            }
        }
    }
}

