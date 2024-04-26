using Market.DAL;
using Market.Models;

namespace Market.DI;

public interface IOrdersRepository
{
    Task<DbResult> CreateOrderAsync(Order order);
    Task<DbResult> SetStateOrderAsync(Guid orderId, OrderState newOrderState);
    Task<DbResult<IReadOnlyCollection<Order>>> GetOrdersForSeller(Guid sellerId, Boolean onlyCreated, bool all);

}