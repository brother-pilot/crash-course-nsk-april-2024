using Market.DTO;
using Market.Models;
using Microsoft.EntityFrameworkCore;

namespace Market.DAL.Repositories;

internal class OrdersRepository
{
    private readonly RepositoryContext _context;
    
    public OrdersRepository()
    {
        _context = new RepositoryContext();
    }

    public async Task<DbResult> CreateOrderAsync(Order order)
    {
        try
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return new DbResult(DbResultStatus.Ok);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new DbResult(DbResultStatus.UnknownError);
        }
    }

    public async Task<DbResult> SetStateOrderAsync(Guid orderId, OrderState newOrderState)
    {
        var orderForUpdate = await _context.Orders.FirstOrDefaultAsync(o => o.Id.Equals(orderId));
        if (orderForUpdate is null)
            return new DbResult(DbResultStatus.NotFound);
        
        orderForUpdate.State = newOrderState;
        try
        {
            await _context.SaveChangesAsync();
            return new DbResult(DbResultStatus.Ok);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new DbResult(DbResultStatus.UnknownError);
        }
    }


    public async Task<DbResult<IReadOnlyCollection<Order>>> GetOrdersForSeller(Guid sellerId, Boolean onlyCreated, bool all)
    {
        var query = _context.Orders.Where(o => o.SellerId == sellerId);

        if (onlyCreated)
        {
            query = query.Where(o => o.State == OrderState.Created);
        }

        var orders = await query.ToListAsync();
        return new DbResult<IReadOnlyCollection<Order>>(orders, DbResultStatus.Ok);
    }
}