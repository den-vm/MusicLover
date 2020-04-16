using System;
using Microsoft.Extensions.Logging;
using MuloApi.Interfaces;

namespace MuloApi.Classes
{
    public class LoggerApp : ILoggerApp
    {
        private static LoggerApp _instanceLogger;

        public static LoggerApp Log
        {
            get { return _instanceLogger ??= new LoggerApp(); }
        }

        public async void LogException(Exception messageError)
        {
            if (Startup.Logger != null)
                Startup.Logger.LogError(messageError.ToString());
            await AmazonWebServiceS3.Current.UploadLogAsync(TypesMessageLog.Error, messageError.ToString());
        }

        public async void LogInformation(string messageInfo)
        {
            if (Startup.Logger != null)
                Startup.Logger.LogInformation(messageInfo);
            await AmazonWebServiceS3.Current.UploadLogAsync(TypesMessageLog.Information, messageInfo);
        }
    }
}