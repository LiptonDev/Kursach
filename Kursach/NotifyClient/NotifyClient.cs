using Kursach.Core;
using Kursach.Properties;
using Kursach.Views;
using Microsoft.AspNet.SignalR.Client;
using Prism.Regions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Kursach.NotifyClient
{
    /// <summary>
    /// Клиент сервера уведомлений.
    /// </summary>
    class NotifyClient : INotifyClient
    {
        /// <summary>
        /// Proxy.
        /// </summary>
        readonly IHubProxy proxy;

        /// <summary>
        /// Hub.
        /// </summary>
        readonly HubConnection hub;

        /// <summary>
        /// Менеджер регионов.
        /// </summary>
        readonly IRegionManager regionManager;

        /// <summary>
        /// Поток синхронизации.
        /// </summary>
        TaskFactory Sync { get; } = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());

        /// <summary>
        /// Конструктор.
        /// </summary>
        public NotifyClient(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

            hub = new HubConnection(Settings.Default.notifyServer);
            proxy = hub.CreateHubProxy("NotificationHub");

            InitEvents();

            hub.Closed += () => Disconnected?.Invoke();
            hub.Reconnected += () => Reconnected?.Invoke();

            hub.Received += Hub_Received;
        }

        private void Hub_Received(string obj)
        {
            Debug.WriteLine(obj);
        }

        /// <summary>
        /// Подписка на события сервера.
        /// </summary>
        private void InitEvents()
        {
            proxy.On<int, int>(nameof(INotifyServerEvents.StudentChanged), (oldId, newId) => CallAction<StudentsView>(() => StudentChanged?.Invoke(oldId, newId)));

            proxy.On<int, int>(nameof(INotifyServerEvents.GroupChanged), (oldId, newId) => CallAction<GroupsView>(() => GroupChanged?.Invoke(oldId, newId)));

            proxy.On(nameof(INotifyServerEvents.StaffChanged), () => CallAction<StaffView>(StaffChanged));

            proxy.On(nameof(INotifyServerEvents.UserChanged), () => CallAction<UsersView>(UserChanged));

            void CallAction<TView>(Action action)
            {
                if (GetCurrentView().GetType() == typeof(TView))
                    Sync.StartNew(action);
            }

            object GetCurrentView()
            {
                return regionManager.Regions[RegionNames.MainRegion].ActiveViews.FirstOrDefault() ?? new object();
            }
        }

        /// <summary>
        /// Подключение к серверу.
        /// </summary>
        /// <returns></returns>
        public async Task ConnectAsync()
        {
            try
            {
                await hub.Start();

                Connected?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Подключен к серверу уведомлений.
        /// </summary>
        public event Action Connected;

        /// <summary>
        /// Отключен от сервера уведомлений.
        /// </summary>
        public event Action Disconnected;

        /// <summary>
        /// Переподключен к серверу уведомлений.
        /// </summary>
        public event Action Reconnected;

        /// <summary>
        /// Уведомить об изменении студента.
        /// </summary>
        public event Notify StudentChanged;

        /// <summary>
        /// Уведомить об изменении группы.
        /// </summary>
        public event Notify GroupChanged;

        /// <summary>
        /// Уведомить об изменении сотрудника.
        /// </summary>
        public event Action StaffChanged;

        /// <summary>
        /// Уведомить об изменении пользователя.
        /// </summary>
        public event Action UserChanged;

        void TryIntoke(string method, params object[] args)
        {
            try
            {
                proxy.Invoke(method, args);
            }
            catch { }
        }

        /// <summary>
        /// Установить статус пользователя (вошел/вышел в программе).
        /// </summary>
        /// <param name="status">Статус.</param>
        /// <returns></returns>
        public void SetStatus(bool status)
        {
            TryIntoke(nameof(INotfyServerMethods.SetStatus), status);
            Consts.LoginStatus = status;
        }

        /// <summary>
        /// Уведомить об изменении студента.
        /// </summary>
        /// <param name="oldGroupId">ИД группы, в которой находился студент.</param>
        /// <param name="newGroupId">ИД группы, в котором находится студент.</param>
        public void ChangeStudent(int oldGroupId, int newGroupId)
        {
            TryIntoke(nameof(INotfyServerMethods.ChangeStudent), oldGroupId, newGroupId);
        }

        /// <summary>
        /// Уведомить об изменении группы.
        /// </summary>
        /// <param name="oldDivision">Подразделение, в котором находилась группа.</param>
        /// <param name="newDivision">Подразделение, в котором находится группа.</param>
        public void ChangeGroup(int oldDivision, int newDivision)
        {
            TryIntoke(nameof(INotfyServerMethods.ChangeGroup), oldDivision, newDivision);
        }

        /// <summary>
        /// Уведомить об изменении сотрудника.
        /// </summary>
        public void ChangeStaff()
        {
            TryIntoke(nameof(INotfyServerMethods.ChangeStaff));
        }

        /// <summary>
        /// Уведомить об изменении пользователя.
        /// </summary>
        public void ChangeUser()
        {
            TryIntoke(nameof(INotfyServerMethods.ChangeUser));
        }
    }
}
