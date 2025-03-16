using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortenerAPI.Models;

namespace UrlShortenerAPI.Services
{
    public class CosmosDbService
    {
        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;

        public CosmosDbService(string connectionString, string databaseName, string containerName)
        {
            _cosmosClient = new CosmosClient(connectionString);
            var database = _cosmosClient.GetDatabase(databaseName);
            _container = database.GetContainer(containerName);
        }

        public async Task<Url> CreateUrlAsync(Url url)
        {
            // Create the shortened URL in the database
            var result = await _container.CreateItemAsync(url, new PartitionKey(url.ShortenedUrl));
            return result.Resource;
        }

        public async Task<Url> GetUrlAsync(string shortenedUrl)
        {
            try
            {
                var response = await _container.ReadItemAsync<Url>(shortenedUrl, new PartitionKey(shortenedUrl));
                return response.Resource;
            }
            catch (CosmosException)
            {
                return null;
            }
        }

        public async Task<List<Url>> GetAllUrlsAsync()
        {
            var query = _container.GetItemQueryIterator<Url>("SELECT * FROM c");
            List<Url> urls = new List<Url>();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                urls.AddRange(response);
            }

            return urls;
        }
    }
}
