using Newtonsoft.Json;

namespace iTunesClient.Models
{
    public partial class Result
    {
        [JsonProperty("wrapperType")]
        public string WrapperType { get; set; }
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("artistId")]
        public long ArtistId { get; set; }
        [JsonProperty("collectionId")]
        public long CollectionId { get; set; }
        [JsonProperty("trackId")]
        public long TrackId { get; set; }
        [JsonProperty("bundleId")]
        public long BundleId { get; set; }

        [JsonProperty("artistName")]
        public string ArtistName { get; set; }
        [JsonProperty("collectionName")]
        public string CollectionName { get; set; }
        [JsonProperty("trackName")]
        public string TrackName { get; set; }

        [JsonProperty("collectionCensoredName")]
        public string CollectionCensoredName { get; set; }
        [JsonProperty("trackCensoredName")]
        public string TrackCensoredName { get; set; }

        [JsonProperty("artistViewUrl")]
        public string ArtistViewUrl { get; set; }
        [JsonProperty("collectionViewUrl")]
        public string CollectionViewUrl { get; set; }
        [JsonProperty("feedUrl")]
        public string FeedUrl { get; set; }
        [JsonProperty("trackViewUrl")]
        public string TrackViewUrl { get; set; }
        [JsonProperty("previewUrl")]
        public string PreviewUrl { get; set; }

        [JsonProperty("artworkUrl30")]
        public string ArtworkUrl30 { get; set; }
        [JsonProperty("artworkUrl60")]
        public string ArtworkUrl60 { get; set; }
        [JsonProperty("artworkUrl100")]
        public string ArtworkUrl100 { get; set; }
        [JsonProperty("artworkUrl512")]
        public string ArtworkUrl512 { get; set; }
        [JsonProperty("ArtworkUrl600")]
        public string ArtworkUrl600 { get; set; }
    }
}
