using Kursach.Core.Models;
using System;

namespace Kursach.Models
{
    /// <summary>
    /// Сообщение в чате.
    /// </summary>
    class ChatMessage
    {
        /// <summary>
        /// Отправитель.
        /// </summary>
        public User User { get; }

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
        public ChatMessage(User user, string text)
        {
            User = user;
            Text = text;
            Time = DateTime.Now.ToShortTimeString();
        }
    }
}
