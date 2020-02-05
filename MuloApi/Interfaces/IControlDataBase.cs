using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MuloApi.DataBase.Entities;

namespace MuloApi.Interfaces
{
    public interface IControlDataBase
    {
        Task<bool> TestConnection();
        Task<string> GetStrConnection();
    }
}