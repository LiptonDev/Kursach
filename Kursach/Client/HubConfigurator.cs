using Kursach.Properties;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace Kursach.Client
{
    /// <summary>
    /// Конфигуратор подключения.
    /// </summary>
    class HubConfigurator : IHubConfigurator
    {
        /// <summary>
        /// Подключен к серверу.
        /// </summary>
        public event Action Connected;

        /// <summary>
        /// Отключен от сервера.
        /// </summary>
        public event Action Disconnected;

        /// <summary>
        /// Переподключен к серверу.
        /// </summary>
        public event Action Reconnected;

        /// <summary>
        /// Hub.
        /// </summary>
        public HubConnection Hub { get; }

        /// <summary>
        /// Sync.
        /// </summary>
        readonly TaskFactory sync;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public HubConfigurator(TaskFactory sync)
        {
            Hub = new HubConnection(Settings.Default.server);

            this.sync = sync;

            Hub.Closed += () => sync.StartNew(() => Disconnected?.Invoke());
            Hub.Reconnected += () => sync.StartNew(() => Reconnected?.Invoke());

            Hub.Received += Console.WriteLine;
        }

        /// <summary>
        /// Подключиться.
        /// </summary>
        /// <returns></returns>
        public async Task ConnectAsync()
        {
            try
            {
                await Hub.Start();

                await sync.StartNew(() => Connected?.Invoke());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
