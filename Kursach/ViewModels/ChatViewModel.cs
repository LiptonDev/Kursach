using DevExpress.Mvvm;
using Kursach.Client.Interfaces;
using Kursach.Models;
using Kursach.Providers;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Kursach.ViewModels
{
    /// <summary>
    /// Chat view model.
    /// </summary>
    class ChatViewModel : ViewModelBase
    {
        /// <summary>
        /// Текст сообщения.
        /// </summary>
        public string MessageText { get; set; }

        /// <summary>
        /// Сообщения.
        /// </summary>
        public ObservableCollection<ChatMessage> Messages { get; }

        /// <summary>
        /// Клиент сервера.
        /// </summary>
        readonly IClient client;

        /// <summary>
        /// Конструктор для DesignTime.
        /// </summary>
        public ChatViewModel()
        {
            Messages = new ObservableCollection<ChatMessage>
            {
                new ChatMessage(new Core.Models.User{ Login = "DESIGN NAME"}, "TEXT")
            };
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ChatViewModel(IDataProvider dataProvider, IClient client)
        {
            Messages = dataProvider.ChatMessages;
            this.client = client;

            SendMessageCommand = new DelegateCommand(SendMessage);
        }

        /// <summary>
        /// Команда отправки сообщения.
        /// </summary>
        public ICommand SendMessageCommand { get; }

        /// <summary>
        /// Отправить сообщение.
        /// </summary>
        private void SendMessage()
        {
            if (MessageText.IsEmpty())
                return;

            client.Chat.SendMessage(MessageText);

            MessageText = "";
        }
    }
}
