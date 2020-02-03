using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MuloApi.DataBase.Control.Interfaces
{
    public interface IActionUser
    {
        Task<bool> AddUser(string login, string password);
        Task<bool> ExistUser(string login);
        Task<int> GetUserId(string login);
        Task<string> SaveHashUser(int idUser, IHeaderDictionary headerUser);
        Task<bool> CheckUserSession(string cookieUser, int idUser, IHeaderDictionary headerUser);
    }
}