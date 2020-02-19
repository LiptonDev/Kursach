using ISTraining_Part.Client.Delegates;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.ServerMethods;

namespace ISTraining_Part.Client.Interfaces
{
    /// <summary>
    /// Управление пользователями.
    /// </summary>
    interface IUsers : IUsersHub
    {
        /// <summary>
        /// Пользователь изменен.
        /// </summary>
        event OnChanged<User> OnChanged;
    }
}
