using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MuloApi.Classes;
using MuloApi.DataBase.Control;
using MuloApi.DataBase.Control.Interfaces;
using MuloApi.Interfaces;

namespace MuloApi.Controllers
{
    [ApiController]
    public class ActionsOnFilesController : ControllerBase
    {
        public IActionUser ControlDataBase = new ActionUserDataBase().Current;

        [HttpPost]
        [Route("/user/{idUser:min(0)}/soundtracks/upload")]
        public async Task<ActionResult> UploadSoundTrack(int idUser, IFormFileCollection tracks)
        {
            IActionDirectory userDirectory = new UserDirectory();
            var downloadedTrack = await userDirectory.SavedRootTrackUser(idUser, tracks);
            if (downloadedTrack == null || downloadedTrack.Count == 0)
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