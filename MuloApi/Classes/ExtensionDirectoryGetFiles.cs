using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MuloApi.Classes
{
    public class ExtensionDirectoryGetFiles
    {
        public static async Task<IEnumerable<string>> GetFiles(string path,
            string[] searchPatterns,
            SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            try
            {
                var pathSounds = searchPatterns.AsParallel()
                    .SelectMany(searchPattern =>
                        Directory.EnumerateFiles(path, searchPattern, searchOption));
                return pathSounds.ToArray();
            }
            catch (Exception e)
            {
                if (Startup.LoggerApp != null)
                    await Task.Run(() => Startup.LoggerApp.LogError(e.ToString()));
                await AmazonWebServiceS3.Current.UploadLogAsync(TypesMessageLog.Error, e.ToString());
                throw;
            }
        }
    }
}