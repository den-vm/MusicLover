using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MuloApi
{
    public interface IClientHub
    {
        Task GetMusicTrack(string method, IActionResult result);
        Task ErrorResponce(string method, string error);
    }
}