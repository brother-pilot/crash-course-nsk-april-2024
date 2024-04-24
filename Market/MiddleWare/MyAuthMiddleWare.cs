using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using Market.DAL.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Market.MiddleWare;


public class MyAuthMiddleWare
{
    private readonly RequestDelegate _next;
    private readonly UsersRepository _usersRepository;

    public MyAuthMiddleWare(RequestDelegate next)
    {
        _next = next;
        _usersRepository = new UsersRepository();
    }

   //public async Task Invoke(HttpContext httpContext)
   public async Task OnActionExecutionAsync(ActionExecutingContext context,ActionExecutionDelegate next)
   {
       //если такое действие то делаем так
       if (context.HttpContext.Request.Path != "/products"&&context.HttpContext.Request.Method=="POST")
       {
           await _next(context.HttpContext);
           return;
       }
       
       var authHeader = AuthenticationHeaderValue.Parse(context.HttpContext.Request.Headers.Authorization);// Basic Login;

       if (authHeader.Scheme != "Basic")
       {
           context.Result = new BadRequestResult();//401
           return Task.CompletedTask;
       }
       if (string.IsNullOrWhiteSpace(authHeader.Parameter))
       {
           context.Result = new BadRequestResult();
           return Task.CompletedTask;
       }
           return;

       var credentialsBytes = Convert.FromBase64String(authHeader.Parameter);
       var rawCredential = Encoding.UTF8.GetString(credentialsBytes);
       var credentials = rawCredential.Split(':');
       var login = credentials[0];
       var pass = credentials[1];

       var checkResult = _usersRepository.CheckPass(login, pass);
       if (checkResult != null)
           await _next(context.httpContext);
       else
           return context.httpContext.Request. UseExceptionHandler("/Error");//HandleExceptionAsync(StatusCodes.Status401Unauthorized);
       //new StatusCodeResult(StatusCodes.Status205ResetContent)
   }

   private object HandleExceptionAsync(int status401Unauthorized)
   {
       
       await httpContext.Response.StatusCode=(int)
       
   }
}