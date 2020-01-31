using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MuloApi.Classes;
using MuloApi.Interfaces;
using Org.BouncyCastle.Crypto.Tls;
using Org.BouncyCastle.Ocsp;

namespace MuloApi.Controllers
{
    [ApiController]
    public class ActionsOnFilesController : ControllerBase
    {
        [HttpPost]
        [Route("/user/{idUser:min(0)}/soundtracks/upload")]
        public async Task<JsonResult> UploadSoundTrack(int idUser)
        {
            var soundtrackBinary = Request.Body;
            IActionDirectory userDirectory = new UserDirectory();
            var downloadedTrack = await userDirectory.SavedRootTrackUser(idUser, soundtrackBinary);
            if (downloadedTrack == null)
                return new JsonResult(new
                    {
                        error = "ERRORSERVER"
                    })
                    {StatusCode = 500};
            return new JsonResult(new
            {
                tracks = downloadedTrack
            });
        }

        [HttpGet]
        [Route("/user/{idUser:min(0)}/soundtracks/{idTrack:min(0)}.mp3")]
        public async Task<ActionResult> PlaySoundTrack(int idUser, int idTrack)
        {
            IActionDirectory userDirectory = new UserDirectory();
            var trackBinary = await userDirectory.GetActiveTrackUser(idUser, idTrack);
            if (trackBinary == null)
                return new JsonResult(new
                    {
                        error = "ERRORSERVER"
                    })
                    {StatusCode = 500};

            return trackBinary;
        }
    }
}