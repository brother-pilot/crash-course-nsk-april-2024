using Market.DAL;
using Market.Models;

namespace Market.DI;

public interface IUsersRepository
{
    Task<DbResult> CreateUserAsync(User user);
    Task<DbResult> SetSellerAsync(Guid userId, bool isSeller);
    bool CheckPass(string login, string pass);
   Task<DbResult<User>> FindUser(string login, string pass);
   Task<User?> GetUser(string login);
    Task<DbResult<Order>> GetOrdersForSeller(Guid sellerId, bool onlyCreated, bool all);
}