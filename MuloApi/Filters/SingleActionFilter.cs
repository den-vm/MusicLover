using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MuloApi.DataBase.Control;

namespace MuloApi.Filters
{
    public class SingleActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var routeData = context.RouteData.Values.Values.ToArray();
            var cookieUser = context.HttpContext.Request.Cookies["session"];
            if (cookieUser != null)
            {
                var validCookie = await new ActionUserDataBase().Current.GetDataCookieUser(cookieUser);
                if (validCookie != null)
                {
                    if (validCookie.End > DateTime.Now) //если cookie существует и валидный
                    {
                        if (routeData[0].ToString().Equals("ConnectUser")) // если пытается авторизироваться то возвращаем idUser
                        {
                            context.Result = new JsonResult(new
                                {
                                    user_id = validCookie.IdUser
                                })
                                {StatusCode = 200};
                            return;
                        }

                        if (context.ActionArguments.ContainsKey("idUser")) // меняем idUser в соотвествии с cookie
                            context.ActionArguments["idUser"] = validCookie.IdUser;
                        await next();
                        return;
                    }

                    // обновление cookie
                    var newCookie = await new ActionUserDataBase().Current.SaveCookieUser(validCookie.IdUser,
                        context.HttpContext.Request.Headers, true, validCookie);
                    var newSettingCookie = new CookieOptions
                    {
                        HttpOnly = true
                    };
                    context.HttpContext.Response.Cookies.Append("session", newCookie, newSettingCookie);
                    if (routeData[0].ToString().Equals("ConnectUser")) // если пытается авторизироваться то возвращаем idUser
                    {
                        context.Result = new JsonResult(new
                            {
                                user_id = validCookie.IdUser
                            })
                            {StatusCode = 200};
                        return;
                    }

                    if (context.ActionArguments.ContainsKey("idUser")) // меняем idUser в соотвествии с cookie
                        context.ActionArguments["idUser"] = validCookie.IdUser;
                    await next();
                    return;
                }
            }

            context.HttpContext.Response.StatusCode = 401;
            if (routeData[0].ToString().Equals("ConnectUser") || routeData[0].ToString().Equals("CreateUser")) // если авторизируется или регистрируется то выполняем Action
                await next();
        }
    }
}