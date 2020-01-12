namespace MuloApi.Interfaces
{
    /// <summary>
    /// Управление пользователями
    /// </summary>
    /// <typeparam name="S">User</typeparam>
    /// <typeparam name="L">List user</typeparam>
    internal interface IControlUser<in S, out L>
    {
        void SetUser(S user);
        L GetUsers();
    }
}