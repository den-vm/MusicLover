using System.Threading.Tasks;
using MuloApi.Classes;

namespace MuloApi.Interfaces
{
    internal interface IControlSaveLogs
    {
        Task UploadLogAsync(TypesMessageLog typeMessage, string message);
    }
}