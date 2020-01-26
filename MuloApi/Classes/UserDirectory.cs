using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using MuloApi.Interfaces;
using MuloApi.Models;
using MusicFile = TagLib.File;

namespace MuloApi.Classes
{
    public class UserDirectory : IActionDirectory
    {
        private readonly string _defaultDirectoryUser = @"../ExistingUsers/";

        public void CreateDirectoryUser(int idUser)
        {
            var dirInfo = new DirectoryInfo(_defaultDirectoryUser);
            if (!dirInfo.Exists) dirInfo.Create();
            dirInfo.CreateSubdirectory($"user_{idUser}");
        }

        public ModelUserTracks[] GetRootTracksUser(int idUser)
        {
            try
            {
                string[] filters = {"*.mp3"};
                var mp3List = ExtensionDirectoryGetFiles.GetFiles(_defaultDirectoryUser + $"user_{idUser}", filters);
                var tracksUser = (from track in mp3List
                    select MusicFile.Create(track)
                    into tagsTrack
                    let idTrack = int.Parse(tagsTrack.Name.Split($"user_{idUser}")[1]
                        .Split(".")[0]
                        .Substring(1))
                    let nameTrack =
                        string.Concat(
                            tagsTrack.Tag.JoinedPerformers.Equals("") ? "None" : tagsTrack.Tag.JoinedPerformers, " - ",
                            tagsTrack.Tag.Title ?? "None")
                    select new ModelUserTracks {Id = idTrack, Name = nameTrack}).ToArray();
                return tracksUser;
            }
            catch (Exception e)
            {
                if (Startup.LoggerApp != null)
                    Startup.LoggerApp.LogWarning(e.ToString());
            }

            return null;
        }

        public void DeleteDirectoryUser(int idUser)
        {
            var dirUser = new DirectoryInfo(_defaultDirectoryUser) + $"user_{idUser}";
            Directory.Delete(dirUser);
        }
    }
}