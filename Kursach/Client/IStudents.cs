using Kursach.Core.Models;
using Kursach.Core.ServerMethods;

namespace Kursach.Client
{
    /// <summary>
    /// Управление студентами.
    /// </summary>
    interface IStudents : StudentsMethods
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
