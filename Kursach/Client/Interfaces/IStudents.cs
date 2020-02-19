using ISTraining_Part.Client.Delegates;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.ServerMethods;

namespace ISTraining_Part.Client.Interfaces
{
    /// <summary>
    /// Управление студентами.
    /// </summary>
    interface IStudents : IStudentsHub
    {
        /// <summary>
        /// Студент изменен.
        /// </summary>
        event OnChanged<Student> OnChanged;

        /// <summary>
        /// Студенты импортированы.
        /// </summary>
        event StudentsImported Imported;
    }
}
