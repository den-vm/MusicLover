using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;

namespace MuloApi.Interfaces
{
    interface IAmazonWebServiceS3
    {
        bool GetDirectoryInfo(string directory);
    }
}
