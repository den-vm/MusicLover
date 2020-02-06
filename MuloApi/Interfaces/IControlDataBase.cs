using System.Threading.Tasks;

namespace MuloApi.Interfaces
{
    public interface IControlDataBase
    {
        Task<bool> TestConnection();
        Task<string> GetStrConnection();
    }
}