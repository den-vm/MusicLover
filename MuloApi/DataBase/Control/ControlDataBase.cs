using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MuloApi.Classes;
using MuloApi.DataBase.Control.Interfaces;
using MuloApi.DataBase.Entities;

namespace MuloApi.DataBase.Control
{
    public class ControlDataBase : IActionUser
    {
        private readonly AppDBContent _db = AppDBContent.Current;
        public static ControlDataBase Current { get; } = new ControlDataBase();

        public async Task<bool> AddUser(string login, string password)
        {
            try
            {
                var newUser = new ModelUser {Login = login, Password = password};
                await _db.Users.AddAsync(newUser);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                if (Startup.LoggerApp != null)
                    await Task.Run(() => Startup.LoggerApp.LogWarning(e.ToString()));
            }

            return false;
        }

        public async Task<bool> ExistUser(string login)
        {
            try
            {
                var result = await _db.Users.FirstOrDefaultAsync(user => user.Login.Equals(login));
                if (result != null) return true;
            }
            catch (Exception e)
            {
                if (Startup.LoggerApp != null)
                    await Task.Run(() => Startup.LoggerApp.LogWarning(e.ToString()));
            }

            return false;
        }

        public async Task<int> GetUserId(string login)
        {
            try
            {
                var result = await _db.Users.FirstOrDefaultAsync(user => user.Login.Equals(login));
                if (result != null) return result.Id;
            }
            catch (Exception e)
            {
                if (Startup.LoggerApp != null)
                    await Task.Run(() => Startup.LoggerApp.LogWarning(e.ToString()));
            }

            return -1;
        }

        public async Task<string> SaveHashUser(int idUser, IHeaderDictionary headerUser)
        {
            var checkDataUser = new CheckDataUser();
            var agent = (from header in headerUser
                where header.Key.Equals("User-Agent")
                select header.Value).ToArray().LastOrDefault()[0];
            var hashUser = checkDataUser.GetHash(idUser, agent);

            var result =
                await _db.HashUsers.FirstOrDefaultAsync(hash =>
                    hash.IdUser.Equals(idUser) && hash.HashUser.Equals(hashUser));
            if (result != null)
                return result.HashUser;

            var newHashUser = new ModelHashUser {IdUser = idUser, HashUser = hashUser};
            await _db.HashUsers.AddAsync(newHashUser);
            await _db.SaveChangesAsync();

            return hashUser;
        }

        public async Task<bool> CheckUserSession(string cookieUser, int idUser, IHeaderDictionary headerUser)
        {
            var checkDataUser = new CheckDataUser();
            var agent = (from header in headerUser
                where header.Key.Equals("User-Agent")
                select header.Value).ToArray().LastOrDefault()[0];
            var hashUser = checkDataUser.GetHash(idUser, agent);
            var result =
                await _db.HashUsers.FirstOrDefaultAsync(hash =>
                    hash.IdUser.Equals(idUser) && hash.HashUser.Equals(hashUser));
            return result != null && result.HashUser.Equals(hashUser);
        }
    }
}