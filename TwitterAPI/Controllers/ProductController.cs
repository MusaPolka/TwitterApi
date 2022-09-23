using DomainLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using ServiceLayer.Contracts;
using TwitterAPI.Extensions;

namespace TwitterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ICosmosDbService _cosmosDbService;
        private readonly IDistributedCache _cache;

        public ProductController(ICosmosDbService cosmosDbService, IDistributedCache cache)
        {
            _cosmosDbService = cosmosDbService;
            _cache = cache;
        }
        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            if (_cache.TryGetValue("productList", out IEnumerable<Product>? productsCache))
            {
                return Ok(productsCache);
            }

            var products = await _cosmosDbService.GetProductsAsync("SELECT * FROM c");

            var cacheEntryOptions = new DistributedCacheEntryOptions()
                       .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                       .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600));

            await _cache.SetAsync("productList", products, cacheEntryOptions);

            return Ok(products);
        }
        [HttpGet("GetProduct/{id}")]
        public async Task<IActionResult> GetProduct(string id)
        {
            if (_cache.TryGetValue("product", out IEnumerable<Product>? productChahe))
            {
                return Ok(productChahe);
            }

            var product = await _cosmosDbService.GetProductAsync(id);

            var cacheEntryOptions = new DistributedCacheEntryOptions()
                       .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                       .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600));

            await _cache.SetAsync("product", product, cacheEntryOptions);

            return Ok(product);
        }

        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            product.Id = Guid.NewGuid().ToString();
            await _cosmosDbService.AddProductAsync(product);

            return Ok(product);
        }
        [HttpPost("EditProduct")]
        public async Task<IActionResult> EditProduct(Product product)
        {
            await _cosmosDbService.UpdateProductAsync(product.Id, product);

            return Ok(product);
        }
        [HttpPost("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            await _cosmosDbService.DeleteProductAsync(id);

            return Ok();
        }
    }
}
