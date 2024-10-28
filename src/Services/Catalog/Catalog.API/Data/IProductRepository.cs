using Catalog.API.Models;

namespace Catalog.API.Data;

public interface IProductRepostiory
{
    IEnumerable<Product> GetProducts();
    Product FindProduct(int productId);
    IEnumerable<Product> GetProductsByCategory(string category);
    bool CreateProduct(Product product);
    bool UpdateProduct(Product product);
    bool DeleteProduct(int productId);
}
