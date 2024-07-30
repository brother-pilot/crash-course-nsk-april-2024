using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Market.DAL;
using Market.DAL.Repositories;
using Market.DI;
using Market.DTO;
using Market.Helpers;
using Market.Misc;
using Market.Models;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

[ApiController]
[Route("users")]
public class UsersController:ControllerBase
{
    private IUsersRepository UsersRepository { get; }
    
    public UsersController()
    {
        UsersRepository = new UsersRepository(new RepositoryContext());
    }
    
    [HttpPost()]
    //Task<IActionResult> - ожидаем получить задачу у которой возращаемый тип будет IActionResult
    public async Task<IActionResult> CreateUserAsync([FromBody]UserDto user)
    {
        var salt = Guid.NewGuid().ToString();
        var passHash = PasswordHelper.GetPasswordHash(user.Pass,salt);
        HttpContext.Response.Headers.Add("HeaderUsersController", "point1");
        Debug.WriteLine("UsersController, point1");
        //HttpStatusCode.Conflict
        var existUser = UsersRepository.GetUser(user.Login);
        if (existUser!=null)
            return new StatusCodeResult(StatusCodes.Status409Conflict);
        //return new DbResult(StatusCodes.Status409Conflict);
        var result = await UsersRepository.CreateUserAsync(new User()
        {
            Id = user.Id,
            Name = user.Name,
            Login=user.Login,
            PasswordHash=passHash,
            Salt=salt,
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
    public async Task<IActionResult> GetProductsAsync([FromRoute]Guid sellerId,[FromBody] bool onlyCreated, bool all)
    {
        var result = await UsersRepository.GetOrdersForSeller(sellerId, onlyCreated, all);
        throw new NotImplementedException();
        //var orderDtos = result.Result.Select(OrderDto.FromModel);
        /* return result != null 
             ? ProductDto.FromModel(product) 
             : throw ErrorRegistry.NotFound("product", productId);*/
        //return new JsonResult(orderDtos);
    }
    
}



