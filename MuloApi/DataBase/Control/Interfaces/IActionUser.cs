using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MuloApi.DataBase.Entities;

namespace MuloApi.DataBase.Control.Interfaces
{
    public interface IActionUser
    {
        Task<bool> AddUser(string login, string password);
        Task<bool> ExistUser(string login);
        Task<int> GetUserId(string login, string password = "");
        Task<string> GetDataUser(int idUser);
        Task<string> SaveCookieUser(int idUser, IHeaderDictionary headerRequest, bool updateCookie = false, ModelCookieUser dataCookieUser = null);
        Task<ModelCookieUser> GetDataCookieUser(string cookieUser);
        Task<bool> DeleteCookieUser(string cookie);
    }
}