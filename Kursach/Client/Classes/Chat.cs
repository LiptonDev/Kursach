using Kursach.Client.Delegates;
using Kursach.Client.Interfaces;
using Kursach.Core;
using Kursach.Core.ServerEvents;
using Microsoft.AspNet.SignalR.Client;

namespace Kursach.Client.Classes
{
    /// <summary>
    /// Управление чатом.
    /// </summary>
    class Chat : Invoker, IChat
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public Chat(IHubConfigurator hubConfigurator) : base(hubConfigurator, HubNames.ChatHub)
        {
            Proxy.On<string, string>(nameof(IChatHubEvents.NewMessage), 
                (senderName, text) => NewMessage?.Invoke(senderName, text));
        }

        /// <summary>
        /// Новое сообщение.
        /// </summary>
        public event OnChatMessage NewMessage;

        /// <summary>
        /// Отправить сообщение.
        /// </summary>
        /// <param name="text"></param>
        public void SendMessage(string text)
        {
            TryInvokeAsync(args: new[] { text });
        }
    }
}
