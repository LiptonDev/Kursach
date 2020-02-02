using Kursach.Core.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Server.Hubs
{
    /// <summary>
    /// Указывает, что доступ к хабу есть только у администраторов.
    /// </summary>
    class AdminHubAttribute : AuthorizeAttribute
    {
        public override bool AuthorizeHubMethodInvocation(IHubIncomingInvokerContext context, bool appliesToMethod)
        {
            LoginHub.Users.TryGetValue(context.Hub.Context.ConnectionId, out var mode);

            return mode == UserMode.Admin;
        }

        public override bool AuthorizeHubConnection(HubDescriptor hubDescriptor, IRequest request)
        {
            return true;
        }
    }
}
