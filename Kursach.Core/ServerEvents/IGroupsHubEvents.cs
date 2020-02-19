using ISTraining_Part.Core.Models;

namespace ISTraining_Part.Core.ServerEvents
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
