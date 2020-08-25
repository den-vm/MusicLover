using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MuloApi.Classes;
using MuloApi.Interfaces;
using Newtonsoft.Json;

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
                var errorMessage = JsonConvert.SerializeObject(new
                {
                    errors = new[]
                    {
                        new
                        {
                            message = "Track not found",
                            name = "ERROR_SERVER"
                        }
                    }
                });
                await Clients.Caller.Error(errorMessage);
                return;
            }

            for (var i = 0; i < trackStream.ToArray().Length; i += 6000)
            {
                var trackByte = trackStream.ToArray().Skip(i).Take(6000);
                await Clients.Caller.PartMusicTrack(trackByte.ToArray());
            }

            var message = JsonConvert.SerializeObject(new
            {
                message = "Complete"
            });
            await Clients.Caller.Message(message);
        }
    }
}