using Kursach.Client.Delegates;
using Kursach.Core.ServerMethods;

namespace Kursach.Client.Interfaces
{
    /// <summary>
    /// Управление чатом.
    /// </summary>
    interface IChat : IChatHub
    {
        /// <summary>
        /// Новое сообщение.
        /// </summary>
        event OnChatMessage NewMessage;
    }
}
