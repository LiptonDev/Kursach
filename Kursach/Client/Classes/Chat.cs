using ISTraining_Part.Client.Delegates;
using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core;
using ISTraining_Part.Core.ServerEvents;
using Microsoft.AspNet.SignalR.Client;

namespace ISTraining_Part.Client.Classes
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
        /// <param name="text">Текст сообщения.</param>
        public void SendMessage(string text)
        {
            Logger.Log.Info($"Отправка сообщения в чат: {{text: {text}}}");

            TryInvokeAsync(args: new[] { text });
        }
    }
}
