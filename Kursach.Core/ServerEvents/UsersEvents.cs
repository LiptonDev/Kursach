using Kursach.Core.Models;

namespace Kursach.Core.ServerEvents
{
    /// <summary>
    /// Список событий в хабе пользователей.
    /// </summary>
    public interface UsersEvents
    {
        /// <summary>
        /// Изменение пользователя.
        /// </summary>
        /// <param name="status">Статус.</param>
        /// <param name="user">Пользователь.</param>
        void UserChanged(DbChangeStatus status, User user);
    }
}
