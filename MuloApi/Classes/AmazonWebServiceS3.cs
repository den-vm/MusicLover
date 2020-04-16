using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
        private readonly string _bucketName = "storage-app-musiclover";

        private readonly IAmazonS3 _clientAws = new AmazonS3Client("AKIAJ5ASHTQ3JPIGMOKA",
            "tkLM5m0sLLwCPyGTssOmfJfl94GikINyN0QDPIUA",
            RegionEndpoint.EUNorth1);

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

        ///Рефакторинг
        public async Task<IEnumerable<Stream>> GetStreamTraks(string directory)
        {
            var nameTracks = await GetNameTraks(directory);
            var listStreamFile = new List<Stream>();
            foreach (var name in nameTracks)
            {
                var response = await _clientAws.GetObjectAsync(_bucketName, name);
                var newStreamFormFile = new MemoryStream();
                await response.ResponseStream.CopyToAsync(newStreamFormFile);
                listStreamFile.Add(newStreamFormFile);
            }
            return listStreamFile;
        }

        ///Рефакторинг
        public async Task<IEnumerable<string>> GetNameTraks(string directory)
        {
            var request = new ListObjectsRequest
            {
                BucketName = _bucketName,
                Prefix = directory
            };

            var response = await _clientAws.ListObjectsAsync(request);
            var traks = response.S3Objects.Where(e => e.Key.Contains(".mp3"))
                .Select(e => e.Key).ToList();
            return traks;
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

        public async Task<bool> UploadFile(string directory, Stream inputStream)
        {
            try
            {
                var request = new PutObjectRequest()
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