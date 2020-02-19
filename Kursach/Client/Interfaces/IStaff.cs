using ISTraining_Part.Client.Delegates;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.ServerMethods;

namespace ISTraining_Part.Client.Interfaces
{
    /// <summary>
    /// Управление сотрудниками.
    /// </summary>
    interface IStaff : IStaffHub
    {
        /// <summary>
        /// Сотрудник изменен.
        /// </summary>
        event OnChanged<Staff> OnChanged;
    }
}
