using ISTraining_Part.Client.Interfaces;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace ISTraining_Part.Client.Design
{
    class DesignConfigurator : IHubConfigurator
    {
        public HubConnection Hub { get; }

        public event Action Connected;
        public event Action Disconnected;
        public event Action Reconnected;
        public event Action Reconnecting;

        public async Task ConnectAsync()
        {
            await Task.Delay(500);
            Connected?.Invoke();
        }
    }
}
