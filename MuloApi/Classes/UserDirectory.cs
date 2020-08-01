using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MuloApi.DataBase.Control;
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
        private readonly string catalogMusic = "/";

        public async Task CreateDirectoryUser(int idUser, string catalog = "/")
        {
            try
            {
                var dirInfo = await _directoryApp.IsCreatedDirectory(_defaultDirectoryUser, $"user_{idUser}{catalog}");
                if (!dirInfo)
                    throw new DirectoryNotFoundException("Error creating a user folder");
                var isCreatedCatalog = await new ActionUserDataBase().Current.CreateCatalog(idUser, catalog);
                if (!isCreatedCatalog)
                    throw new DirectoryNotFoundException("Error saving the user's folder to the database");
            }
            catch (Exception e)
            {
                LoggerApp.Log.LogException(e);
            }
        }


        public async Task<ModelUserTracks[]> GetTracksUser(int idUser, int idCatalog)
        {
            try
            {
                var tracksUser = await new ActionUserDataBase().Current.GetTracksUser(idUser, idCatalog);
                if (tracksUser == null)
                    throw new Exception("Error in executing the request to output the track list");
                return tracksUser;
            }
            catch (Exception e)
            {
                LoggerApp.Log.LogException(e);
            }

            return null;
        }

        public async Task<FileResult> GetActiveTrackUser(int idUser, int idCatalog, int idTrack)
        {
            try
            {
                var pathCatalog = await new ActionUserDataBase().Current.GetPathCatalog(idUser, idCatalog);
                if (pathCatalog == null)
                    throw new Exception("Error in executing the request to output the track list");
                var fullPathTrack = $"{_defaultDirectoryUser}user_{idUser}{pathCatalog}{idTrack}.mp3";
                var trackStream = await _directoryApp.GetFile(fullPathTrack);
                return new FileContentResult(trackStream.ToArray(), "audio/mpeg");
            }
            catch (Exception e)
            {
                LoggerApp.Log.LogException(e);
            }

            return null;
        }

        public async Task DeleteDirectoryUser(int idUser, int idCatalog)
        {
            try
            {
                throw new Exception("Undefined method 'Delete directory'");
            }
            catch (Exception e)
            {
                LoggerApp.Log.LogException(e);
            }
        }

        public async Task<List<ModelUserTracks>> SavedTracksUser(int idUser, int idCatalog,
            IFormFileCollection tracksCollection)
        {
            try
            {
                var tracksUser = await new ActionUserDataBase().Current.GetTracksUser(idUser, idCatalog);
                if (tracksUser == null)
                    throw new Exception("Error in executing the request to output the track list");

                var newIdTrack = tracksUser.Length > 0 ? tracksUser.Select(track => track.Id).Max() + 1 : 1;

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
                    var splitFileName = removeTrackMp3.Split('-', '–');

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
                        Name = nameTrack,
                        DateLoad = DateTime.Now.ToString("O") // datetime format ISO 8601
                    });

                    await _directoryApp.UploadFile(
                        _defaultDirectoryUser + $"user_{idUser}{catalogMusic}{newIdTrack}.mp3",
                        newStreamFormFile);

                    newStreamFormFile.Close();

                    newIdTrack++;
                }

                await new ActionUserDataBase().Current.AddTrackLoaded(idUser, $"{catalogMusic}", uploadTrackList);

                return uploadTrackList;
            }
            catch (Exception e)
            {
                LoggerApp.Log.LogException(e);
            }

            return new List<ModelUserTracks>();
        }

        public async Task<string> DeleteTrackUser(int idUser, int idCatalog, int idTrack)
        {
            try
            {
                var pathCatalog = await new ActionUserDataBase().Current.GetPathCatalog(idUser, idCatalog);
                if (pathCatalog == null)
                    throw new Exception("Error in executing the request to output the track list");
                var fullPathTrack = $"{_defaultDirectoryUser}user_{idUser}{pathCatalog}{idTrack}.mp3";
                var responseAws = await _directoryApp.DeleteFile(fullPathTrack);
                var responseDataBase =
                    await new ActionUserDataBase().Current.DeleteTrackUser(idUser, idCatalog, idTrack);
                if (responseAws && responseDataBase)
                    return "success";
            }
            catch (Exception e)
            {
                LoggerApp.Log.LogException(e);
            }

            return "error";
        }

        public async Task<ModelUserTracks> ChangeDataTrack(int idUser, int idCatalog, int idTrack, string author,
            string name)
        {
            try
            {
                var pathCatalog = await new ActionUserDataBase().Current.GetPathCatalog(idUser, idCatalog);
                if (pathCatalog == null)
                    throw new Exception("Error in executing the request to output the track list");
                var fullPathTrack = $"{_defaultDirectoryUser}user_{idUser}{pathCatalog}{idTrack}.mp3";
                var responseAws = await _directoryApp.GetFile(fullPathTrack);

                var tempFile =
                    new AudioFile(new DataAudioFile(fullPathTrack, responseAws));
                var tagsAudioFile = MusicFile.Create(tempFile);
                tagsAudioFile.Tag.Performers = author.Split();
                tagsAudioFile.Tag.Title = name;
                tagsAudioFile.Save();
                var responseUpdateAws = await _directoryApp.UpdateFile(fullPathTrack, tempFile.ReadStream);
                var responseDataBase =
                    await new ActionUserDataBase().Current.ChangeTrackUser(idUser, idCatalog, idTrack, author, name);

                if (responseUpdateAws && responseDataBase)
                    return new ModelUserTracks
                    {
                        Id = idTrack,
                        Name = author + " - " + name
                    };
            }
            catch (Exception e)
            {
                LoggerApp.Log.LogException(e);
            }

            return new ModelUserTracks();
        }
    }
}