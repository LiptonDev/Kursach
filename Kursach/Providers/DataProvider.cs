using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.ServerEvents;
using ISTraining_Part.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ISTraining_Part.Providers
{
    /// <summary>
    /// Поставщик данных.
    /// </summary>
    class DataProvider : IDataProvider
    {
        /// <summary>
        /// Пользователи.
        /// </summary>
        public ObservableCollection<User> Users { get; }

        /// <summary>
        /// Сотрудники.
        /// </summary>
        public ObservableCollection<Staff> Staff { get; }

        /// <summary>
        /// Группы.
        /// </summary>
        public ObservableCollection<Group> Groups { get; }

        /// <summary>
        /// Сообщения в чате.
        /// </summary>
        public ObservableCollection<ChatMessage> ChatMessages { get; }

        /// <summary>
        /// Клиент сервера.
        /// </summary>
        readonly IClient client;

        /// <summary>
        /// Поток синхронизации.
        /// </summary>
        readonly TaskFactory sync;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public DataProvider(IClient client, TaskFactory sync)
        {
            this.client = client;
            this.sync = sync;

            client.Users.OnChanged += Users_OnChanged;

            client.Staff.OnChanged += Staff_OnChanged;

            client.Groups.OnChanged += Groups_OnChanged;
            client.Groups.Imported += Groups_Imported;

            client.Chat.NewMessage += Chat_NewMessage;

            Users = new ObservableCollection<User>();
            Staff = new ObservableCollection<Staff>();
            Groups = new ObservableCollection<Group>();
            ChatMessages = new ObservableCollection<ChatMessage>();
        }

        /// <summary>
        /// Сообщение в чате.
        /// </summary>
        /// <param name="senderName">Отправитель.</param>
        /// <param name="text">Текст сообщения.</param>
        private void Chat_NewMessage(string senderName, string text)
        {
            sync.StartNew(() => ChatMessages.Add(new ChatMessage(senderName, text)));
        }

        /// <summary>
        /// Импортированы группы.
        /// </summary>
        private async void Groups_Imported()
        {
            var group = await client.Groups.GetGroupsAsync();

            if (group)
            {
                await sync.StartNew(() =>
                {
                    Groups.Clear();
                    Groups.AddRange(group.Response);
                });
            }
        }

        /// <summary>
        /// Загрузка данных.
        /// </summary>
        public async void Load(UserMode mode)
        {
            if (mode == UserMode.Admin)
            {
                var users = await client.Users.GetUsersAsync();
                if (users)
                    await sync.StartNew(() => Users.AddRange(users.Response));
            }

            var staff = await client.Staff.GetStaffsAsync();

            if (staff)
                await sync.StartNew(() => Staff.AddRange(staff.Response));

            Groups_Imported();
        }

        /// <summary>
        /// Очистка данных.
        /// </summary>
        public void Clear()
        {
            Users.Clear();
            Staff.Clear();
            Groups.Clear();
            ChatMessages.Clear();
        }

        /// <summary>
        /// Изменения группы.
        /// </summary>
        private void Groups_OnChanged(DbChangeStatus status, Group arg)
        {
            ProcessChanges(status, arg, Groups);
        }

        /// <summary>
        /// Изменения сотрудника.
        /// </summary>
        private void Staff_OnChanged(DbChangeStatus status, Staff arg)
        {
            ProcessChanges(status, arg, Staff);
        }

        /// <summary>
        /// Изменения пользователя.
        /// </summary>
        private void Users_OnChanged(DbChangeStatus status, User arg)
        {
            ProcessChanges(status, arg, Users);
        }

        /// <summary>
        /// Обработка изменения.
        /// </summary>
        void ProcessChanges<T>(DbChangeStatus status, T arg, ObservableCollection<T> collection)
        {
            ProcessChangesHelper.ProcessChanges(status, arg, collection, sync);

            if (status == DbChangeStatus.Remove && arg is Staff staff)
            {
                Clear(Groups, x => x.CuratorId == staff.Id);
            }
        }

        /// <summary>
        /// Связь между коллекциями.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">Коллекция, из которой будут удалены данные.</param>
        /// <param name="func">Фильтрация.</param>
        void Clear<T>(ObservableCollection<T> collection, Func<T, bool> func)
        {
            var removeList = collection.Where(func).ToList();
            foreach (var item in removeList)
            {
                collection.Remove(item);
            }
        }
    }
}
