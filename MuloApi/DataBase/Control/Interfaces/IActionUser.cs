using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MuloApi.DataBase.Entities;
using MuloApi.Models;

namespace MuloApi.DataBase.Control.Interfaces
{
    public interface IActionUser
    {
        Task<bool> AddUser(string login, string password);
        Task<bool> ExistUser(string login);
        Task<int> GetUserId(string login, string password = "");
        Task<string> GetDataUser(int idUser);
        Task<string> SaveCookieUser(int idUser, IHeaderDictionary headerRequest, bool updateCookie = false, ModelCookieUser dataCookieUser = null);
        Task<ModelCookieUser> GetDataCookieUser(string cookieUser);
        Task<bool> DeleteCookieUser(string cookie);
        Task<bool> CreateCatalog(int idUser, string path);
        Task AddTrackLoaded<T>(int idUser, string path, T tracks);
        Task<ModelUserTracks[]> GetTracksUser(int idUser, int idCatalog);
        Task<string> GetPathCatalog(int idUser, int idCatalog);
        Task<bool> DeleteTrackUser(int idUser, int idCatalog, int idTrack);
        Task<bool> ChangeTrackUser(int idUser, int idCatalog, int idTrack, string author, string name);
    }
}