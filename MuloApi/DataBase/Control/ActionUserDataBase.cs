using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MuloApi.Classes;
using MuloApi.DataBase.Control.Interfaces;
using MuloApi.DataBase.Entities;
using MuloApi.Models;

namespace MuloApi.DataBase.Control
{
    public class ActionUserDataBase : IActionUser
    {
        private static ActionUserDataBase _instance;

        public AppDbContent DataBase = new AppDbContent().Current;

        public ActionUserDataBase Current
        {
            get { return _instance ??= new ActionUserDataBase(); }
        }

        public async Task<bool> AddUser(string login, string password)
        {
            try
            {
                var hashPassword = CheckDataUser.Md5Hash(CheckDataUser.Md5Hash(password));
                var newUser = new ModelUser {Login = login, Password = hashPassword};
                await DataBase.Users.AddAsync(newUser);
                await DataBase.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                LoggerApp.Log.LogException(e);
            }

            return false;
        }

        public async Task<bool> ExistUser(string login)
        {
            try
            {
                var result = await DataBase.Users.FirstOrDefaultAsync(user => user.Login.Equals(login));
                if (result != null) return true;
            }
            catch (Exception e)
            {
                LoggerApp.Log.LogException(e);
            }

            return false;
        }

        public async Task<int> GetUserId(string login, string password = "")
        {
            try
            {
                var result = await DataBase.Users.FirstOrDefaultAsync(user => user.Login.Equals(login));
                if (result == null)
                    return -1;
                var passHash = CheckDataUser.Md5Hash(CheckDataUser.Md5Hash(password));
                if (result.Password.Equals(passHash) || password.Equals("")) return result.Id;
            }
            catch (Exception e)
            {
                LoggerApp.Log.LogException(e);
            }

            return -1;
        }

        public async Task<string> SaveCookieUser(int idUser, IHeaderDictionary headerRequest, bool updateCookie = false,
            ModelCookieUser dataCookieUser = null)
        {
            try
            {
                var checkDataUser = new CheckDataUser();
                var agent = (from header in headerRequest
                    where header.Key.Equals("User-Agent")
                    select header.Value).ToArray().LastOrDefault();
                var hashUser = checkDataUser.GetHash(idUser, agent);

                if (!updateCookie) // create new cookie
                {
                    var newHashUser = new ModelCookieUser
                    {
                        IdUser = idUser,
                        Cookie = hashUser,
                        Start = DateTime.Now,
                        End = DateTime.Now.AddHours(24)
                    };
                    await DataBase.HashUsers.AddAsync(newHashUser);
                    await DataBase.SaveChangesAsync();
                    return hashUser;
                }

                //update expired cookies
                dataCookieUser.Cookie = hashUser;
                dataCookieUser.Start = DateTime.Now;
                dataCookieUser.End = DateTime.Now.AddHours(24);
                DataBase.HashUsers.Update(dataCookieUser);
                await DataBase.SaveChangesAsync();
                return hashUser;
            }
            catch (Exception e)
            {
                LoggerApp.Log.LogException(e);
            }

            return null;
        }

        public async Task<ModelCookieUser> GetDataCookieUser(string cookieUser)
        {
            try
            {
                var result =
                    await DataBase.HashUsers.FirstOrDefaultAsync(hash =>
                        hash.Cookie.Equals(cookieUser));
                if (result != null)
                    return result;
            }
            catch (Exception e)
            {
                LoggerApp.Log.LogException(e);
            }

            return null;
        }

        public async Task<bool> DeleteCookieUser(string cookie)
        {
            try
            {
                var resultSearch =
                    await DataBase.HashUsers.FirstOrDefaultAsync(hash => hash.Cookie.Equals(cookie));
                DataBase.HashUsers.Remove(resultSearch);
                await DataBase.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                LoggerApp.Log.LogException(e);
            }

            return false;
        }

        public async Task<bool> CreateCatalog(int idUser, string path)
        {
            try
            {
                var newCatalog = new ModelCatalog {IdUser = idUser, Path = path};
                await DataBase.Catalogs.AddAsync(newCatalog);
                await DataBase.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                LoggerApp.Log.LogException(e);
            }

            return false;
        }

        public async Task AddTrackLoaded<T>(int idUser, string path, T tracks)
        {
            try
            {
                if (tracks is List<ModelUserTracks> listTracks)
                {
                    var catalog =
                        await DataBase.Catalogs.FirstOrDefaultAsync(e =>
                            e.IdUser.Equals(idUser) && e.Path.Equals(path));
                    var userTracks = listTracks.Select(track => new ModelMusicTracks
                        {
                            IdUser = catalog.IdUser,
                            IdCatalog = catalog.Id,
                            IdTrack = track.Id,
                            NameTrack = track.Name,
                            DateLoadTrack = DateTime.ParseExact(track.DateLoad, "O", CultureInfo.CurrentCulture)
                        })
                        .ToList();
                    await DataBase.MusicTracks.AddRangeAsync(userTracks);
                    await DataBase.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                LoggerApp.Log.LogException(e);
            }
        }

        public async Task<string> GetDataUser(int idUser)
        {
            try
            {
                var resultSearch =
                    await DataBase.Users.FirstOrDefaultAsync(user => user.Id.Equals(idUser));
                var jsonDataUser = JsonSerializer.Serialize(new ModelUser
                {
                    Id = resultSearch.Id,
                    Login = resultSearch.Login
                });
                return jsonDataUser;
            }
            catch (Exception e)
            {
                LoggerApp.Log.LogException(e);
            }

            return "";
        }

        public async Task<ModelUserTracks[]> GetTracksUser(int idUser, int idCatalog)
        {
            try
            {
                if (idCatalog == -1)
                    idCatalog = (await DataBase.Catalogs.FirstOrDefaultAsync(catalog => catalog.IdUser.Equals(idUser)))
                        .Id;

                var result = await Task.Run(() =>
                {
                    return DataBase.MusicTracks.Where(track =>
                        track.IdUser.Equals(idUser) && track.IdCatalog.Equals(idCatalog)).ToList();
                });
                var listTrack = result.Select(track => new ModelUserTracks
                {
                    Id = track.IdTrack,
                    Name = track.NameTrack,
                    DateLoad = track.DateLoadTrack.ToString("O")
                }).ToArray();

                return listTrack;
            }
            catch (Exception e)
            {
                LoggerApp.Log.LogException(e);
            }

            return null;
        }

        public async Task<string> GetPathCatalog(int idUser, int idCatalog)
        {
            try
            {
                return idCatalog == -1
                    ? (await DataBase.Catalogs.FirstOrDefaultAsync(catalog => catalog.IdUser.Equals(idUser))).Path
                    : (await DataBase.Catalogs.FirstOrDefaultAsync(catalog =>
                        catalog.IdUser.Equals(idUser) && catalog.Id.Equals(idCatalog))).Path;
            }
            catch (Exception e)
            {
                LoggerApp.Log.LogException(e);
            }

            return null;
        }

        public async Task<bool> DeleteTrackUser(int idUser, int idCatalog, int idTrack)
        {
            try
            {
                idCatalog = idCatalog == -1 ? 1 : idCatalog;
                var dataMusicTrack = DataBase.
                    MusicTracks.
                    FirstOrDefault(track => track.IdUser.Equals(idUser) &&
                                            track.IdCatalog.Equals(idCatalog) &&
                                            track.IdTrack.Equals(idTrack));
                // ReSharper disable once AssignNullToNotNullAttribute
                DataBase.MusicTracks.Remove(dataMusicTrack);
                await DataBase.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                LoggerApp.Log.LogException(e);
                return false;
            }
        }
    }
}