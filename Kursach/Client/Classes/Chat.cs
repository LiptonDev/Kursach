using Kursach.Client.Delegates;
using Kursach.Client.Interfaces;
using Kursach.Core;
using Kursach.Core.Models;
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
            Proxy.On<User, string>(nameof(IChatHubEvents.NewMessage), 
                (sender, text) => NewMessage?.Invoke(sender, text));
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
