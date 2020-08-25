using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MuloApi
{
    public interface IClientHub
    {
        Task PartMusicTrack(byte[] byteTrack);
        Task Message(string message);
        Task Error(string error);
    }
}