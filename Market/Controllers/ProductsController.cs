using Market.DAL;
using Market.DAL.Repositories;
using Market.DI;
using Market.DTO;
using Market.Enums;
using Market.Filters;
using Market.Misc;
using Market.Models;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

//[]
[ApiController]
[Route("[controller]")]//тогда в адрес берет первую часть названия контроллера
//[Route("/v1/products")]
public sealed class ProductsController : ControllerBase
{
    
    private IProductsRepository ProductsRepository { get; }
    private IMainValidator _mainValidator { get; }
    
    public ProductsController(IProductsRepository productsRepository,IMainValidator  mainValidator)
    {
        ProductsRepository = productsRepository;
        _mainValidator = mainValidator;
    }

    
//[HttpGet("GetProductById")]
    [HttpGet("{productId:guid}")]
    public async Task<IActionResult> GetProductByIdAsync(Guid productId)
    {
        var productResult = await ProductsRepository.GetProductAsync(productId);
        return ParserDbResult.DbResultIsSuccessful(productResult, out var error)
            ? new JsonResult(productResult.Result)
            : error;
    }
//[HttpPost("SearchProducts")]
//лучше было для поиска сделать отдельный контроллер search
    [HttpPost("Search")]
    public async Task<IActionResult> SearchProductsAsync([FromBody] SearchProductDTO requestInfo)
    {
        //throw new NotImplementedException("Нужно реализовать позже");
        var productsResult = await ProductsRepository.GetProductsAsync(
            productName:requestInfo.ProductName,
            productCategory:requestInfo.Category,
            take:requestInfo.Take,
            skip:requestInfo.Skip);
        
        if (!ParserDbResult.DbResultIsSuccessful(productsResult, out var error))
            return error;
        var sorteredProducts = SortProducts(productsResult.Result,requestInfo.SortType, requestInfo.Ascending)
            .Select(ProductDto.FromModel)
            .ToList();
        return new JsonResult(sorteredProducts);
        
    }
    
    [HttpPost("DifficultSearch")]
    public async Task<IActionResult> SearchDifficultProductsAsync([FromBody] SearchProductDTO requestInfo)
    {
        //throw new NotImplementedException("Нужно реализовать позже");
        var productsResult = await ProductsRepository.GetProductsAsync(
            productName:requestInfo.ProductName,
            productCategory:requestInfo.Category,
            take:requestInfo.Take,
            skip:requestInfo.Skip);
        
        if (!ParserDbResult.DbResultIsSuccessful(productsResult, out var error))
            return error;
        var sorteredProducts = SortProducts(productsResult.Result,requestInfo.SortType, requestInfo.Ascending)
            .Select(ProductDto.FromModel)
            .ToList();
        return new JsonResult(sorteredProducts);
        
    }

    private IEnumerable<Product> SortProducts(IEnumerable<Product> source, SortType? sortType, bool asc)
    {
        //if (sortType.HasValue)
        //    return source;
        if (sortType.Value==SortType.Name)
            return asc
                ? source.OrderBy(p => p.Name) 
                : source.OrderByDescending(p => p.Name);
        if (sortType.Value==SortType.Price)
            return asc
                ? source.OrderBy(p => p.PriceInRubles) 
                : source.OrderByDescending(p => p.PriceInRubles);
        return asc
            ? source.OrderBy(p => p.Name) 
            : source.OrderByDescending(p => p.Name);
    }

    //запрос будет Get
    [HttpGet()]
    public async Task<IActionResult> GetSellerProductsAsync(
        [FromQuery] Guid sellerId,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 50)
    {
        var productsResult = await ProductsRepository.GetProductsAsync(sellerId: sellerId, skip: skip, take: take);
        if (!ParserDbResult.DbResultIsSuccessful(productsResult, out var error))
            return error;

        var productDtos = productsResult.Result.Select(ProductDto.FromModel);
        return new JsonResult(productDtos);
    }
//[HttpPost("CreateProduct")]
    [HttpPost()]
    [CheckAuthFilter]
    public async Task<IActionResult> CreateProductAsync([FromBody] Product product)
    {
        _mainValidator.Validate(product);
        /*if (!validationResult.IsValid)
        {
            var errors=new List<object>();
            foreach (var validationResultError in validationResult.Errors)
            {
                errors.Add(new
                {
                    ErrorCode = validationResultError.ErrorCode,
                    PropertyName = validationResultError.PropertyName,
                    Message=validationResultError.ErrorMessage
                });

            }
        }*/
        var createResult = await ProductsRepository.CreateProductAsync(product);

        return ParserDbResult.DbResultIsSuccessful(createResult, out var error)
            ? new StatusCodeResult(StatusCodes.Status205ResetContent)
            : error;
    }
//HTTPPOST("UpdateProductById")]
    [HttpPut("UpdateProductById")]
    public async Task<IActionResult> UpdateProductAsync([FromRoute] Guid productId, [FromBody] UpdateProductRequestDto requestInfo)
    {
        var updateResult = await ProductsRepository.UpdateProductAsync(productId, new ProductUpdateInfo
        {
            Name = requestInfo.Name,
            Description = requestInfo.Description,
            Category = requestInfo.Category,
            PriceInRubles = requestInfo.PriceInRubles
        });

        return ParserDbResult.DbResultIsSuccessful(updateResult, out var error)
            ? new StatusCodeResult(StatusCodes.Status404NotFound)
            : error;
    }

    [HttpPost("DeleteProductById")]
    public async Task<IActionResult> DeleteProductAsync(Guid productId)
    {
        var deleteResult = await ProductsRepository.DeleteProductAsync(productId);

        return ParserDbResult.DbResultIsSuccessful(deleteResult, out var error)
            ? NotFound()//new StatusCodeResult(StatusCodes.Status404NotFound)//Status405MethodNotAllowed
            : error;
    }

    
}



