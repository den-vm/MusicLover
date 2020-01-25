using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MuloApi.Classes;
using MuloApi.DataBase;
using MuloApi.DataBase.Control;
using MuloApi.DataBase.Control.Interfaces;
using MuloApi.DataBase.Entities;
using MuloApi.Interfaces;
using MuloApi.Models;

namespace MuloApi.Controllers
{
    [ApiController]
    public class AuthentificationController : ControllerBase
    {
        private readonly ICheckData _checkDataUser = new CheckDataUser();
        private readonly IActionUser _controlDBUser = new ControlDataBase();

        [HttpPost]
        [Route("/authorization")]
        public async Task<JsonResult> ConnectUser(ModelConnectingUser dataUser)
        {
            if (dataUser?.Login != null && dataUser?.Password != null)
            {
                if (!_checkDataUser.CheckLoginRegular(dataUser.Login) ||
                    !_checkDataUser.CheckPassword(dataUser.Password))
                    return new JsonResult(new
                        {
                            errors = new
                            {
                                message = "INCORRECT_PASSWORD_OR_LOGIN"
                            }
                        })
                        {StatusCode = 401};

                if (!await AppDBContent<NewUser>.TestConnection())
                    return new JsonResult(new
                        {
                            error = "ERRORSERVER"
                        })
                        {StatusCode = 521};

                var idUser = _controlDBUser.GetUserId(dataUser.Login);
                if (idUser != -1)
                    return new JsonResult(new
                        {
                            user_id = idUser,
                            login = dataUser.Login
                        })
                        {StatusCode = 200};
            }

            return new JsonResult(new
                {
                    errors = new
                    {
                        message = "INCORRECT_PASSWORD_OR_LOGIN"
                    }
                })
                {StatusCode = 401};
        }

        [HttpPost]
        [Route("/registration")]
        public async Task<JsonResult> CreateUser(ModelConnectingUser dataUser)
        {
            if (dataUser?.Login != null && dataUser?.Password != null)
            {
                if (!_checkDataUser.CheckLoginRegular(dataUser.Login))
                    return new JsonResult(new
                        {
                            errors = new
                            {
                                message = "INCORRECT_LOGIN"
                            }
                        })
                        {StatusCode = 401};

                if (!_checkDataUser.CheckPassword(dataUser.Password))
                    return new JsonResult(new
                        {
                            errors = new
                            {
                                message = "INCORRECT_PASSWORD"
                            }
                        })
                        {StatusCode = 401};

                if (!await AppDBContent<ExistUser>.TestConnection())
                    return new JsonResult(new
                        {
                            error = "ERRORSERVER"
                        })
                        {StatusCode = 521};

                var resultExist = _controlDBUser.ExistUser(dataUser.Login);
                if (resultExist)
                    return new JsonResult(new
                        {
                            errors = new
                            {
                                message = "EXISTING_USER"
                            }
                        })
                        {StatusCode = 401};

                var resultAdd = _controlDBUser.AddUser(dataUser.Login, dataUser.Password);
                if (!resultAdd)
                    return new JsonResult(new
                        {
                            error = "ERRORSERVER"
                        })
                        {StatusCode = 521};

                var idUser = _controlDBUser.GetUserId(dataUser.Login);
                if (idUser != -1)
                {
                    IActionDirectory addDirectoryUser = new UserDirectory();
                    addDirectoryUser.CreateDirectoryUser(idUser);
                    return new JsonResult(new
                        {
                            user_id = idUser,
                            login = dataUser.Login
                        })
                        {StatusCode = 200};
                }
            }

            return new JsonResult(new
                {
                    error = "ERRORSERVER"
                })
                {StatusCode = 500};
        }

        [HttpGet]
        [Route("/user/{idUser:min(0)}/soundtracks")]
        public async Task<JsonResult> GetSoundTracksUser(int idUser)
        {
            return new JsonResult(new { testRequest = "Успешно" });
        }
    }
}