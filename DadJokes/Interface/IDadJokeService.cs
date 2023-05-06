using DadJokes.Models;

namespace DadJokes.Interface
{
    public interface IDadJokeService
    {
        public Task<DadJoke> GetRandomJoke();

        public Task<DadJokeSearchResults> SearchJokes(String query);
    }

}

