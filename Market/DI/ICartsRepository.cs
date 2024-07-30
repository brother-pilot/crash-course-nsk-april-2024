using Market.DAL;
using Market.Models;

namespace Market.DI;

public interface ICartsRepository
{
    Task<Cart> GetCartAsync(Guid customerId);
    Task<bool> AddOrRemoveProductToCartAsync(Guid customerId, Guid productId, bool isRemove);
    Task<bool> ClearAll(Guid customerId);
}