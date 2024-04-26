using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using Market.DAL;
using Market.DAL.Repositories;
using Market.DI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Market.MiddleWare;


public class MyAuthMiddleWare
{
    private readonly RequestDelegate _next;
    private readonly IUsersRepository _usersRepository;

    public MyAuthMiddleWare(RequestDelegate next)
    {
        _next = next;
        _usersRepository = new UsersRepository(new RepositoryContext());
    }

   public async Task Invoke(HttpContext httpContext)
   {
       
       MarkPointRespHeader(httpContext, "MyAuthMiddleWare1", "Point1");
       //если такое действие то делаем так
       if (httpContext.Request.Path != "/v1/products"&&httpContext.Request.Method=="POST")
       {
           MarkPointRespHeader(httpContext, "MyAuthMiddleWare2", "PointSkipAuthInMiddle");
           await _next(httpContext);
           return;
       }
       MarkPointRespHeader(httpContext, "MyAuthMiddleWare3", "Point2");
       
       AuthenticationHeaderValue.TryParse(httpContext.Request.Headers.Authorization,out AuthenticationHeaderValue? authHeader);// Basic Login;

       if (authHeader==null||string.IsNullOrWhiteSpace(authHeader.Parameter))
       {
           httpContext.Response.StatusCode = 401;
           return;// Task.CompletedTask;
       }
       
       if (authHeader.Scheme != "Basic")
       {
           httpContext.Response.StatusCode = 401;
           return;// Task.CompletedTask;
       }

       var credentialsBytes = Convert.FromBase64String(authHeader.Parameter);
       var rawCredential = Encoding.UTF8.GetString(credentialsBytes);
       var credentials = rawCredential.Split(':');
       var login = credentials[0];
       var pass = credentials[1];

       var checkResult = _usersRepository.CheckPass(login, pass);
       if (checkResult != null)
           await _next(httpContext);
       else
       {
           httpContext.Response.StatusCode = 401;
           return;// Task.CompletedTask;
       }
           //return context.httpContext.Request. UseExceptionHandler("/Error");//HandleExceptionAsync(StatusCodes.Status401Unauthorized);
       //new StatusCodeResult(StatusCodes.Status205ResetContent)
   }

   private object HandleExceptionAsync(int status401Unauthorized)
   {
       throw new NotImplementedException();
       //await httpContext.Response.StatusCode=(int)

   }

   private void MarkPointRespHeader(HttpContext httpContext,string header,string point)
   {
       httpContext.Response.OnStarting(() =>
       {
           httpContext.Response.Headers.Add(header, point);
           return Task.CompletedTask;
       });
   }
}