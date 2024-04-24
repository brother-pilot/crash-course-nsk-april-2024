using System.Security.Cryptography;
using System.Text;
using Market.DAL.Repositories;
using Market.DTO;
using Market.Misc;
using Market.Models;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

[ApiController]
[Route("users")]
public class UsersController:ControllerBase
{
    private UsersRepository UsersRepository { get; }
    
    public UsersController()
    {
        UsersRepository = new UsersRepository();
    }
    
    [HttpPost()]
    public async Task<IActionResult> CreateUserAsync([FromBody]UserDto user)
    {
        var salt = Guid.NewGuid();
        var passHash = GenerateHash(user.Pass+salt);
       
        var result = await UsersRepository.CreateUserAsync(new User()
        {
            Id = user.Id,
            Name = user.Name,
            Login=user.Login,
            Pass=passHash,
            Salt=salt.ToString(),
            IsSeller=false
        });

        
        

        return ParserDbResult.DbResultIsSuccessful(result, out var error)
            ? new JsonResult(user.Id)
            : error;
    }
    
    [HttpPost("{orderId:guid}/add-to-seller")]
    public async Task<IActionResult> SetSellerAsync([FromRoute]Guid userId,[FromBody] bool isSeller)
    {
        var result = await UsersRepository.SetSellerAsync(userId,isSeller);
        
        return ParserDbResult.DbResultIsSuccessful(result, out var error)
            ? Ok()
            : error;
            
    }
    
    [HttpGet("/products")]
    public async Task<IActionResult> GetProductsAsync([FromRoute]Guid sellerId,[FromBody] )
    {
        var result = await OrdersRepository.GetOrdersForSeller(sellerId, onlyCreated, all);

        var orderDtos = result.Result.Select(OrderDto.FromModel);
        return new JsonResult(orderDtos);
    }
    
    private HMACMD5 _md5 = new HMACMD5();

    private string GenerateHash(string pass)
    {
        var bytes = Encoding.UTF8.GetBytes(pass);
        var computeHash = _md5.ComputeHash(bytes);
        return Encoding.Default.GetString(computeHash);
    }
}



