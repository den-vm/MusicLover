using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using MuloApi.Interfaces;
using Newtonsoft.Json.Linq;

namespace MuloApi.Classes
{
    internal class AmazonWebServiceS3 : IAmazonWebServiceS3, IControlSaveLogs
    {
        private static AmazonWebServiceS3 _instance;
        private readonly AmazonS3Client _clientAws = new AmazonS3Client("AKIAJ5ASHTQ3JPIGMOKA",
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

        public async Task UploadLogAsync(TypesMessageLog typeMessage, string message)
        {
            var filename = typeMessage + "_" +
                           DateTime.Now.ToString("yyyyMMdd hh:mm:ss tt", CultureInfo.InvariantCulture) + ".txt";
            var bytes = Encoding.ASCII.GetBytes(message);

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = new MemoryStream(bytes),
                Key = filename,
                BucketName = "storage-app-musiclover",
                CannedACL = S3CannedACL.PublicRead
            };

            var fileTransferUtility = new TransferUtility(_clientAws);
            await fileTransferUtility.UploadAsync(uploadRequest);
        }

        public bool GetDirectoryInfo(string directory)
        {
            throw new NotImplementedException();
        }
    }
}