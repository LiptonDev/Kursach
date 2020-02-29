using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.ServerMethods;

namespace Server.DataBase.Interfaces
{
    /// <summary>
    /// Репозиторий пользователей.
    /// </summary>
    public interface IUsersRepository : IUsersHub
    {
        /// <summary>
        /// Добавить лог входа в программу.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        void AddSignInLogAsync(User user);
    }
}
