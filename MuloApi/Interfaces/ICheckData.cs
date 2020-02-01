namespace MuloApi.Interfaces
{
    /// <summary>
    ///     Проверка входных данных пользователя
    /// </summary>
    internal interface ICheckData
    {
        bool CheckLoginRegular(string login);
        bool CheckLoginSmtp(string login);
        bool CheckPassword(string password);
        string GetHash(int idUser, string agent);
    }
}