using DevExpress.Mvvm;
using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Models;
using ISTraining_Part.Providers;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ISTraining_Part.ViewModels
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
