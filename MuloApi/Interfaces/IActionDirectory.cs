using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MuloApi.Models;

namespace MuloApi.Interfaces
{
    internal interface IActionDirectory
    {
        Task CreateDirectoryUser(int idUser, string catalog = "/");
        Task<ModelUserTracks[]> GetTracksUser(int idUser, int idCatalog);
        Task<List<ModelUserTracks>> SavedTracksUser(int idUser, int idCatalog, IFormFileCollection tracksCollection);
        Task<FileResult> GetActiveTrackUser(int idUser, int idCatalog, int idTrack);
        Task DeleteDirectoryUser(int idUser, int idCatalog);
        Task<string> DeleteTrackUser(int idUser, int idCatalog, int idTrack);
        Task<ModelUserTracks> ChangeDataTrack(int idUser, int idCatalog, int idTrack, string author, string name);
    }
}