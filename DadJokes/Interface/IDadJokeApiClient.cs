using System;
using DadJokes.Models;
using System.Threading.Tasks;

namespace DadJokes.Interface
{
    public interface IDadJokeApiClient
    {
        public Task<TModel> GetAsync<TModel>(string url);
        public Task<TModel> GetAsync<TModel>(string url, Dictionary<string, string> urlParams);
    }

}

