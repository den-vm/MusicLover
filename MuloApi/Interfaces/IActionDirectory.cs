using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MuloApi.Models;

namespace MuloApi.Interfaces
{
    interface IActionDirectory
    {
        void CreateDirectoryUser(int idUser); 
        Task<ModelUserTracks[]> GetRootTracksUser(int idUser);
        Task<ModelUserTracks> SavedRootTrackUser(int idUser, Stream trackBinary);
        void DeleteDirectoryUser(int idUser);
    }
}
