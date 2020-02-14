namespace MuloApi.Interfaces
{
    /// <summary>
    ///     Проверка входных данных пользователя
    /// </summary>
    public interface ICheckData
    {
        bool CheckLoginRegular(string login);
        bool CheckLoginSmtp(string login);
        bool CheckPasswordRegular(string password);
    }
}