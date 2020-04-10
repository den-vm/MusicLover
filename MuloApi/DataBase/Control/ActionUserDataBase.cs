using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MuloApi.Classes;
using MuloApi.DataBase.Control.Interfaces;
using MuloApi.DataBase.Entities;

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
                if (Startup.LoggerApp != null)
                    await Task.Run(() => Startup.LoggerApp.LogError(e.ToString()));
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
                if (Startup.LoggerApp != null)
                    await Task.Run(() => Startup.LoggerApp.LogError(e.ToString()));
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
                if (Startup.LoggerApp != null)
                    await Task.Run(() => Startup.LoggerApp.LogError(e.ToString()));
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
                    select header.Value).ToArray().LastOrDefault()[0];
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
                if (Startup.LoggerApp != null)
                    await Task.Run(() => Startup.LoggerApp.LogError(e.ToString()));
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
                if (Startup.LoggerApp != null)
                    await Task.Run(() => Startup.LoggerApp.LogError(e.ToString()));
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
                if (Startup.LoggerApp != null)
                    await Task.Run(() => Startup.LoggerApp.LogError(e.ToString()));
            }

            return false;
        }

        public async Task<string> GetDataUser(int idUser)
        {
            try
            {
                var resultSearch =
                    await DataBase.Users.FirstOrDefaultAsync(user => user.Id.Equals(idUser));
                var jsonDataUser = JsonSerializer.Serialize(new ModelUser()
                {
                    Id = resultSearch.Id,
                    Login = resultSearch.Login
                });
                return jsonDataUser;
            }
            catch (Exception e)
            {
                if (Startup.LoggerApp != null)
                    await Task.Run(() => Startup.LoggerApp.LogError(e.ToString()));
            }

            return "";
        }
    }
}