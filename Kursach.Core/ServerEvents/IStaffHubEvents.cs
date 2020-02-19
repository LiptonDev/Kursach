using ISTraining_Part.Core.Models;

namespace ISTraining_Part.Core.ServerEvents
{
    /// <summary>
    /// Список событий в хабе сотрудников.
    /// </summary>
    public interface IStaffHubEvents : IChangedEvent<Staff>
    {
    }
}
