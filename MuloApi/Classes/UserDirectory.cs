using System;
using System.IO;
using MuloApi.Interfaces;

namespace MuloApi.Classes
{
    public class UserDirectory : IActionDirectory
    {
        private readonly string _defaultDirectoryUser = Directory.GetCurrentDirectory() + "/ExistingUsers/";

        public void CreateDirectoryUser(int idUser)
        {
            var dirInfo = new DirectoryInfo(_defaultDirectoryUser);
            if (!dirInfo.Exists) dirInfo.Create();
            dirInfo.CreateSubdirectory($"user_{idUser}");
        }

        public void DeleteDirectoryUser(int idUser)
        {
            var dirUser = new DirectoryInfo(_defaultDirectoryUser) + $"/user_{idUser}";
            Directory.Delete(dirUser);
        }
    }
}