using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Market.DAL;
using Market.DAL.Repositories;
using Market.DI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Market.Filters;


    //делаем фильтр
    public class CheckAuthFilter : ActionFilterAttribute,IAsyncActionFilter
    {
        //private readonly RequestDelegate _next;
        private readonly IUsersRepository _usersRepositore;

        public CheckAuthFilter()
        {
            //_next = next;
            _usersRepositore = new UsersRepository(new RepositoryContext());
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            AuthenticationHeaderValue.TryParse(context.HttpContext.Request.Headers.Authorization, out var authHeader);// Basic Login;

            if (authHeader==null||string.IsNullOrWhiteSpace(authHeader.Parameter))
            {
                context.HttpContext.Response.Headers.Add("HeaderAuthFilter","nonAuth");
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                return;
            }
            
            if (authHeader.Scheme != "Basic")
            {
                context.Result = new BadRequestResult();
                return;
            }
                
                //"401";
            

            var credentialsBytes = Convert.FromBase64String(authHeader.Parameter);
            var rawCredential = Encoding.UTF8.GetString(credentialsBytes);
            var credentials = rawCredential.Split(':');
            var login = credentials[0];
            var pass = credentials[1];
           
            var userId = _usersRepositore.FindUser(login, pass);
            if (userId==null)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                return;
            }

            var claimIdentity = new ClaimsIdentity();
            claimIdentity.AddClaim(new Claim("user-id",Guid.NewGuid().ToString()));
            context.HttpContext.User.AddIdentity(claimIdentity);
            
            context.HttpContext.Items.Add("user-id",Guid.NewGuid());

           // await next;
            /*var checkResult = _usersRepositore.CheckPass(login, pass);
            if (checkResult != null)
                await _next(httpContext);
            else
                return httpContext.Request. UseExceptionHandler("/Error");*/
                    
                    
        }
    }


