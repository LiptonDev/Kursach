using ISTraining_Part.Client.Delegates;
using ISTraining_Part.Core.ServerMethods;

namespace ISTraining_Part.Client.Interfaces
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
