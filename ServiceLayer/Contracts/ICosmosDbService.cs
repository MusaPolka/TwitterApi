using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Contracts
{
    public interface ICosmosDbService
    {
        Task AddProductAsync(Product product);
        Task DeleteProductAsync(string id);
        Task<Product> GetProductAsync(string id);
        Task<IEnumerable<Product>> GetProductsAsync(string queryString);
        Task UpdateProductAsync(string id, Product product);

    }
}
