using Kursach.Core;
using Microsoft.AspNet.SignalR;
using System;
using System.Threading.Tasks;

namespace NotificationServer
{
    /// <summary>
    /// Hub для уведомлений.
    /// </summary>
    public class NotificationHub : Hub<INotifyServerEvents>, INotfyServerMethods
    {
        const string NotifyGroup = nameof(NotifyGroup);

        /// <summary>
        /// Установить статус пользователя (вошел/вышел в программе).
        /// </summary>
        /// <param name="status">Статус.</param>
        /// <returns></returns>
        public void SetStatus(bool status)
        {
            if (status)
                Groups.Add(Context.ConnectionId, NotifyGroup);
            else
                Groups.Remove(Context.ConnectionId, NotifyGroup);

            Console.WriteLine($"Set status: {Context.ConnectionId} => {status}");
        }

        /// <summary>
        /// Уведомить об изменении студента.
        /// </summary>
        /// <param name="oldGroupId">ИД группы, в которой находился студент.</param>
        /// <param name="newGroupId">ИД группы, в котором находится студент.</param>
        public void ChangeStudent(int oldGroupId, int newGroupId)
        {
            Clients.Group(NotifyGroup, Context.ConnectionId).StudentChanged(oldGroupId, newGroupId);

            Console.WriteLine($"{nameof(ChangeStudent)}({oldGroupId}, {newGroupId}) => {Context.ConnectionId}");
        }

        /// <summary>
        /// Уведомить об изменении группы.
        /// </summary>
        /// <param name="oldDivision">Подразделение, в котором находилась группа.</param>
        /// <param name="newDivision">Подразделение, в котором находится группа.</param>
        public void ChangeGroup(int oldDivision, int newDivision)
        {
            Clients.Group(NotifyGroup, Context.ConnectionId).GroupChanged(oldDivision, newDivision);

            Console.WriteLine($"{nameof(ChangeGroup)}({oldDivision}, {newDivision}) => {Context.ConnectionId}");
        }

        /// <summary>
        /// Уведомить об изменении сотрудника.
        /// </summary>
        public void ChangeStaff()
        {
            Clients.Group(NotifyGroup, Context.ConnectionId).StaffChanged();

            Console.WriteLine($"{nameof(ChangeStaff)}() => {Context.ConnectionId}");
        }

        /// <summary>
        /// Уведомить об изменении пользователя.
        /// </summary>
        public void ChangeUser()
        {
            Clients.Group(NotifyGroup, Context.ConnectionId).UserChanged();

            Console.WriteLine($"{nameof(ChangeUser)}() => {Context.ConnectionId}");
        }

        /// <summary>
        /// Клиент переподключен.
        /// </summary>
        /// <returns></returns>
        public override Task OnReconnected()
        {
            Console.WriteLine($"Reconnected: {Context.ConnectionId}");

            return base.OnReconnected();
        }

        /// <summary>
        /// Клиент подключен.
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {
            Console.WriteLine($"Connected: {Context.ConnectionId}");

            return base.OnConnected();
        }

        /// <summary>
        /// Клиент отключен.
        /// </summary>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            Console.WriteLine($"Disconnected: {Context.ConnectionId}");

            Groups.Remove(Context.ConnectionId, NotifyGroup);
            return base.OnDisconnected(stopCalled);
        }
    }
}
