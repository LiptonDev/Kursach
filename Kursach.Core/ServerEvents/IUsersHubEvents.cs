using ISTraining_Part.Core.Models;

namespace ISTraining_Part.Core.ServerEvents
{
    /// <summary>
    /// Список событий в хабе пользователей.
    /// </summary>
    public interface IUsersHubEvents : IChangedEvent<User>
    {
    }
}
