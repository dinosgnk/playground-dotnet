using System.Data;
using System.Text.Json;
using Catalog.API.Models;
using Dapper;
using Npgsql;
using StackExchange.Redis;

namespace Catalog.API.Data;

public class ProductRepositoryDapper : IProductRepository
{
    private readonly DapperDbContext _dbContext;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private IDbConnection _dbConnection;

    public ProductRepositoryDapper(DapperDbContext dbContext, IConnectionMultiplexer connectionMultiplexer)
    {
        _dbContext = dbContext;
        _dbConnection = _dbContext.GetConnection();
        _connectionMultiplexer = connectionMultiplexer;
    }

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

        return _dbConnection.Query<Product>(sql);
    }

    public Product FindProduct(int productId)
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

        return _dbConnection.QuerySingle<Product>(sql, parameters);
    }

    public IEnumerable<Product> GetProductsByCategory(string category)
    {
        string cacheKey = category;
        var redis = _connectionMultiplexer.GetDatabase();
        var cachedCategoryRedisValue = redis.StringGet(cacheKey);

        if (cachedCategoryRedisValue.HasValue)
        {
            Console.WriteLine($"Got {category} category from Redis");
            return JsonSerializer.Deserialize<IEnumerable<Product>>(cachedCategoryRedisValue);
        }

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

        var products = _dbConnection.Query<Product>(sql, parameters);

        var serializedProducts = JsonSerializer.Serialize(products);
        redis.StringSet(cacheKey, serializedProducts);
        return products;
    }

    public bool CreateProduct(Product product)
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

        return _dbConnection.Execute(sql, parameters) > 0;
    }

    public bool UpdateProduct(Product product)
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

        return _dbConnection.Execute(sql, parameters) > 0;
    }

    public bool DeleteProduct(int productId)
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

        return _dbConnection.Execute(sql, parameters) > 0;
    }
}
