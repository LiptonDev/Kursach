using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Server.Hubs
{
    static class HubHelper
    {
        public static IHubContext GetHubContext<THub>() where THub : IHub
            => GlobalHost.ConnectionManager.GetHubContext<THub>();

        public static IHubContext<TEvents> GetHubContext<THub, TEvents>() where THub : IHub where TEvents : class
            => GlobalHost.ConnectionManager.GetHubContext<THub, TEvents>();
    }
}
