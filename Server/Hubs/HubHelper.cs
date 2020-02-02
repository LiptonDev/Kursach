using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;

namespace Server.Hubs
{
    static class HubHelper
    {
        public static Task RemoveFromAdminGroup<THub>(string id) where THub : IHub
            => RemoveFromGroup<THub>(id, Consts.AdminGroup);

        public static Task RemoveFromAuthorizedGroup<THub>(string id) where THub : IHub
            => RemoveFromGroup<THub>(id, Consts.AuthorizedGroup);

        public static Task RemoveFromGroup<THub>(string id, string group) where THub : IHub
            => GetHub<THub>().Groups.Remove(id, group);

        public static Task AddToAdminGroup<THub>(string id) where THub : IHub
            => AddToGroup<THub>(id, Consts.AdminGroup);

        public static Task AddToAuthorizedGroup<THub>(string id) where THub : IHub
            => AddToGroup<THub>(id, Consts.AuthorizedGroup);

        public static Task AddToGroup<THub>(string id, string group) where THub : IHub
            => GetHub<THub>().Groups.Add(id, group);

        public static IHubContext GetHub<THub>() where THub : IHub
            => GlobalHost.ConnectionManager.GetHubContext<THub>();
    }
}
