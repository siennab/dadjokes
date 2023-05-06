using DadJokes.AppSettings;
using DadJokes.Interface;
using DadJokes.Models;
using Microsoft.Extensions.Options;

namespace DadJokes.Services
{
    public class DadJokeService : IDadJokeService
    {
        private readonly IDadJokeApiClient _client;
        private readonly IOptions<DadJokeAppSettings> _appSettings;

        private readonly string _randomJokeEndpointUrl;
        private readonly string _jokeSearchEndpointUrl;
        private readonly string _searchLimit;


        public DadJokeService(IDadJokeApiClient client, IOptions<DadJokeAppSettings> appSettings)
        {
            _client = client;
            _appSettings = appSettings;

            _randomJokeEndpointUrl = appSettings.Value.RandomJokeEndpointUrl;
            _jokeSearchEndpointUrl = appSettings.Value.SearchEndpointUrl;
            _searchLimit = appSettings.Value.DefaultSearchLimit;
        }

        public async Task<DadJoke> GetRandomJoke()
        {
            return await _client.GetAsync<DadJoke>(_randomJokeEndpointUrl!);
        }

        public async Task<DadJokeSearchResults> SearchJokes(string term)
        {
            return await _client.GetAsync<DadJokeSearchResults>(
                _jokeSearchEndpointUrl!,
                new Dictionary<string, string> {
                    { "term", term },
                    { "limit", _searchLimit }
                });
        }
    }
}

