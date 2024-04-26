

using Market.DAL;
using Market.DAL.Repositories;
using Market.DI;
using Market.Filters;
using Market.MiddleWare;
using Market.Validators;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
/*builder.Services.AddControllers(c =>
{
    c.Filters.Add<CheckAuthFilter>();
});//добавляем фильтры тут*/


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<RepositoryContext>()
    .AddScoped<IUsersRepository, UsersRepository>()
    .AddScoped<IMainValidator, MainValidator>()
    .AddScoped<IProductsRepository, ProductsRepository>()
    .AddScoped<ICartsRepository, CartsRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

//app.Run(ctx=>Task.Run(()=>ctx.Response.Headers.Add("My-Header","123")));
/*app.Use((ctx,next)=>Task.Run(async () =>
{
    ctx.Request.Headers.Add("My-Header2","123");
    await next(ctx);
    ctx.Response.Headers.Add("My-Header3toResponce","123");
    
}));
*/

//включи middleware
//app.UseMiddleware<MyAuthMiddleWare>();

/*app.Map("/ggg",(ctx,next)=>Task.Run(async () =>
{
    ctx.Request.Headers.Add("My-Header2","123");
    await next(ctx);
    ctx.Response.Headers.Add("My-Header3toResponce","123");
    
}));*/
//app.Run(handler=>Task.Run(()=context.Req));
app.Run();

