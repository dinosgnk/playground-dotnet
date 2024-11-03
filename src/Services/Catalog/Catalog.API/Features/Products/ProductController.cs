using Microsoft.AspNetCore.Mvc;

using Catalog.API.Data;
using Catalog.API.Models;
using Microsoft.AspNetCore.Connections;
using StackExchange.Redis;

namespace Catalog.API.Features.Products;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductRepostiory _repository;
    public ProductController(IConfiguration config, IConnectionMultiplexer connectionMultiplexer)
    {
        Console.WriteLine("ProductController created");
        _repository = new ProductRepositoryDapper(config, connectionMultiplexer);
    }

    [HttpGet("")]
    public IEnumerable<Product> GetProducts()
    {
        return _repository.GetProducts();
    }

    [HttpGet("{productId}")]
    public Product GetProductById(int productId)
    {
        return _repository.FindProduct(productId);
    }

    [HttpGet("category/{category}")]
    public IEnumerable<Product> GetProductByCategory(string category)
    {
        return _repository.GetProductsByCategory(category);
    }

    [HttpPost("")]
    public IActionResult CreateProduct([FromBody] Product product)
    {
        if (_repository.CreateProduct(product))
        {
            return Ok();
        }
        throw new Exception("Failed to create product");
    }

    [HttpPut("")]
    public IActionResult UpdateProduct([FromBody] Product product)
    {
        if (_repository.UpdateProduct(product))
        {
            return Ok();
        }
        throw new Exception("Failed to update product");
    }

    [HttpDelete("{productId}")]
    public IActionResult DeleteProduct(int productId)
    {
        if (_repository.DeleteProduct(productId))
        {
            return Ok();
        }
        throw new Exception("Failed to delete product");
    }
}
