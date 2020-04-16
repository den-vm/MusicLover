using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MuloApi.Models;

namespace MuloApi.Interfaces
{
    internal interface IActionDirectory
    {
        void CreateDirectoryUser(int idUser);
        Task<ModelUserTracks[]> GetTracksUser(int idUser);
        Task<List<ModelUserTracks>> SavedRootTrackUser(int idUser, IFormFileCollection tracksCollection);
        Task<FileResult> GetActiveTrackUser(int idUser, int idTrack);
        void DeleteDirectoryUser(int idUser);
    }
}