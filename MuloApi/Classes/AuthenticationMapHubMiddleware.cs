using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MuloApi.DataBase.Control;

namespace MuloApi.Classes
{
    public class AuthenticationMapHubMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMapHubMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var controlDataBase = new ActionUserDataBase().Current;
            var dataCookie = await controlDataBase.GetDataCookieUser(context.Request.Cookies["session"]);
            if (dataCookie == null && context.Request.Path.StartsWithSegments("/user/negotiate"))
                context.Response.StatusCode = 401;
            else
                await _next.Invoke(context);
        }
    }
}