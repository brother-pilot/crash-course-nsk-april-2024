using Market.DAL.Repositories;
using Market.DTO;
using Market.Misc;
using Market.Models;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

[ApiController]
[Route("orders")]
public class OrdersController: ControllerBase
{
    private OrdersRepository OrdersRepository { get; }
    
    public OrdersController()
    {
        OrdersRepository = new OrdersRepository();
    }
    
    [HttpPost()]
    public async Task<IActionResult> CreateAsync([FromBody]OrderDto orderDto)
    {
        var result = await OrdersRepository.CreateOrderAsync(new Order()
        {
            Id = orderDto.Id,
            State = OrderState.Created,
            CustomerId = orderDto.CustomerId,
            ProductId = orderDto.ProductId,
            SellerId = orderDto.SellerId
        });

        return ParserDbResult.DbResultIsSuccessful(result, out var error)
            ? Ok()
            : error;
    }
    
    [HttpPost("{orderId:guid}/set-state")]
    public async Task<IActionResult> SetStateAsync([FromRoute]Guid orderId,[FromBody] OrderState orderState)
    {
        var result = await OrdersRepository.SetStateOrderAsync(orderId,orderState);
        
        return ParserDbResult.DbResultIsSuccessful(result, out var error)
            ? Ok()
            : error;
            
    }
    
    //важно путем помечать что мы отдаем наружу, если не конкретезировать, что watch
    //то тогда будет не понятно что делает этот запрос
    [HttpGet("{sellerId:guid}/watch")]
    public async Task<IActionResult> GetOrdersAsync([FromRoute]Guid sellerId,[FromBody] bool onlyCreated,
        [FromQuery] bool all)
    {
        var result = await OrdersRepository.GetOrdersForSeller(sellerId, onlyCreated, all);

        var orderDtos = result.Result.Select(OrderDto.FromModel);
        return new JsonResult(orderDtos);
    }
}