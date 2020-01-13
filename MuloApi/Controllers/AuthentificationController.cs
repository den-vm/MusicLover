using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MuloApi.Classes;
using MuloApi.Models;

namespace MuloApi.Controllers
{
    [ApiController]
    public class AuthentificationController : ControllerBase
    {
        private readonly CheckDataUser _checkDataUser = new CheckDataUser();

        [HttpPost]
        [Route("/authorization")]
        public async Task<JsonResult> ConnectUser(ModelUser dataUser)
        {
            if (_checkDataUser.CheckLogin(dataUser.Login) && _checkDataUser.CheckPassword(dataUser.Password))
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

        [HttpPost]
        [Route("/registration")]
        public async Task<JsonResult> CreateUser(ModelUser dataUser)
        {
            if (!_checkDataUser.CheckLogin(dataUser.Login))
            {
                Response.StatusCode = 401;
                return new JsonResult(new
                {
                    errors = new
                    {
                        message = "INCORRECT_LOGIN"
                    }
                });
            }
            if (!_checkDataUser.CheckPassword(dataUser.Password))
            {
                Response.StatusCode = 401;
                return new JsonResult(new
                {
                    errors = new
                    {
                        message = "INCORRECT_PASSWORD"
                    }
                });
            }
            Response.StatusCode = 200;
            return new JsonResult(new
            {
                user_id = 64315,
                login = dataUser.Login
            });
        }
    }
}