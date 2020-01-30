using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace XUnitTestMuloApi
{
    interface IQueryResults
    {
        JsonResult[] MethodConnectUser();
        JsonResult[] MethodCreateUser();
        JsonResult[] MethodGetSoundTracksUser();
        JsonResult[] MethodUploadSoundTrack();
    }
}
