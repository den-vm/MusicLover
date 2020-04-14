using System.Threading.Tasks;
using MuloApi.Classes;

namespace MuloApi.Interfaces
{
    internal interface IControlSaveUserFiles
    {
        Task UploadFileAsync();
        Task DownloadFileAsync();
    }
}