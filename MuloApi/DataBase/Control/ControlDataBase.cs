using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MuloApi.DataBase.Control.Interfaces;
using MuloApi.DataBase.Entities;

namespace MuloApi.DataBase.Control
{
    public class ControlDataBase : IActionUser
    {
        private static ControlDataBase _control;

        public bool AddUser(string login, string password)
        {
            try
            {
                using var db = new AppDBContent();
                var user = new DBUser {Login = login, Password = password};
                db.Users.AddAsync(user);
                db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                if (Startup.LoggerApp != null)
                    Startup.LoggerApp.LogWarning(e.ToString());
            }

            return false;
        }

        public bool ExistUser(string login)
        {
            try
            {
                using var db = new AppDBContent();
                var result = db.Users.FirstOrDefaultAsync(user => user.Login.Equals(login)).Result;
                if (result != null) return true;
            }
            catch (Exception e)
            {
                if (Startup.LoggerApp != null)
                    Startup.LoggerApp.LogWarning(e.ToString());
            }

            return false;
        }

        public int GetUserId(string login)
        {
            using var db = new AppDBContent();
            var result = db.Users.FirstOrDefaultAsync(user => user.Login.Equals(login)).Result;
            if (result != null)
                return result.Id;
            return -1;
        }

        public static ControlDataBase Instance()
        {
            return _control ??= new ControlDataBase();
        }
    }
}