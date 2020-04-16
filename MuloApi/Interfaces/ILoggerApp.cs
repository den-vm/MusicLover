using System;
using System.Threading.Tasks;

namespace MuloApi.Interfaces
{
    internal interface ILoggerApp
    {
        void LogException(Exception messageError);
        void LogInformation(string messageInfo);
    }
}