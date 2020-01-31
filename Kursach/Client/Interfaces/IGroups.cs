using Kursach.Client.Delegates;
using Kursach.Core.Models;
using Kursach.Core.ServerMethods;

namespace Kursach.Client.Interfaces
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
