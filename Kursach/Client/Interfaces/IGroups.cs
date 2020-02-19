using ISTraining_Part.Client.Delegates;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.ServerMethods;
using System;

namespace ISTraining_Part.Client.Interfaces
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
