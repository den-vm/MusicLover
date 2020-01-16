using System;
using System.Linq;
using MuloApi.DataBase.Control.Interfaces;
using MuloApi.DataBase.Entities;

namespace MuloApi.DataBase.Control
{
    public class ControlDataBase : IActionAdd
    {
        private static ControlDataBase _control;

        public string AddUser(string login, string password)
        {
            try
            {
                using (var db = new ConnectDataBase())
                {
                    var user1 = new DBUser {Login = "Tom", Password = "sdfdgtdrh"};
                    db.Users.Add(user1);
                    db.SaveChanges();
                    var users = db.Users.ToList();
                    foreach (var u in users) Console.WriteLine($"{u.Id}.{u.Login} - {u.Password}");
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }

            return "added";
        }

        public static ControlDataBase Instance()
        {
            return _control ??= new ControlDataBase();
        }
    }
}