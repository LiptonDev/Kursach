using DevExpress.Mvvm;
using DryIoc;
using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Dialogs;
using ISTraining_Part.Providers;
using MaterialDesignThemes.Wpf;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ISTraining_Part.ViewModels
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
                              IDataProvider dataProvider,
                              IContainer container)
            : base(dialogManager, snackbarMessageQueue, client, dataProvider, container)
        {
            Users = dataProvider.Users;

            ShowLogsCommand = new DelegateCommand<User>(ShowLogs);
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

        void Log(string msg, User user)
        {
            Logger.Log.Info($"{msg}: {{login: {user.Login}, mode: {user.Mode}}}");
            snackbarMessageQueue.Enqueue(msg);
        }
    }
}
