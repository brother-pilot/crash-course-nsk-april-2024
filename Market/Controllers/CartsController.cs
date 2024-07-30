using Market.DAL;
using Market.DAL.Repositories;
using Market.DI;
using Market.Misc;
using Market.Models;
using Microsoft.AspNetCore.Mvc;
using Market.Exceptions;
using Market.MiddleWare;

namespace Market.Controllers;

[ApiController]
[Route("/customers/{customerId:guid}/cart")]
public class CartsController:ControllerBase
{
    private ICartsRepository CartsRepository { get; }
    
    public CartsController()
    {
        CartsRepository = new CartsRepository();
    }

    //[HttpGet()]
    //public async Task<IActionResult> GetCartAsync([FromRoute]Guid customerId)
    //{
    //    HttpContext.Request.Headers.Add("CartsControleer","EnterGetCartAsync");
    //    var cart = await CartsRepository.GetCartAsync(customerId);
    //    //GeneralClass.GetResponceFromDB(result,) 
    //    return ParserDbResult.DbResultIsSuccessful(cart, out var error)
    //        ? new JsonResult(cart)
    //        : error;

    //    /*var productResult = await ProductsRepository.GetProductAsync(customerId);
    //    return DbResultIsSuccessful(productResult, out var error)
    //        ? new JsonResult(productResult.Result)
    //        : error;
    //        */
    //}
    [HttpGet]
    public async Task<ActionResult<Cart>> GetCartAsync([FromRoute] Guid customerId)
    {
        var cart = await CartsRepository.GetCartAsync(customerId);
        if (cart == null)
        {
            return NotFound($"Cart for customer with id {customerId} not found!");
            //throw new AdminException(404, $"Cart for customer with id {customerId} not found!");
            //return new DbResult(DbResultStatus.NotFound);
        }
        return cart;
    }
    //по умолчанию берет данные из query  параметров если ему не указать [FromBody]
    [HttpPatch("add-product")]
    public async Task<IActionResult> AddProductAsync(Guid customerId,[FromBody] Product product)
    {
       
            var result = await CartsRepository.AddOrRemoveProductToCartAsync(customerId, product.Id, false);
            return result
            ? Ok()
            : NotFound($"Cart for customer with id {customerId} not found!");
    }
        /*var productResult = await ProductsRepository.GetProductAsync(customerId);
        return DbResultIsSuccessful(productResult, out var error)
            ? new JsonResult(productResult.Result)
            : error;
            */
    
    //post запрещает брать данные из query, post позволяет брать из body
    //может быть по запросу не понятно что именно Guid нужен, поэтому при расширении
    //или модификации можно создать какую свою DTO на этот запрос
    [HttpPost("remove-product")]
    public async Task<IActionResult> RemoveProductAsync(Guid customerId,[FromBody] Guid productId)
    {
        var result = await CartsRepository.AddOrRemoveProductToCartAsync(customerId, productId, true);

        return result
            ? Ok()
            : NotFound($"Cart for customer with id {customerId} not found!");

    }
    
    [HttpPost("clear")]
    public async Task<IActionResult> ClearAsync(Guid customerId)
    {
        var result = await CartsRepository.ClearAll(customerId);
        
        return result
            ? Ok()
            : NotFound($"Cart for customer with id {customerId} not found!");
    }   
}
