using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuloApi.Interfaces
{
    interface IActionDirectory
    {
        void CreateDirectoryUser(int idUser);
        void DeleteDirectoryUser(int idUser);
    }
}
