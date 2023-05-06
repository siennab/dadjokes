using System.Text;
using System.Text.Json;
using System.Web;
using DadJokes.Interface;

namespace DadJokes.Clients
{
    public class DadJokeApiClient : IDadJokeApiClient
    {
        private readonly HttpClient _httpClient;

        public DadJokeApiClient(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task<TModel> GetAsync<TModel>(string url)
        {
            return await RequestAsync<TModel>(url);
        }

        public async Task<TModel> GetAsync<TModel>(string url, Dictionary<string, string> urlParams)
        {

            return await RequestAsync<TModel>(BuildUri(url, urlParams));
        }

        private async Task<TModel> RequestAsync<TModel>(string formattedUrl)
        {

            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await _httpClient.GetAsync(formattedUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                try
                {
                    var mappedModel = JsonSerializer.Deserialize<TModel>(jsonContent, options); ;

                    if (mappedModel != null)
                    {
                        return mappedModel!;
                    }
                    else
                    {
                        throw new HttpRequestException("API did not return expected result");
                    }
                }
                catch
                {
                    throw new FormatException($"API response is improperly formatted");
                }
            }
            else
            {
                throw new HttpRequestException($"API call failed with status code {response.StatusCode}");
            }
        }

        private static string BuildUri(string url, Dictionary<string, string> urlParams)
        {
            StringBuilder builder = new StringBuilder(url);
            builder.Append(url.Contains('?') ? "&" : "?");

            foreach (var param in urlParams)
            {
                builder.AppendFormat("{0}={1}&", param.Key, HttpUtility.UrlEncode(param.Value));
            }

            builder.Length--;

            return builder.ToString();
        }
    }
}

