using System;

namespace ISTraining_Part.Models
{
    /// <summary>
    /// Сообщение в чате.
    /// </summary>
    class ChatMessage
    {
        /// <summary>
        /// Отправитель.
        /// </summary>
        public string SenderName { get; }

        /// <summary>
        /// Текст сообщения.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Время получения сообщения.
        /// </summary>
        public string Time { get; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ChatMessage(string senderName, string text)
        {
            SenderName = senderName;
            Text = text;
            Time = DateTime.Now.ToShortTimeString();
        }
    }
}
