using Kursach.Core.Models;
using Kursach.Core.ServerMethods;

namespace Kursach.Client
{
    /// <summary>
    /// Управление пользователями.
    /// </summary>
    interface IUsers : UsersMethods
    {
        /// <summary>
        /// Пользователь изменен.
        /// </summary>
        event OnChanged<User> OnChanged;
    }
}
