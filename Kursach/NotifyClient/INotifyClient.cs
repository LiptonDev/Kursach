using Kursach.Core;
using System;
using System.Threading.Tasks;

namespace Kursach.NotifyClient
{
    public delegate void Notify(int oldId, int newId);

    /// <summary>
    /// Клиент сервера уведомлений.
    /// </summary>
    public interface INotifyClient : INotfyServerMethods
    {
        /// <summary>
        /// Подключение к серверу.
        /// </summary>
        /// <returns></returns>
        Task ConnectAsync();

        /// <summary>
        /// Подключен к серверу уведомлений.
        /// </summary>
        event Action Connected;

        /// <summary>
        /// Отключен от сервера уведомлений.
        /// </summary>
        event Action Disconnected;

        /// <summary>
        /// Переподключен к серверу уведомлений.
        /// </summary>
        event Action Reconnected;

        /// <summary>
        /// Уведомить об изменении студента.
        /// </summary>
        event Notify StudentChanged;

        /// <summary>
        /// Уведомить об изменении группы.
        /// </summary>
        event Notify GroupChanged;

        /// <summary>
        /// Уведомить об изменении сотрудника.
        /// </summary>
        event Action StaffChanged;

        /// <summary>
        /// Уведомить об изменении пользователя.
        /// </summary>
        event Action UserChanged;
    }
}
