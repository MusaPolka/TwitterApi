using DomainLayer.Entities;
using Microsoft.Azure.Cosmos;
using ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class CosmosDbService : ICosmosDbService
    {
        private Container _container;

        public CosmosDbService(CosmosClient dbClient, string databaseName, string containerName)
        {
            _container = dbClient.GetContainer(databaseName, containerName);
        }
        public async Task AddProductAsync(Product product)
        {
            await _container.CreateItemAsync(product, new PartitionKey(product.Id));
        }

        public async Task DeleteProductAsync(string id)
        {
            await _container.DeleteItemAsync<Product>(id, new PartitionKey(id));
        }

        public async Task<Product> GetProductAsync(string id)
        {
            var response = await _container.ReadItemAsync<Product>(id, new PartitionKey(id));
            return response.Resource;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(string queryString)
        {
            var query = _container.GetItemQueryIterator<Product>(new QueryDefinition(queryString));
            var results = new List<Product>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateProductAsync(string id, Product product)
        {
            await _container.UpsertItemAsync<Product>(product, new PartitionKey(id));
        }
    }
}
