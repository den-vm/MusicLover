using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
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
                using var db = new ConnectDataBase();
                var user = new DBUser { Login = login, Password = password };
                db.Users.Add(user);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Startup.LoggerApp.LogWarning(e.ToString());
            }

            return true;
        }

        public bool ExistUser(string login)
        {
            try
            {
                using var db = new ConnectDataBase();
                var result = db.Users.FirstOrDefault(user => user.Login.Equals(login));
                if (result != null) return true;
            }
            catch (Exception e)
            {
                Startup.LoggerApp.LogWarning(e.ToString());
            }

            return false;
        }

        public int GetUserId(string login)
        {
            using var db = new ConnectDataBase();
            var result = db.Users.Where(user => user.Login.Equals(login)).Select(data => data.Id).FirstOrDefault();
            return result;
        }

        public static ControlDataBase Instance()
        {
            return _control ??= new ControlDataBase();
        }
    }
}