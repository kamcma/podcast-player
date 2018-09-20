using System.Threading.Tasks;
using System.Net.Http;
using static System.Net.WebUtility;
using iTunes = iTunesClient.Models;

namespace iTunesClient
{
    public class iTunesClient
    {
        private readonly HttpClient client_;

        public iTunesClient(HttpClient client) => client_ = client;

        public async Task<iTunes.Response<iTunes.Result>> Search(
            string term,
            iTunes.Media media,
            int limit = 10)
        {
            var response = await client_
                .GetAsync($"/search?term={UrlEncode(term)}&media={media.ToString().ToLower()}&limit={limit}");

            response.EnsureSuccessStatusCode();
            response.Content.Headers.ContentType.MediaType = "application/json";

            return await response.Content
                .ReadAsAsync<iTunes.Response<iTunes.Result>>();
        }

        public async Task<iTunes.Response<iTunes.Result>> Lookup(long id)
        {
            var response = await client_.GetAsync($"/lookup?id={id}");

            response.EnsureSuccessStatusCode();
            response.Content.Headers.ContentType.MediaType = "application/json";

            return await response.Content.ReadAsAsync<iTunes.Response<iTunes.Result>>();
        }
    }
}
