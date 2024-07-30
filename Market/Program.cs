

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

//builder.Services.AddSingleton<Logger>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//настраиваем DI
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



//включи middleware
//app.UseMiddleware<MyAuthMiddleWare>();


app.Run();

