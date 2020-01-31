using DevExpress.Mvvm;
using DryIoc;
using Kursach.Client;
using Kursach.Core.Models;
using Kursach.Core.ServerEvents;
using Kursach.Dialogs;
using MaterialDesignThemes.Wpf;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Kursach.ViewModels
{
    /// <summary>
    /// Users view model.
    /// </summary>
    class UsersViewModel : BaseViewModel<User>
    {
        /// <summary>
        /// Пользователи.
        /// </summary>
        public ObservableCollection<User> Users { get; }

        /// <summary>
        /// Конструктор для DesignTime.
        /// </summary>
        public UsersViewModel()
        {

        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public UsersViewModel(IDialogManager dialogManager,
                              ISnackbarMessageQueue snackbarMessageQueue,
                              IClient client,
                              IContainer container)
            : base(dialogManager, snackbarMessageQueue, client, container)
        {
            Users = new ObservableCollection<User>();

            ShowLogsCommand = new DelegateCommand<User>(ShowLogs);

            client.Users.OnChanged += Users_OnChanged;
        }

        /// <summary>
        /// Изменения пользователей.
        /// </summary>
        private void Users_OnChanged(DbChangeStatus status, User user)
        {
            switch (status)
            {
                case DbChangeStatus.Add:
                    Users.Add(user);
                    break;

                case DbChangeStatus.Update:
                    var current = Users.FirstOrDefault(x => x.Id == user.Id);
                    current?.SetAllFields(user);
                    break;

                case DbChangeStatus.Remove:
                    Users.Remove(user);
                    break;
            }
        }

        /// <summary>
        /// Команда открытия логов входов.
        /// </summary>
        public ICommand<User> ShowLogsCommand { get; }

        /// <summary>
        /// Открыть окно регистрации.
        /// </summary>
        public override async void Add()
        {
            var editor = await dialogManager.SignUp(null, false);
            if (editor == null)
                return;

            var res = await client.Users.AddUserAsync(editor);
            var msg = res ? "Пользователь добавлен" : res;

            Log(msg, editor);
        }

        /// <summary>
        /// Сохранить новые данные пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        public override async Task Edit(User user)
        {
            var editor = await dialogManager.SignUp(user, true);
            if (editor == null)
                return;

            var res = await client.Users.SaveUserAsync(editor);
            var msg = res ? "Пользователь сохранен" : res;

            Log(msg, user);
        }

        /// <summary>
        /// Удаление пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        public override async Task Delete(User user)
        {
            var answ = await dialogIdentifier.ShowMessageBoxAsync($"Удалить '{user.Login}'?", MaterialMessageBoxButtons.YesNo);
            if (answ != MaterialMessageBoxButtons.Yes)
                return;

            var res = await client.Users.RemoveUserAsync(user);
            var msg = res ? "Пользователь удален" : res;

            Log(msg, user);
        }

        /// <summary>
        /// Открытие логов входов.
        /// </summary>
        private void ShowLogs(User user)
        {
            dialogManager.ShowLogs(user);
        }

        /// <summary>
        /// Загрузка всех пользователей.
        /// </summary>
        protected override async void Load()
        {
            Users.Clear();
            var res = await client.Users.GetUsersAsync();
            if (res)
                Users.AddRange(res.Response);
        }

        void Log(string msg, User user)
        {
            Logger.Log.Info($"{msg}: {{{Logger.GetParamsNamesValues(() => user.Login, () => user.Mode)}}}");
            snackbarMessageQueue.Enqueue(msg);
        }
    }
}
