namespace Catalog.API.Models;

public class Product
{
    public int ProductId { get; set; }
    public string Name { get; set; } = default!;
    public string Category { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
}
