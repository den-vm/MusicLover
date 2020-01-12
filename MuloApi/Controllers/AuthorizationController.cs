using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MuloApi.Classes;
using MuloApi.Models;

namespace MuloApi.Controllers
{
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        [HttpPost]
        [Route("/authorization")]
        public async Task<JsonResult> ConnectUser(ModelUser dataUser)
        {
            var checkDataUser = new CheckDataUser();
            if (checkDataUser.CheckLogin(dataUser.Login) && checkDataUser.CheckPassword(dataUser.Password))
            {
                Response.StatusCode = 200;
                return new JsonResult(new
                {
                    user_id = 64315,
                    login = dataUser.Login
                });
            }

            Response.StatusCode = 401;
            return new JsonResult(new
            {
                errors = new
                {
                    message = "INCORRECT_PASSWORD_OR_LOGIN"
                }
            });
        }
    }
}