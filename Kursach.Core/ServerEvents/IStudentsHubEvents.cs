using Kursach.Core.Models;

namespace Kursach.Core.ServerEvents
{
    /// <summary>
    /// Список событий в хабе студентов.
    /// </summary>
    public interface IStudentsHubEvents : IChangedEvent<Student>
    {
        /// <summary>
        /// Пользователь импортировал список студентов в группу.
        /// </summary>
        /// <param name="groupId">ИД группы.</param>
        void StudentsImportTo(int groupId);
    }
}
