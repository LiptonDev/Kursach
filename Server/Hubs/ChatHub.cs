using Kursach.Core;
using Kursach.Core.Models;
using Kursach.Core.ServerEvents;
using Kursach.Core.ServerMethods;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;

namespace Server.Hubs
{
    /// <summary>
    /// Хаб чата.
    /// </summary>
    [AuthorizeUser]
    [HubName(HubNames.ChatHub)]
    public class ChatHub : Hub<IChatHubEvents>, IChatHub
    {
        /// <summary>
        /// Отправить сообщение в чат.
        /// </summary>
        /// <param name="text">Текст сообщения.</param>
        public void SendMessage(string text)
        {
            User sender = LoginHub.Users[Context.ConnectionId];

            Clients.Group(Consts.AuthorizedGroup).NewMessage(sender, text);
        }
    }
}
