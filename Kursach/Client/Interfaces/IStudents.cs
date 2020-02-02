using Kursach.Client.Delegates;
using Kursach.Core.Models;
using Kursach.Core.ServerMethods;

namespace Kursach.Client.Interfaces
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
