using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MuloApi.Models;

namespace MuloApi.Interfaces
{
    interface IActionDirectory
    {
        void CreateDirectoryUser(int idUser); 
        Task<ModelUserTracks[]> GetRootTracksUser(int idUser);
        Task<List<ModelUserTracks>> SavedRootTrackUser(int idUser, IFormFileCollection tracksCollection);
        Task<FileResult> GetActiveTrackUser(int idUser, int idTrack);
        void DeleteDirectoryUser(int idUser);
    }
}
