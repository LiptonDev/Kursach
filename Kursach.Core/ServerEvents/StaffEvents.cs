using Kursach.Core.Models;

namespace Kursach.Core.ServerEvents
{
    /// <summary>
    /// Список событий в хабе сотрудников.
    /// </summary>
    public interface StaffEvents
    {
        /// <summary>
        /// Изменение сотрудника.
        /// </summary>
        /// <param name="status">Статус.</param>
        /// <param name="staff">Сотрудник.</param>
        void StaffChange(DbChangeStatus status, Staff staff);
    }
}
