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
        Task<bool> UploadFile(string directory, Stream inputStream);
        Task<MemoryStream> GetFile(string directory);
        Task<bool> DeleteFile(string directory);
        Task<bool> UpdateFile(string directory, Stream fileStream);
    }
}
