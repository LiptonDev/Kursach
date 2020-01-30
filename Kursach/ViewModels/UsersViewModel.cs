using DevExpress.Mvvm;
using DryIoc;
using Kursach.DataBase;
using Kursach.Dialogs;
using Kursach.Models;
using Kursach.NotifyClient;
using MaterialDesignThemes.Wpf;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using System.Collections.ObjectModel;

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
        public UsersViewModel(IDataBase dataBase,
                              IDialogManager dialogManager,
                              ISnackbarMessageQueue snackbarMessageQueue,
                              INotifyClient notifyClient,
                              IContainer container)
            : base(dataBase, dialogManager, snackbarMessageQueue, notifyClient, container)
        {
            Users = new ObservableCollection<User>();

            ShowLogsCommand = new DelegateCommand<User>(ShowLogs);

            notifyClient.UserChanged += Load;
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

            var res = await dataBase.SignUpAsync(editor);
            var msg = res ? "Пользователь добавлен" : "Пользователь не добавлен";

            if (res)
            {
                Users.Add(editor);
                notifyClient.ChangeUser();
            }

            Log(msg, editor);
        }

        /// <summary>
        /// Сохранить новые данные пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        public override async void Edit(User user)
        {
            var editor = await dialogManager.SignUp(user, true);
            if (editor == null)
                return;

            var res = await dataBase.SaveUserAsync(editor);
            var msg = res ? "Пользователь сохранен" : "Пользователь не сохранен";

            if (res)
            {
                user.Login = editor.Login;
                user.Mode = editor.Mode;
                notifyClient.ChangeUser();
            }

            Log(msg, user);
        }

        /// <summary>
        /// Удаление пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        public override async void Delete(User user)
        {
            var answ = await dialogIdentifier.ShowMessageBoxAsync($"Удалить '{user.Login}'?", MaterialMessageBoxButtons.YesNo);
            if (answ != MaterialMessageBoxButtons.Yes)
                return;

            var res = await dataBase.RemoveUserAsync(user);
            var msg = res ? "Пользователь удален" : "Пользователь не удален";

            if (res)
            {
                Users.Remove(user);
                notifyClient.ChangeUser();
            }

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
            var res = await dataBase.GetUsersAsync();
            Users.AddRange(res);
        }

        void Log(string msg, User user)
        {
            Logger.Log.Info($"{msg}: {{{Logger.GetParamsNamesValues(() => user.Login, () => user.Mode)}}}");
            snackbarMessageQueue.Enqueue(msg);
        }
    }
}
