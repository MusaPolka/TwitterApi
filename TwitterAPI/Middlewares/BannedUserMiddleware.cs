using DomainLayer.Enums;
using ServiceLayer.Contracts;

namespace TwitterAPI.Middlewares
{
    public class BannedUserMiddleware
    {
        private readonly RequestDelegate _next;

        public BannedUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserService userService)
        {
            var nameClaim = context.User.Claims.FirstOrDefault(x => x.Type == "name");

            if (nameClaim != null)
            {
                var userName = nameClaim.Value;

                if (userName != null)
                {
                    var user = await userService.GetCurrentUserAsync(userName);

                    if(user != null)
                    {
                        if (user.Status == AccountStatus.Banned)
                        {
                            await context.Response.WriteAsync("You are banned!");
                        }
                        else
                        {
                            await _next(context);
                        }
                    }                  
                }
            }
            else
            {
                await _next(context);
            }
            
        }
    }
}
