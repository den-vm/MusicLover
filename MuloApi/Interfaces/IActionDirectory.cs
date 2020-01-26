using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MuloApi.Models;

namespace MuloApi.Interfaces
{
    interface IActionDirectory
    {
        void CreateDirectoryUser(int idUser); 
        ModelUserTracks[] GetRootTracksUser(int idUser);
        void DeleteDirectoryUser(int idUser);
    }
}
