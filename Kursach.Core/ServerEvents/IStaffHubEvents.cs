using Kursach.Core.Models;

namespace Kursach.Core.ServerEvents
{
    /// <summary>
    /// Список событий в хабе сотрудников.
    /// </summary>
    public interface IStaffHubEvents : IChangedEvent<Staff>
    {
    }
}
