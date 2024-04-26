using Market.DAL;
using Market.Models;

namespace Market.DI;

public interface ICartsRepository
{
    Task<DbResult<Cart>> GetCartAsync(Guid customerId);
    Task<DbResult> AddOrRemoveProductToCartAsync(Guid customerId, Guid productId, bool isRemove);
    Task<DbResult> ClearAll(Guid customerId);
}