using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Server.Hubs
{
    /// <summary>
    /// Указывает, что доступ к хабу есть только у авторизованных пользователей.
    /// </summary>
    class AuthorizeUserAttribute : AuthorizeAttribute
    {
        public override bool AuthorizeHubMethodInvocation(IHubIncomingInvokerContext context, bool appliesToMethod)
        {
            return LoginHub.Users.ContainsKey(context.Hub.Context.ConnectionId);
        }

        public override bool AuthorizeHubConnection(HubDescriptor hubDescriptor, IRequest request)
        {
            return true;
        }
    }
}
