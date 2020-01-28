using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MuloApi.Interfaces;
using MuloApi.Models;
using MusicFile = TagLib.File;

namespace MuloApi.Classes
{
    public class UserDirectory : IActionDirectory
    {
        private readonly string _defaultDirectoryUser = @"../ExistingUsers/";
        private readonly string[] _filters = {"*.mp3"};

        public async void CreateDirectoryUser(int idUser)
        {
            try
            {
                var dirInfo = new DirectoryInfo(_defaultDirectoryUser);
                if (!dirInfo.Exists) dirInfo.Create();
                dirInfo.CreateSubdirectory($"user_{idUser}");
            }
            catch (Exception e)
            {
                if (Startup.LoggerApp != null)
                    await Task.Run(() => Startup.LoggerApp.LogWarning(e.ToString()));
            }
        }

        public async Task<ModelUserTracks[]> GetRootTracksUser(int idUser)
        {
            try
            {
                var mp3List = await Task.Run(() =>
                    ExtensionDirectoryGetFiles.GetFiles(_defaultDirectoryUser + $"user_{idUser}", _filters));
                var tracksUser = await Task.Run(() => (from track in mp3List
                    select MusicFile.Create(track)
                    into tagsTrack
                    let idTrack = int.Parse(tagsTrack.Name.Split($"user_{idUser}")[1]
                        .Split(".")[0]
                        .Substring(1))
                    let nameTrack =
                        string.Concat(
                            tagsTrack.Tag.JoinedPerformers.Equals("") ? "None" : tagsTrack.Tag.JoinedPerformers, " - ",
                            tagsTrack.Tag.Title ?? "None")
                    select new ModelUserTracks {Id = idTrack, Name = nameTrack}).ToArray());
                return tracksUser;
            }
            catch (Exception e)
            {
                if (Startup.LoggerApp != null)
                    await Task.Run(() => Startup.LoggerApp.LogWarning(e.ToString()));
            }

            return null;
        }

        public async void DeleteDirectoryUser(int idUser)
        {
            try
            {
                var dirUser = new DirectoryInfo(_defaultDirectoryUser) + $"user_{idUser}";
                Directory.Delete(dirUser);
            }
            catch (Exception e)
            {
                if (Startup.LoggerApp != null)
                    await Task.Run(() => Startup.LoggerApp.LogWarning(e.ToString()));
            }
        }

        public async Task<ModelUserTracks> SavedRootTrackUser(int idUser, Stream trackBinary)
        {
            try
            {
                var mp3List = await Task.Run(() =>
                    ExtensionDirectoryGetFiles.GetFiles(_defaultDirectoryUser + $"user_{idUser}", _filters));

                var mp3ListId = await Task.Run(() => (from track in mp3List
                    select MusicFile.Create(track)
                    into tagsTrack
                    let idTrack = int.Parse(tagsTrack.Name.Split($"user_{idUser}")[1]
                        .Split(".")[0]
                        .Substring(1))
                    select idTrack).ToArray());

                var mp3NewId = mp3ListId.Length > 0 ? mp3ListId.Max() + 1 : 0;

                await using (var fileStream =
                    new FileStream(_defaultDirectoryUser + $"user_{idUser}" + $"/{mp3NewId}.mp3", FileMode.Create))
                {
                    await trackBinary.CopyToAsync(fileStream);
                }

                var newTrack = await Task.Run(() =>
                    MusicFile.Create(_defaultDirectoryUser + $"user_{idUser}" + $"/{mp3NewId}.mp3"));
                var nameTrack =
                    string.Concat(
                        newTrack.Tag.JoinedPerformers.Equals("") ? "None" : newTrack.Tag.JoinedPerformers, " - ",
                        newTrack.Tag.Title ?? "None");

                return new ModelUserTracks
                {
                    Id = mp3NewId,
                    Name = nameTrack
                };
            }
            catch (Exception e)
            {
                if (Startup.LoggerApp != null)
                    await Task.Run(() => Startup.LoggerApp.LogWarning(e.ToString()));
            }

            return null;
        }
    }
}