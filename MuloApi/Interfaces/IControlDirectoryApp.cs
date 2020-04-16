using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

namespace MuloApi.Interfaces
{
    interface IControlDirectoryApp
    {
        Task<bool> IsCreatedDirectory(string directory, string userCatalog);
        Task<IEnumerable<string>> GetNameTraks(string directory);
        Task<bool> UploadFile(string directory, Stream inputStream);
        Task<IEnumerable<Stream>> GetStreamTraks(string directory);
    }
}
