using Microsoft.Azure.Cosmos;
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

        public async Task CreateOrUpdateUrlAsync(Url url)
        {
            try
            {
                // Upsert means insert or update based on the ID
                await _container.UpsertItemAsync(url, new PartitionKey(url.ShortenedUrl));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating/updating URL: {ex.Message}");
            }
        }

        // Get a URL by its Shortened URL
        public async Task<Url> GetUrlAsync(string shortenedUrl)
        {
            try
            {
                var response = await _container.ReadItemAsync<Url>(shortenedUrl, new PartitionKey(shortenedUrl));
                return response.Resource;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching URL: {ex.Message}");
                return null;
            }
        }

        // Get all URLs in the database (useful for debugging or admin purposes)
        public async Task<List<Url>> GetAllUrlsAsync()
        {
            try
            {
                var query = _container.GetItemQueryIterator<Url>("SELECT * FROM c");
                List<Url> urls = new List<Url>();

                while (query.HasMoreResults)
                {
                    var result = await query.ReadNextAsync();
                    urls.AddRange(result);
                }

                return urls;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching all URLs: {ex.Message}");
                return new List<Url>();
            }
        }

        // Delete a URL by its Shortened URL
        public async Task DeleteUrlAsync(string shortenedUrl)
        {
            try
            {
                await _container.DeleteItemAsync<Url>(shortenedUrl, new PartitionKey(shortenedUrl));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting URL: {ex.Message}");
            }
        }
    }
}
