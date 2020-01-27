using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MuloApi.Models;

namespace MuloApi.Controllers
{
    [ApiController]
    public class ActionsOnFilesController : ControllerBase
    {
        [HttpPost]
        [Route("/user/{idUser:min(0)}/soundtracks/upload")]
        public async Task<JsonResult> UploadSoundTrack(int idUser)
        {
            return new JsonResult(new
            {
                tracks = new ModelUserTracks
                {
                    Id = 5,
                    Name = "The Score - Rush"
                }
            });
        }
    }
}