using Kursach.Core.Models;
using Kursach.Core.ServerMethods;

namespace Kursach.Client
{
    /// <summary>
    /// Управление группами.
    /// </summary>
    interface IGroups : GroupsMethods
    {
        /// <summary>
        /// Группа изменена.
        /// </summary>
        event OnChanged<Group> OnChanged;

        /// <summary>
        /// Группы импортированы.
        /// </summary>
        event GroupsImported Imported;
    }
}
