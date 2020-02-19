using ISTraining_Part.Core.Models;

namespace ISTraining_Part.Core.ServerEvents
{
    /// <summary>
    /// Список событий в хабе студентов.
    /// </summary>
    public interface IStudentsHubEvents : IChangedEvent<Student>
    {
        /// <summary>
        /// Пользователь импортировал список студентов.
        /// </summary>
        void StudentsImported();
    }
}
