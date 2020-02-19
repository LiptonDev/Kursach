using Microsoft.AspNet.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace ISTraining_Part.Client.Interfaces
{
    /// <summary>
    /// Конфигуратор подключения.
    /// </summary>
    interface IHubConfigurator
    {
        /// <summary>
        /// Hub.
        /// </summary>
        HubConnection Hub { get; }

        /// <summary>
        /// Подключен к серверу.
        /// </summary>
        event Action Connected;

        /// <summary>
        /// Отключен от сервера.
        /// </summary>
        event Action Disconnected;

        /// <summary>
        /// Переподключен к серверу.
        /// </summary>
        event Action Reconnected;

        /// <summary>
        /// Процесс переподключения к серверу.
        /// </summary>
        event Action Reconnecting;

        /// <summary>
        /// Подключиться.
        /// </summary>
        /// <returns></returns>
        Task ConnectAsync();
    }
}
