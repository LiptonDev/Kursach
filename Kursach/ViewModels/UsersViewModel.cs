using DevExpress.Mvvm;
using DryIoc;
using Kursach.Models;
using Kursach.Dialogs;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using System.Collections.ObjectModel;
using Kursach.DataBase;

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
        /// Ctor.
        /// </summary>
        public UsersViewModel(IDataBase dataBase, IDialogManager dialogManager, IContainer container)
            : base(dataBase, dialogManager, container)
        {
            Users = new ObservableCollection<User>();

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

            var res = await dataBase.SignUpAsync(editor);
            var msg = res ? "Пользователь добавлен" : "Пользователь не добавлен";

            User user = null;
            if (res)
            {
                user = await dataBase.GetUserAsync(editor.Login, null, false);
                Users.Add(user);
            }

            Log(msg, user);
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
                Users.Remove(user);

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

        async void Log(string msg, User user)
        {
            Logger.Log.Info($"{msg}: {{login: {user?.Login}, mode: {user?.Mode}}}");
            await dialogIdentifier.ShowMessageBoxAsync(msg, MaterialMessageBoxButtons.Ok);
        }
    }
}
