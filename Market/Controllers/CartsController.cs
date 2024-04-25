using Market.DAL;
using Market.DAL.Repositories;
using Market.Misc;
using Market.Models;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

[ApiController]
[Route("/customers/{customerId:guid}/cart888")]
public class CartsController:ControllerBase
{
    private CartsRepository CartsRepository { get; }
    
    public CartsController()
    {
        CartsRepository = new CartsRepository();
    }
    
    [HttpGet()]
    public async Task<IActionResult> GetProductsAsync(Guid customerId)
    {
        var result = await CartsRepository.GetCartAsync(customerId);
        //GeneralClass.GetResponceFromDB(result,) 
        return ParserDbResult.DbResultIsSuccessful(result, out var error)
            ? new JsonResult(result)
            : error;
            
        /*var productResult = await ProductsRepository.GetProductAsync(customerId);
        return DbResultIsSuccessful(productResult, out var error)
            ? new JsonResult(productResult.Result)
            : error;
            */
    }
    //по умолчанию берет данные из query  параметров если ему не указать [FromBody]
    [HttpPatch("add-product")]
    public async Task<IActionResult> AddProductAsync(Guid customerId,[FromBody] Product product)
    {
        var result = await CartsRepository.AddOrRemoveProductToCartAsync(customerId,product.Id,false);

        return ParserDbResult.DbResultIsSuccessful(result, out var error)
            ? Ok()
            : error;
        /*var productResult = await ProductsRepository.GetProductAsync(customerId);
        return DbResultIsSuccessful(productResult, out var error)
            ? new JsonResult(productResult.Result)
            : error;
            */
    }
    
    //post запрещает брать данные из query, post позволяет брать из body
    //может быть по запросу не понятно что именно Guid нужен, поэтому при расширении
    //или модификации можно создать какую свою DTO на этот запрос
    [HttpPost("remove-product")]
    public async Task<IActionResult> RemoveProductAsync(Guid customerId,[FromBody] Guid productId)
    {
        var result = await CartsRepository.AddOrRemoveProductToCartAsync(customerId, productId, true);

        return ParserDbResult.DbResultIsSuccessful(result, out var error)
            ? Ok()
            : error;
       
    }
    
    [HttpPost("clear")]
    public async Task<IActionResult> ClearAsync(Guid customerId)
    {
        var result = await CartsRepository.ClearAll(customerId);
        
        return ParserDbResult.DbResultIsSuccessful(result, out var error)
            ? Ok()
            : error;
        
    }
    
    
}