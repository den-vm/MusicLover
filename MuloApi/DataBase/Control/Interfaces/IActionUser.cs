using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuloApi.DataBase.Control.Interfaces
{
    internal interface IActionUser
    {
        Task<bool> AddUser(string login, string password);
        Task<bool> ExistUser(string login);
        Task<int> GetUserId(string login);
    }
}
