using Kursach.Client.Delegates;
using Kursach.Core.Models;
using Kursach.Core.ServerMethods;
using System;

namespace Kursach.Client.Interfaces
{
    /// <summary>
    /// Управление группами.
    /// </summary>
    interface IGroups : IGroupsHub
    {
        /// <summary>
        /// Группа изменена.
        /// </summary>
        event OnChanged<Group> OnChanged;

        /// <summary>
        /// Группы импортированы.
        /// </summary>
        event Action Imported;
    }
}
