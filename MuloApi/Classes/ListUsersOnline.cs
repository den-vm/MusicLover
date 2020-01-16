using System.Collections.Generic;
using MuloApi.DataBase.Entities;
using MuloApi.Interfaces;

namespace MuloApi.Classes
{

    public class ListUsersOnline : IControlUsersOnline<ModelOnlineUser, List<ModelOnlineUser>>
    {
        private static ListUsersOnline _instUsersLogIn;
        private readonly List<ModelOnlineUser> _logIn = new List<ModelOnlineUser>();

        public void Add(ModelOnlineUser user)
        {
            _logIn.Add(user);
        }

        public List<ModelOnlineUser> Get()
        {
            return _logIn;
        }

        public static ListUsersOnline Instance()
        {
            return _instUsersLogIn ??= new ListUsersOnline();
        }
    }
}