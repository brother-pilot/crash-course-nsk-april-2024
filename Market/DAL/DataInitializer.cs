using Market.Enums;
using Market.Misc;
using Market.Models;

namespace Market.DAL;

internal static class DataInitializer
//для задания каких то начальных данных в БД
{
    private static readonly Random Random = Random.Shared;
    private static readonly ProductCategory[] Categories = Enum.GetValues<ProductCategory>();

    public static Product[] InitializeCarts(int count = 10)
    {
        return Enumerable.Range(1, count).Select(number =>
            new Product
            {
                Id = Guid.NewGuid(),
                Name = $"Product-{number}",
                Description = $"Some description for product-{number}",
                PriceInRubles = (decimal)Random.NextDouble(100, 10000),
                Category = Categories[Random.Next(Categories.Length)],
                SellerId = Guid.NewGuid()
            })
            .ToArray();
    }

    public static Cart[] InitializeProducts()
    {
        return new[]
        {
            new Cart()
            {
                CustomerId = Guid.Parse("B50C17D9-85F9-4BFD-BE6F-CB4ECBA97DB0")
                //Products  //new List<Product>();
            }
        };
    }
}