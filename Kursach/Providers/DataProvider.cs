using Kursach.Client.Interfaces;
using Kursach.Core.Models;
using Kursach.Core.ServerEvents;
using Kursach.Helpers;
using Kursach.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Kursach.Providers
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
        /// Студенты.
        /// </summary>
        public ObservableCollection<Student> Students { get; }

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

            client.Students.OnChanged += Students_OnChanged;
            client.Students.Imported += Students_Imported;

            client.Chat.NewMessage += Chat_NewMessage;

            Users = new ObservableCollection<User>();
            Staff = new ObservableCollection<Staff>();
            Groups = new ObservableCollection<Group>();
            Students = new ObservableCollection<Student>();
            ChatMessages = new ObservableCollection<ChatMessage>();

            Staff.SetRelationship(Groups, (staff, group) => group.CuratorId == staff.Id, sync);
            Groups.SetRelationship(Students, (group, student) => student.GroupId == group.Id, sync);
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
        /// Импортированы студенты.
        /// </summary>
        /// <param name="groupId">ИД группы.</param>
        private async void Students_Imported(int groupId)
        {
            var res = await client.Students.GetStudentsAsync(groupId);

            if (res)
            {
                if (groupId > -1)
                {
                    var forRemove = Students.Where(x => x.GroupId == groupId).ToList();
                    foreach (var item in forRemove)
                        await sync.StartNew(() => Students.Remove(item));
                }

                await sync.StartNew(() => Students.AddRange(res.Response));
            }
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
            Students_Imported(-1);
        }

        /// <summary>
        /// Очистка данных.
        /// </summary>
        public void Clear()
        {
            Users.Clear();
            Staff.Clear();
            Groups.Clear();
            Students.Clear();
            ChatMessages.Clear();
        }

        /// <summary>
        /// Изменения студента.
        /// </summary>
        private void Students_OnChanged(DbChangeStatus status, Student arg)
        {
            ProcessChanges(status, arg, Students);
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
            switch (status)
            {
                case DbChangeStatus.Add:
                    if (!collection.Contains(arg))
                        sync.StartNew(() => collection.Add(arg));
                    break;

                case DbChangeStatus.Update:
                    var item = collection.FirstOrDefault(x => x.Equals(arg));
                    int index = collection.IndexOf(item);
                    sync.StartNew(() =>
                    {
                        collection.RemoveAt(index);
                        collection.Insert(index, arg);
                    });
                    break;

                case DbChangeStatus.Remove:
                    sync.StartNew(() => collection.Remove(arg));
                    break;

                default:
                    Logger.Log.Warn($"Необработанное событие изменения: {{type: {typeof(T)}, arg: {arg}, status: {status}}}");
                    break;
            }
        }
    }
}
