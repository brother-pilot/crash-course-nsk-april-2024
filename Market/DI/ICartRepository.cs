using Market.DAL;
using Market.Models;

namespace Market.DI;

internal interface ICartRepository
{
    Task<DbResult<Cart>> GetCartAsync(Guid customerId);
    Task<DbResult> AddOrRemoveProductToCartAsync(Guid customerId, Guid productId, bool isRemove);
    Task<DbResult> ClearAll(Guid customerId);
}