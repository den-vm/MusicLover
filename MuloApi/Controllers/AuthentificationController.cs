using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MuloApi.Classes;
using MuloApi.DataBase;
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
        public async Task<JsonResult> ConnectUser(ModelConnectingUser dataUser)
        {
            if (_checkDataUser.CheckLoginRegular(dataUser.Login) && _checkDataUser.CheckPassword(dataUser.Password))
                if (await AppDBContent.TestConnection())
                {
                    var idUser = _controlDb.GetUserId(dataUser.Login);
                    if (idUser != -1)
                    {
                        Response.StatusCode = 200;
                        return new JsonResult(new
                        {
                            user_id = idUser,
                            login = dataUser.Login
                        });
                    }
                }
                else
                {
                    Response.StatusCode = 521;
                    return new JsonResult(new
                    {
                        error = "ERRORSERVER"
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
        public async Task<JsonResult> CreateUser(ModelConnectingUser dataUser)
        {
            if (!_checkDataUser.CheckLoginRegular(dataUser.Login))
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

            if (await AppDBContent.TestConnection())
            {
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
                    var result = _controlDb.GetUserId(dataUser.Login);
                    if (result != -1)
                    {
                        Response.StatusCode = 200;
                        return new JsonResult(new
                        {
                            user_id = result,
                            login = dataUser.Login
                        });
                    }

                    Response.StatusCode = 500;
                    return new JsonResult(new
                    {
                        error = "ERRORSERVER"
                    });
                }
            }

            Response.StatusCode = 521;
            return new JsonResult(new
            {
                error = "ERRORSERVER"
            });
        }
    }
}