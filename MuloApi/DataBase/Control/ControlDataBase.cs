using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MuloApi.DataBase.Control.Interfaces;
using MuloApi.DataBase.Entities;

namespace MuloApi.DataBase.Control
{
    public class ControlDataBase : IActionUser
    {
        public async Task<bool> AddUser(string login, string password)
        {
            try
            {
                using var db = new AppDBContent<NewUser>();
                var user = new NewUser {Login = login, Password = password};
                await db.Users.AddAsync(user);
                await db.SaveChangesAsync();
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
                using var db = new AppDBContent<ExistUser>();
                var result = await db.Users.FirstOrDefaultAsync(user => user.Login.Equals(login));
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
                using var db = new AppDBContent<ExistUser>();
                var result = await db.Users.FirstOrDefaultAsync(user => user.Login.Equals(login));
                if (result != null)
                    return result.Id;
            }
            catch (Exception e)
            {
                if (Startup.LoggerApp != null)
                    await Task.Run(() => Startup.LoggerApp.LogWarning(e.ToString()));
            }

            return -1;
        }
    }
}