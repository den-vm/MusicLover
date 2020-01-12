using System.Collections.Generic;
using MuloApi.Interfaces;

namespace MuloApi.Classes
{
    public class UsersLogIn : IControlUser<User, List<User>>
    {
        private static UsersLogIn _instUsersLogIn;
        private readonly List<User> _logIn = new List<User>();

        public void SetUser(User user)
        {
            _logIn.Add(user);
        }

        public List<User> GetUsers()
        {
            return _logIn;
        }

        public static UsersLogIn Instance()
        {
            return _instUsersLogIn ??= new UsersLogIn();
        }
    }
}