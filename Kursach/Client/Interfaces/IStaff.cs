﻿using Kursach.Client.Delegates;
using Kursach.Core.Models;
using Kursach.Core.ServerMethods;

namespace Kursach.Client.Interfaces
{
    /// <summary>
    /// Управление сотрудниками.
    /// </summary>
    interface IStaff : StaffMethods
    {
        /// <summary>
        /// Сотрудник изменен.
        /// </summary>
        event OnChanged<Staff> OnChanged;
    }
}