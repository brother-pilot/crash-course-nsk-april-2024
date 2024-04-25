namespace Market.Models;

public class Cart
{
    public Guid CustomerId { get; set; }

    public List<Guid> ProductIds { get; set; } = new();
    //public List<Product>? Products { get; set; }
    //public Guid ProductId { get; set; } //= Array.Empty<Guid>();
}