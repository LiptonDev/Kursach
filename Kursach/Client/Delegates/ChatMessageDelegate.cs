using Kursach.Core.Models;

namespace Kursach.Client.Delegates
{
    /// <summary>
    /// Делегат нового сообщения в чате.
    /// </summary>
    /// <param name="sender">Отправитель.</param>
    /// <param name="text">Текст сообщения.</param>
    delegate void OnChatMessage(User sender, string text);
}
