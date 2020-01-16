using Microsoft.AspNetCore.Mvc;
using MuloApi.Classes;
using MuloApi.DataBase.Control;
using MuloApi.Models;

namespace MuloApi.Controllers
{
    [ApiController]
    public class AuthentificationController : ControllerBase
    {
        private readonly CheckDataUser _checkDataUser = new CheckDataUser();
        private readonly ControlDataBase _controlDb = ControlDataBase.Instance();
        [HttpPost]
        [Route("/authorization")]
        public JsonResult ConnectUser(ModelConnectingUser dataUser)
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
        public JsonResult CreateUser(ModelConnectingUser dataUser)
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

            var resultExist = _controlDb.ExistUser(dataUser.Login);
            if (resultExist)
            {
                Response.StatusCode = 401;
                return new JsonResult(new
                {
                    errors = new
                    {
                        message = "EXISTING_USER"
                    }
                });
            }

            var resultAdd = _controlDb.AddUser(dataUser.Login, dataUser.Password);
            if (resultAdd)
            {
                var idUser = _controlDb.GetUserId(dataUser.Login);
                Response.StatusCode = 200;
                return new JsonResult(new
                {
                    user_id = idUser,
                    login = dataUser.Login
                });
            }
            Response.StatusCode = 401;
            return new JsonResult(new
            {
                error = "ERRORSERVER"
            });
        }
    }
}