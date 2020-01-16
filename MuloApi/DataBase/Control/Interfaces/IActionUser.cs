using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuloApi.DataBase.Control.Interfaces
{
    interface IActionUser
    {
        bool AddUser(string login, string password);
        bool ExistUser(string login);
        int GetUserId(string login);
    }
}
