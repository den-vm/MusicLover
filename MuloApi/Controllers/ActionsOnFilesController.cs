using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using MuloApi.Classes;
using MuloApi.Interfaces;
using MuloApi.Models;
using TagLib.Mpeg;

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
                    { StatusCode = 500 };
            return new JsonResult(new
            {
                tracks = downloadedTrack
            });
        }
    }
}