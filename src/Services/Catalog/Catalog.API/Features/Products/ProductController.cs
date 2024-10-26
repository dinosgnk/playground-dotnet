using Dapper;
using Microsoft.AspNetCore.Mvc;

using Catalog.API.Data;
using Catalog.API.Models;

namespace Catalog.API.Features.Products;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly DataContextDapper _dapper;
    public ProductController(IConfiguration config)
    {
        Console.WriteLine("ProductController created");
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("")]
    public IEnumerable<Product> GetProducts()
    {
        string sql = @"
            SELECT
                ProductId,
                Name,
                Category,
                Description,
                Price
            FROM
                products.Product";

        IEnumerable<Product> products = _dapper.LoadData<Product>(sql);
        return products;
    }

    [HttpGet("{productId}")]
    public Product GetProductById(int productId)
    {
        string sql = @"
            SELECT
                ProductId,
                Name,
                Category,
                Description,
                Price
            FROM
                products.Product
            WHERE
                ProductId = @ProductId";

        var paramDictionary = new Dictionary<string, object>
        {
            { "@ProductId", productId }
        };
        var parameters = new DynamicParameters(paramDictionary);

        Product product = _dapper.LoadSingleData<Product>(sql, parameters);
        return product;
    }

    [HttpGet("category/{category}")]
    public IEnumerable<Product> GetProductByCategory(string category)
    {
        string sql = @"
            SELECT
                ProductId,
                Name,
                Category,
                Description,
                Price
            FROM
                products.Product
            WHERE
                Category = @Category";

        var paramDictionary = new Dictionary<string, object>
        {
            { "@Category", category }
        };
        var parameters = new DynamicParameters(paramDictionary);

        IEnumerable<Product> products = _dapper.LoadData<Product>(sql, parameters);
        return products;
    }

    [HttpPost("")]
    public IActionResult CreateProduct([FromBody] Product product)
    {
        string sql = @"
            INSERT INTO products.Product(
                Name,
                Category,
                Description,
                Price
            ) VALUES (
                @Name,
                @Category,
                @Description,
                @Price
            )";

        var paramDictionary = new Dictionary<string, object>
        {
            { "@Name", product.Name },
            { "@Category", product.Category },
            { "@Description", product.Description },
            { "@Price", product.Price }
        };
        var parameters = new DynamicParameters(paramDictionary);

        if (_dapper.ExecuteSql(sql, parameters))
        {
            return Ok(); // Built-in method that comes from ControllerBase class that we are inheriting
        }
        throw new Exception("Failed to create product");
    }

    [HttpPut("")]
    public IActionResult UpdateProduct([FromBody] Product product)
    {
        string sql = @"
            UPDATE products.Product
            SET
                Name = @Name,
                Category = @Category,
                Description = @Description,
                Price = @Price
            WHERE
                ProductId = @ProductId";

        var paramDictionary = new Dictionary<string, object>
        {
            { "@ProductId", product.ProductId },
            { "@Name", product.Name },
            { "@Category", product.Category },
            { "@Description", product.Description },
            { "@Price", product.Price }
        };
        var parameters = new DynamicParameters(paramDictionary);

        if (_dapper.ExecuteSql(sql, parameters))
        {
            return Ok();
        }
        throw new Exception("Failed to update product");
    }

    [HttpDelete("{productId}")]
    public IActionResult DeleteProduct(int productId)
    {
        string sql = @"
            DELETE FROM
                products.Product
            WHERE
                ProductId = @ProductId";

        var paramDictionary = new Dictionary<string, object>
        {
            { "@ProductId", productId }
        };
        var parameters = new DynamicParameters(paramDictionary);

        if (_dapper.ExecuteSql(sql, parameters))
        {
            return Ok();
        }
        throw new Exception("Failed to delete product");
    }
}
