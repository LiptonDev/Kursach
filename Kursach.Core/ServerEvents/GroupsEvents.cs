using Kursach.Core.Models;

namespace Kursach.Core.ServerEvents
{
    /// <summary>
    /// Список событий в хабе групп.
    /// </summary>
    public interface GroupsEvents
    {
        /// <summary>
        /// Изменение группы.
        /// </summary>
        /// <param name="status">Статус.</param>
        /// <param name="group">Группа.</param>
        void GroupChanged(DbChangeStatus status, Group group);

        /// <summary>
        /// Добавлено несколько групп (импорт из Excel).
        /// </summary>
        void GroupsImport();
    }
}
