using Newtonsoft.Json;

namespace iTunesClient.Models
{
    public class Response<T>
    {
        [JsonProperty("resultCount")]
        public int ResultCount { get; set; }
        [JsonProperty("results")]
        public T[] Results { get; set; }
    }
}
