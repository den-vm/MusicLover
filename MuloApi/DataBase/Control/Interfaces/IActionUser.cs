using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MuloApi.DataBase.Control.Interfaces
{
    public interface IActionUser
    {
        Task<bool> AddUser(string login, string password);
        Task<bool> ExistUser(string login);
        Task<int> GetUserId(string login);
        Task<string> SaveCookieUser(int idUser, IHeaderDictionary headerRequest);
        Task<bool?> CheckCookieUser(string cookieUser, int idUser, IHeaderDictionary headerRequest);
        Task<bool?> DeleteCookieUser(int idUser, string cookie);
    }
}