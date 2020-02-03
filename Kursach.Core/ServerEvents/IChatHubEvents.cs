using Kursach.Core.Models;

namespace Kursach.Core.ServerEvents
{
    /// <summary>
    /// Список событий в хабе чата.
    /// </summary>
    public interface IChatHubEvents
    {
        /// <summary>
        /// Новое сообщение в чате.
        /// </summary>
        /// <param name="sender">Отправитель.</param>
        /// <param name="text">Текст сообщения.</param>
        void NewMessage(User sender, string text);
    }
}
