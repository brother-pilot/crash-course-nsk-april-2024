using Market.DAL;
using Market.Enums;
using Market.Models;

namespace Market.DI;

public interface IProductsRepository
{
    Task<DbResult<IReadOnlyCollection<Product>>> GetProductsAsync(
        Guid? sellerId = null,
        string? productName = null,
        ProductCategory? productCategory = null,
        int skip = 0,
        int take = 50);

    Task<DbResult<Product>> GetProductAsync(Guid productId);
    Task<DbResult> CreateProductAsync(Product product);
    Task<DbResult> UpdateProductAsync(Guid productId, ProductUpdateInfo updateInfo);
    Task<DbResult> DeleteProductAsync(Guid productId);
    
}