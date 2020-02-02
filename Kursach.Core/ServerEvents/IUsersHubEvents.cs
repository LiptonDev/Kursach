using Kursach.Core.Models;

namespace Kursach.Core.ServerEvents
{
    /// <summary>
    /// Список событий в хабе пользователей.
    /// </summary>
    public interface IUsersHubEvents : IChangedEvent<User>
    {
    }
}
