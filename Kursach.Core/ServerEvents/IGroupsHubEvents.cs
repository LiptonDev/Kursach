using Kursach.Core.Models;

namespace Kursach.Core.ServerEvents
{
    /// <summary>
    /// Список событий в хабе групп.
    /// </summary>
    public interface IGroupsHubEvents : IChangedEvent<Group>
    {
        /// <summary>
        /// Добавлено несколько групп (импорт из Excel).
        /// </summary>
        void GroupsImport();
    }
}
