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
            if (!_checkDataUser.CheckLoginRegular(dataUser.Login) || !_checkDataUser.CheckPassword(dataUser.Password))
                return new JsonResult(new
                {
                    errors = new
                    {
                        message = "INCORRECT_PASSWORD_OR_LOGIN"
                    }
                })
                { StatusCode = 401 };

            if (!await AppDBContent.TestConnection())
                return new JsonResult(new
                {
                    error = "ERRORSERVER"
                })
                { StatusCode = 521 };

            var idUser = _controlDb.GetUserId(dataUser.Login);
            if (idUser != -1)
                return new JsonResult(new
                {
                    user_id = idUser,
                    login = dataUser.Login
                })
                { StatusCode = 200 };

            return new JsonResult(new
            {
                errors = new
                {
                    message = "INCORRECT_PASSWORD_OR_LOGIN"
                }
            })
            { StatusCode = 401 };
        }

        [HttpPost]
        [Route("/registration")]
        public async Task<JsonResult> CreateUser(ModelConnectingUser dataUser)
        {
            if (!_checkDataUser.CheckLoginRegular(dataUser.Login))
                return new JsonResult(new
                {
                    errors = new
                    {
                        message = "INCORRECT_LOGIN"
                    }
                })
                { StatusCode = 401 };

            if (!_checkDataUser.CheckPassword(dataUser.Password))
                return new JsonResult(new
                {
                    errors = new
                    {
                        message = "INCORRECT_PASSWORD"
                    }
                })
                { StatusCode = 401 };

            if (!await AppDBContent.TestConnection())
                return new JsonResult(new
                {
                    error = "ERRORSERVER"
                })
                { StatusCode = 521 };

            var resultExist = _controlDb.ExistUser(dataUser.Login);
            if (resultExist)
                return new JsonResult(new
                {
                    errors = new
                    {
                        message = "EXISTING_USER"
                    }
                })
                { StatusCode = 401 };

            var resultAdd = _controlDb.AddUser(dataUser.Login, dataUser.Password);
            if (!resultAdd)
                return new JsonResult(new
                {
                    error = "ERRORSERVER"
                })
                { StatusCode = 521 };

            var idUser = _controlDb.GetUserId(dataUser.Login);
            if (idUser != -1)
                return new JsonResult(new
                {
                    user_id = idUser,
                    login = dataUser.Login
                })
                { StatusCode = 200 };

            return new JsonResult(new
            {
                error = "ERRORSERVER"
            })
            { StatusCode = 500 };
        }
    }
}