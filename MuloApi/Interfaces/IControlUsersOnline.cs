namespace MuloApi.Interfaces
{
    /// <summary>
    /// Управление пользователями
    /// </summary>
    /// <typeparam name="S">User</typeparam>
    /// <typeparam name="L">List user</typeparam>
    interface IControlUsersOnline<in S, out L>
    {
        void Add(S user);
        L Get();
    }
}