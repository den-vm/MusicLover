namespace MuloApi.Interfaces
{
    /// <summary>
    /// Проверка входных данных пользователя
    /// </summary>
    interface ICheckData
    {
        bool CheckLogin(string login);
        bool CheckPassword(string password);
    }
}