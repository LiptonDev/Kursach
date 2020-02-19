namespace ISTraining_Part.Core.ServerEvents
{
    /// <summary>
    /// Список событий в хабе чата.
    /// </summary>
    public interface IChatHubEvents
    {
        /// <summary>
        /// Новое сообщение в чате.
        /// </summary>
        /// <param name="senderName">Отправитель.</param>
        /// <param name="text">Текст сообщения.</param>
        void NewMessage(string senderName, string text);
    }
}
