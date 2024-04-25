using System.Security.Cryptography;
using System.Text;
using Market.Models;
using Microsoft.EntityFrameworkCore;

namespace Market.DAL.Repositories;

internal class UsersRepository
{
    private readonly RepositoryContext _context;
    
    public UsersRepository()
    {
        _context = new RepositoryContext();
    }
    public async Task<DbResult> CreateUserAsync(User user)
    {
        try
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return new DbResult(DbResultStatus.Ok);
            //return DbResult<Guid>.Ok(user.Id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new DbResult(DbResultStatus.UnknownError);
        }
    }

    public async Task<DbResult> SetSellerAsync(Guid userId, bool isSeller)
    {
        var userForUpdate = await _context.Users.FirstOrDefaultAsync(u => u.Id.Equals(userId));
        if (userForUpdate is null)
            return new DbResult(DbResultStatus.NotFound);
        
        userForUpdate.IsSeller = true;
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


    public object CheckPass(string login, string pass)
    {
        //идем в базу данных и проверяем корректность введенных данных
        throw new NotImplementedException();
    }

    public static Task<DbResult<User>> FindUser(string login, string pass)
    {
        throw new NotImplementedException();
    }

    public async Task<DbResult<Order>> GetOrdersForSeller(Guid sellerId, bool onlyCreated, bool all)
    {
        throw new NotImplementedException();
    }
}