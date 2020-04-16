using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MuloApi.Interfaces;
using MuloApi.Models;
using MusicFile = TagLib.File;

namespace MuloApi.Classes
{
    public class UserDirectory : IActionDirectory
    {
        private readonly string _defaultDirectoryUser = @"ExistingUsers/";
        private readonly IControlDirectoryApp _directoryApp = AmazonWebServiceS3.Current;
        private readonly string[] _filters = {"*.mp3"};

        public async void CreateDirectoryUser(int idUser)
        {
            try
            {
                var dirInfo = await _directoryApp.IsCreatedDirectory(_defaultDirectoryUser, $"user_{idUser}/");
                if (!dirInfo)
                    throw new DirectoryNotFoundException("Error creating or reading a user folder!");
            }
            catch (Exception e)
            {
                LoggerApp.Log.LogException(e);
            }
        }


        ///Рефакторинг
        public async Task<ModelUserTracks[]> GetTracksUser(int idUser)
        {
            try
            {
                var traksList =
                    await _directoryApp.GetStreamTraks(_defaultDirectoryUser +
                                                       $"user_{idUser}/");
                var tempFile = traksList.Select(e => new AudioFile(new DataAudioFile("tempFile.mp3", e)));

                var tracksUser = await Task.Run(() => (from track in tempFile
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
                LoggerApp.Log.LogException(e);
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
                LoggerApp.Log.LogException(e);
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
                LoggerApp.Log.LogException(e);
            }
        }

        ///Рефакторинг
        public async Task<List<ModelUserTracks>> SavedRootTrackUser(int idUser, IFormFileCollection tracksCollection)
        {
            try
            {
                var traksList =
                    await _directoryApp.GetNameTraks(_defaultDirectoryUser +
                                                     $"user_{idUser}/");

                var arrayIdTraks = await Task.Run(() => (from track in traksList
                    select track
                    into tagsTrack
                    let idTrack = int.Parse(tagsTrack.Split($"ExistingUsers/user_{idUser}")[1]
                        .Split(".")[0]
                        .Substring(1))
                    select idTrack).ToArray());

                var newIdTrack = arrayIdTraks.Length > 0 ? arrayIdTraks.Max() + 1 : 0;
                var uploadTrackList = new List<ModelUserTracks>();

                foreach (var track in tracksCollection)
                {
                    if (!(track.FileName.Contains(".mp3") && track.Length != 0))
                        continue;

                    var newStreamFormFile = new MemoryStream();
                    track.OpenReadStream().CopyTo(newStreamFormFile);

                    var tempFile =
                        new AudioFile(new DataAudioFile(_defaultDirectoryUser + $"user_{idUser}/{newIdTrack}.mp3",
                            newStreamFormFile));
                    var tagsAudioFile = MusicFile.Create(tempFile);

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

                    if (tagsAudioFile.Tag.JoinedPerformers.Equals(""))
                        tagsAudioFile.Tag.Performers = newTagTrack.Performance.Equals("")
                            ? new[] {"Неизвестный исполнитель"}
                            : new[] {newTagTrack.Performance};

                    if (tagsAudioFile.Tag.Title == null)
                        tagsAudioFile.Tag.Title = newTagTrack.Title.Equals("")
                            ? track.FileName.Remove(track.FileName.Length - 4)
                            : newTagTrack.Title;

                    tagsAudioFile.Save();

                    var nameTrack =
                        string.Concat(tagsAudioFile.Tag.JoinedPerformers, " - ", tagsAudioFile.Tag.Title);

                    uploadTrackList.Add(new ModelUserTracks
                    {
                        Id = newIdTrack,
                        Name = nameTrack
                    });

                    await _directoryApp.UploadFile(_defaultDirectoryUser + $"user_{idUser}/{newIdTrack}.mp3",
                        newStreamFormFile);

                    newStreamFormFile.Close();

                    newIdTrack++;
                }

                return uploadTrackList;
            }
            catch (Exception e)
            {
                LoggerApp.Log.LogException(e);
            }

            return new List<ModelUserTracks>();
        }
    }
}