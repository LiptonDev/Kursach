using Kursach.Core.Models;

namespace Kursach.Core.ServerEvents
{
    /// <summary>
    /// Список событий в хабе студентов.
    /// </summary>
    public interface StudentsEvents
    {
        /// <summary>
        /// Изменение студента.
        /// </summary>
        /// <param name="status">Статус.</param>
        /// <param name="student">Студент.</param>
        void StudentChanged(DbChangeStatus status, Student student);

        /// <summary>
        /// Пользователь импортировал список студентов в группу.
        /// </summary>
        /// <param name="groupId">ИД группы.</param>
        void StudentsImportTo(int groupId);
    }
}
