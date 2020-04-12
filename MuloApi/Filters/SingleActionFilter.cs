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
                var dbUserCookie = await new ActionUserDataBase().Current.GetDataCookieUser(cookieUser);
                if (dbUserCookie != null) // есть куки в базе данных
                {
                    if (dbUserCookie.End < DateTime.Now) //если срок действия куки истёк и не вышел из аккаунта
                    {
                        var newCookie = await new ActionUserDataBase().Current.SaveCookieUser(dbUserCookie.IdUser,
                            context.HttpContext.Request.Headers, true, dbUserCookie);
                        var newSettingCookie = new CookieOptions
                        {
                            HttpOnly = true
                        };
                        context.HttpContext.Response.Cookies.Append("session", newCookie, newSettingCookie);
                    }

                    if (routeData[0].ToString().Equals("ConnectUser")
                    ) // если пытается авторизироваться то возвращаем idUser по действительному куки
                    {
                        context.Result = new JsonResult(new
                            {
                                user_id = dbUserCookie.IdUser
                            })
                            {StatusCode = 200};
                        return;
                    }

                    if (context.ActionArguments.ContainsKey("idUser")
                    ) // меняем idUser у адресной строки в соотвествии с куки если не авторизируется
                        context.ActionArguments["idUser"] = dbUserCookie.IdUser;
                    await next();
                    return;
                }
            }

            context.HttpContext.Response.StatusCode = 401;
            if (routeData[0].ToString().Equals("ConnectUser") || routeData[0].ToString().Equals("CreateUser")
            ) // если авторизируется или регистрируется без куки то пропускаем
                await next();
        }
    }
}