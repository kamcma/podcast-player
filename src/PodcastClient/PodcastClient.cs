using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using System.Net.Http;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using System.Linq;

namespace PodcastClient
{
    public class PodcastClient
    {
        private readonly HttpClient client_;

        public PodcastClient(HttpClient client) => client_ = client;

        public async Task<List<ISyndicationItem>> Fetch(string url)
        {
            var stream = await client_.GetStreamAsync(url);

            return await Parse(stream);
        }

        private async Task<List<ISyndicationItem>> Parse(Stream stream)
        {
            using (var xmlReader = XmlReader.Create(stream, new XmlReaderSettings { Async = true }))
            {
                var feedReader = new RssFeedReader(xmlReader);
                var syndicationItems = new List<ISyndicationItem>();

                while (await feedReader.Read())
                {
                    switch (feedReader.ElementType)
                    {
                        case SyndicationElementType.Item:
                            syndicationItems.Add(await feedReader.ReadItem());
                            break;
                    }
                }

                return syndicationItems;
            }
        }
    }
}
