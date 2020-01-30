using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MuloApi.Controllers;
using Xunit;

namespace XUnitTestMuloApi
{
    public class UnitTestActionsOnFilesController
    {
        [Fact]
        public async void UploadSoundTrack()
        {
            //Arrange
            var requestBodys = new List<Stream>
            {
                File.Open(Directory.GetCurrentDirectory() + @"\TestFiles\Disept & Zendi - New Balance (Edited).mp3",
                    FileMode.Open),
                null,
                File.Open(Directory.GetCurrentDirectory() + @"\TestFiles\Modestep - Exile Mastered LB (cut).mp3",
                    FileMode.Open)
            };


            var resultGetDataUploadTrack = new List<JsonResult>();
            var actionsOnFilesController = new ActionsOnFilesController
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };

            //Act
            for (var i = 0; i < 3; i++)
            {
                actionsOnFilesController.Request.Body = requestBodys[i];
                var result = await actionsOnFilesController.UploadSoundTrack(i);
                resultGetDataUploadTrack.Add(result);
            }

            // Assert
            foreach (var result in resultGetDataUploadTrack)
            {
                Assert.NotNull(result);
                if (result.Value.ToString().Contains("MuloApi.Models.ModelUserTracks"))
                {
                    Assert.Contains(@"{ tracks = MuloApi.Models.ModelUserTracks }",
                        result.Value.ToString());
                    continue;
                }

                result.AssertContains(ListResults.GetListResults().MethodUploadSoundTrack());
            }
        }
    }
}