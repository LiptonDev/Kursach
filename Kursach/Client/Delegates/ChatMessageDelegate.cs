namespace ISTraining_Part.Client.Delegates
{
    /// <summary>
    /// Делегат нового сообщения в чате.
    /// </summary>
    /// <param name="senderName">Отправитель.</param>
    /// <param name="text">Текст сообщения.</param>
    delegate void OnChatMessage(string senderName, string text);
}
