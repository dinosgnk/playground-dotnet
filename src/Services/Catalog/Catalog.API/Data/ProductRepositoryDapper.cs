using Catalog.API.Models;
using Dapper;
using Npgsql;

namespace Catalog.API.Data;

public class ProductRepositoryDapper : IProductRepostiory
{
    private readonly IConfiguration _config;
    private readonly NpgsqlConnection _connection;

    public ProductRepositoryDapper(IConfiguration config)
    {
        Console.WriteLine("ProductRepositoryDapper created");
        _config = config;
        _connection = new NpgsqlConnection(_config.GetConnectionString("DefaultConnection"));
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

        return _connection.Query<Product>(sql);
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

        return _connection.QuerySingle<Product>(sql, parameters);
    }

    public IEnumerable<Product> GetProductsByCategory(string category)
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

        return _connection.Query<Product>(sql, parameters);
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

        return _connection.Execute(sql, parameters) > 0;
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

        return _connection.Execute(sql, parameters) > 0;
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

        return _connection.Execute(sql, parameters) > 0;
    }
}
