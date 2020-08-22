using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MuloApi.Classes;
using MuloApi.Interfaces;

namespace MuloApi.Hubs
{
    public class ClientHub : Hub<IClientHub>
    {
        public async Task PlaySoundTrack(int idUser, int idTrack)
        {
            var trackStream = new MemoryStream();

            IActionDirectory userDirectory = new UserDirectory();
            var idCatalog = -1;
            if (await userDirectory.GetActiveTrackUser(idUser, idCatalog, idTrack) is FileContentResult
                fileContentResultDashboard)
                trackStream.Write(fileContentResultDashboard.FileContents);

            if (trackStream.Length == 0)
            {
                await Clients.Caller.ErrorResponce("ErrorResponce", "ERRORSERVER");
                return;
            }

            var fileResultPart = new FileContentResult(trackStream.ToArray(), "audio/mpeg");
            trackStream.Close();

            await Clients.Caller.GetMusicTrack("MusicTrack", fileResultPart);
        }
    }
}