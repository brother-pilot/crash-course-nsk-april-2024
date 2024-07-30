using Market.DI;
using Market.Exceptions;
using Market.MiddleWare;
using Market.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;

namespace Market.DAL.Repositories;

internal class CartsRepository:ICartsRepository
{
    private readonly RepositoryContext _context;
    
    public CartsRepository()
    {
        _context = new RepositoryContext();
    }
    
    public async Task<Cart> GetCartAsync(Guid customerId) => await _context.Carts.FirstOrDefaultAsync(p => p.CustomerId == customerId);
    
    public async Task<bool> AddOrRemoveProductToCartAsync(Guid customerId, Guid productId, bool isRemove)
    {
        var cart = await _context.Carts.FirstOrDefaultAsync(p => p.CustomerId.Equals(customerId));

        if (cart == null)
        {
            throw new AdminException(404, $"Cart for customer with id {customerId} not found!");
        }

        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id.Equals(productId));
        if (product == null)
        {
            throw new AdminException(404, $"Product for customer with id {customerId} not found!");
        }

        try//
        {
            if (isRemove)
            {
                cart.ProductIds = new List<Guid>(cart.ProductIds);
                cart.ProductIds.Remove(productId);
            }
            else
            {
                cart.ProductIds = new List<Guid>(cart.ProductIds) { productId };
            }
            await _context.SaveChangesAsync();

            var res = await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception e)
        {
            throw new AdminException(500, e.Message);
        }
    }


        /*public async Task<DbResult<Cart>> GetProductAsync(Guid customerId)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(p => p.CustomerId.Equals(customerId));

            return cart != null
                ? new DbResult<Cart>(сart, DbResultStatus.Ok)
                : new DbResult<Cart>(null!, DbResultStatus.NotFound);
        }*/

    public async Task<bool> ClearAll(Guid customerId)
    {
        //var result = await CartsRepository.GetCartAsync(customerId);
        var cart = await _context.Carts.FirstOrDefaultAsync(p => p.CustomerId.Equals(customerId));
        
        try
        {
            cart.ProductIds = new List<Guid>();
            
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            throw new AdminException(500, e.Message);
        }
        
        
    }
    
}