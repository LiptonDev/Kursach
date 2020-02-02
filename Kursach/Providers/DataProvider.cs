using Kursach.Client.Interfaces;
using Kursach.Core.Models;
using Kursach.Core.ServerEvents;
using Kursach.Helpers;
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
        /// Клиент сервера.
        /// </summary>
        readonly IClient client;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public DataProvider(IClient client, TaskFactory sync)
        {
            this.client = client;

            client.Users.OnChanged += Users_OnChanged;

            client.Staff.OnChanged += Staff_OnChanged;

            client.Groups.OnChanged += Groups_OnChanged;
            client.Groups.Imported += Groups_Imported;

            client.Students.OnChanged += Students_OnChanged;
            client.Students.Imported += Students_Imported;

            Users = new ObservableCollection<User>();
            Staff = new ObservableCollection<Staff>();
            Groups = new ObservableCollection<Group>();
            Students = new ObservableCollection<Student>();

            Staff.SetRelationship(Groups, (staff, group) => group.CuratorId == staff.Id, sync);
            Groups.SetRelationship(Students, (group, student) => student.GroupId == group.Id, sync);
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
                        Students.Remove(item);
                }

                Students.AddRange(res.Response);
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
                Groups.Clear();
                Groups.AddRange(group.Response);
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
                    Users.AddRange(users.Response);
            }

            var staff = await client.Staff.GetStaffsAsync();

            if (staff)
                Staff.AddRange(staff.Response);

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
                        collection.Add(arg);
                    break;

                case DbChangeStatus.Update:
                    var item = collection.FirstOrDefault(x => x.Equals(arg));
                    int index = collection.IndexOf(item);
                    collection.RemoveAt(index);
                    collection.Insert(index, arg);
                    break;

                case DbChangeStatus.Remove:
                    collection.Remove(arg);
                    break;

                default:
                    Logger.Log.Warn($"Необработанное событие изменения: {{type: {typeof(T)}, arg: {arg}, status: {status}}}");
                    break;
            }
        }
    }
}
