using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using MuloApi.Interfaces;
using Newtonsoft.Json.Linq;

namespace MuloApi.Classes
{
    internal class AmazonWebServiceS3 : IControlDirectoryApp, IControlSaveLogs
    {
        private static AmazonWebServiceS3 _instance;
        private readonly string _bucketName = "musiclover";

        private readonly IAmazonS3 _clientAws;
        public AmazonWebServiceS3()
        {
            var settingsFile = File.ReadAllText(@"AWSConnect.json");
            var awsAccessKeyId = (string)JObject.Parse(settingsFile)["awsAccessKeyId"];
            var awsSecretAccessKey = (string)JObject.Parse(settingsFile)["awsSecretAccessKey"];
            _clientAws = new AmazonS3Client(awsAccessKeyId,
                awsSecretAccessKey,
                RegionEndpoint.EUNorth1);
        }
        public static AmazonWebServiceS3 Current
        {
            get { return _instance ??= new AmazonWebServiceS3(); }
        }

        public async Task<MemoryStream> GetFile(string directory)
        {
            var response = await _clientAws.GetObjectAsync(_bucketName, directory);
            var newStreamFormFile = new MemoryStream();
            await response.ResponseStream.CopyToAsync(newStreamFormFile);
            return newStreamFormFile;
        }

        public async Task<bool> DeleteFile(string directory)
        {
            try
            {
                await _clientAws.DeleteObjectAsync(_bucketName, directory);
                return true;
            }
            catch (Exception e)
            {
                LoggerApp.Log.LogException(e);
                return false;
            }
        }

        public async Task<bool> IsCreatedDirectory(string directory, string userCatalog)
        {
            try
            {
                var response = await _clientAws.ListObjectsAsync(new ListObjectsRequest
                {
                    BucketName = _bucketName,
                    Prefix = directory + userCatalog
                });
                if (response.S3Objects.Count != 0)
                    return true;
                var request = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = directory + userCatalog
                };

                await _clientAws.PutObjectAsync(request);
            }
            catch (Exception e)
            {
                LoggerApp.Log.LogException(e);
                return false;
            }

            return true;
        }

        public async Task<bool> UpdateFile(string directory, Stream fileStream)
        {
            try
            {
                var request = new PutObjectRequest
                {
                    InputStream = fileStream,
                    BucketName = _bucketName,
                    Key = directory
                };
                await _clientAws.PutObjectAsync(request);
                return true;
            }
            catch (Exception e)
            {
                LoggerApp.Log.LogException(e);
                return false;
            }
        }

        public async Task<bool> UploadFile(string directory, Stream inputStream)
        {
            try
            {
                var request = new PutObjectRequest
                {
                    InputStream = inputStream,
                    BucketName = _bucketName,
                    Key = directory
                };
                await _clientAws.PutObjectAsync(request);
            }
            catch (Exception e)
            {
                LoggerApp.Log.LogException(e);
                return false;
            }

            return true;
        }

        public async Task UploadLogAsync(TypesMessageLog typeMessage, string message)
        {
            var filename = typeMessage + "_" +
                           DateTime.Now.ToString("yyyyMMdd hh:mm:ss tt", CultureInfo.InvariantCulture) + ".txt";
            var bytes = Encoding.ASCII.GetBytes(message);

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = new MemoryStream(bytes),
                Key = "LogsApp/" + filename,
                BucketName = _bucketName,
                CannedACL = S3CannedACL.PublicRead
            };

            var fileTransferUtility = new TransferUtility(_clientAws);
            await fileTransferUtility.UploadAsync(uploadRequest);
        }
    }
}