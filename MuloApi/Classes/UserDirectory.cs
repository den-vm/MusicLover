using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                var mp3List =
                    await ExtensionDirectoryGetFiles.GetFiles(_defaultDirectoryUser + $"user_{idUser}", _filters);
                var tracksUser = await Task.Run(() => (from track in mp3List
                    select MusicFile.Create(track)
                    into tagsTrack
                    let idTrack = int.Parse(tagsTrack.Name.Split($"user_{idUser}")[1]
                        .Split(".")[0]
                        .Substring(1))
                    let nameTrack =
                        string.Concat(tagsTrack.Tag.JoinedPerformers, " - ", tagsTrack.Tag.Title)
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

        public async Task<FileResult> GetActiveTrackUser(int idUser, int idTrack)
        {
            try
            {
                var trackBytes =
                    await File.ReadAllBytesAsync(_defaultDirectoryUser + $"user_{idUser}" + $"/{idTrack}.mp3");
                return new FileContentResult(trackBytes, "audio/mpeg");
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

        public async Task<List<ModelUserTracks>> SavedRootTrackUser(int idUser, IFormFileCollection tracksCollection)
        {
            try
            {
                var mp3List =
                    await ExtensionDirectoryGetFiles.GetFiles(_defaultDirectoryUser + $"user_{idUser}", _filters);

                var mp3ListId = await Task.Run(() => (from track in mp3List
                    select MusicFile.Create(track)
                    into tagsTrack
                    let idTrack = int.Parse(tagsTrack.Name.Split($"user_{idUser}")[1]
                        .Split(".")[0]
                        .Substring(1))
                    select idTrack).ToArray());

                var mp3NewId = mp3ListId.Length > 0 ? mp3ListId.Max() + 1 : 0;

                var tracksSaved = new List<ModelUserTracks>();

                foreach (var track in tracksCollection)
                {
                    if (Startup.LoggerApp != null)
                        await Task.Run(() => Startup.LoggerApp.LogWarning($"Type track: {track.ContentType} Size track: {track.Length}"));
                    if (!(track.FileName.Contains(".mp3") && track.Length != 0))
                        continue;
                    await Task.Run(() =>
                    {
                        using (var fileStream = new FileStream(
                            _defaultDirectoryUser + $"user_{idUser}" + $"/{mp3NewId}.mp3", FileMode.Create))
                        {
                            track.CopyTo(fileStream);
                        }

                        var tagTrackSaved =
                            MusicFile.Create(_defaultDirectoryUser + $"user_{idUser}" + $"/{mp3NewId}.mp3");

                        var removeTrackMp3 = track.FileName.Remove(track.FileName.Length - 4);
                        var splitFileName = removeTrackMp3.Split("-");

                        string newPerformance = "", newTitle = "";

                        if (splitFileName.Length == 2) // Title from track.FileName
                        {
                            newPerformance = splitFileName[0]?.Trim(' ') ?? "";
                            newTitle = splitFileName[1]?.Trim(' ') ?? "";
                        }

                        var newTagTrack = new
                        {
                            Performance = newPerformance,
                            Title = newTitle
                        }; // Tags track from track.FileName

                        if (tagTrackSaved.Tag.JoinedPerformers.Equals(""))
                            tagTrackSaved.Tag.Performers = newTagTrack.Performance.Equals("")
                                ? new[] {"Неизвестный исполнитель"}
                                : new[] {newTagTrack.Performance};

                        if (tagTrackSaved.Tag.Title == null)
                            tagTrackSaved.Tag.Title = newTagTrack.Title.Equals("")
                                ? track.FileName.Remove(track.FileName.Length - 4)
                                : newTagTrack.Title;

                        tagTrackSaved.Save();

                        var nameTracks =
                            string.Concat(tagTrackSaved.Tag.JoinedPerformers, " - ", tagTrackSaved.Tag.Title);

                        tracksSaved.Add(new ModelUserTracks
                        {
                            Id = mp3NewId,
                            Name = nameTracks
                        });

                        mp3NewId++;
                    });
                }

                return tracksSaved;
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