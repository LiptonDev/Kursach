using DevExpress.Mvvm;
using DryIoc;
using Kursach.DataBase;
using Kursach.Dialogs;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kursach.ViewModels
{
    /// <summary>
    /// Users view model.
    /// </summary>
    class UsersViewModel : NavigationViewModel
    {
        /// <summary>
        /// Пользователи.
        /// </summary>
        public ObservableCollection<User> Users { get; }

        /// <summary>
        /// База данных.
        /// </summary>
        readonly IDataBase dataBase;

        /// <summary>
        /// Менеджер диалогов.
        /// </summary>
        readonly IDialogManager dialogManager;

        /// <summary>
        /// Идентификатор диалогов.
        /// </summary>
        readonly IDialogIdentifier dialogIdentifier;

        /// <summary>
        /// Ctor.
        /// </summary>
        public UsersViewModel(IDataBase dataBase, IDialogManager dialogManager, IContainer container)
        {
            this.dataBase = dataBase;
            this.dialogManager = dialogManager;
            this.dialogIdentifier = container.ResolveRootDialogIdentifier();

            Users = new ObservableCollection<User>();

            ShowLogsCommand = new DelegateCommand<User>(ShowLogs);
            DeleteUserCommand = new AsyncCommand<User>(DeleteUser);
            SaveUserCommand = new AsyncCommand<User>(SaveUser);
            SignUpCommand = new DelegateCommand(SignUp);
        }

        /// <summary>
        /// Команда открытия окна регистрации.
        /// </summary>
        public ICommand SignUpCommand { get; }

        /// <summary>
        /// Команда сохранения изменения пользователя.
        /// </summary>
        public ICommand<User> SaveUserCommand { get; }

        /// <summary>
        /// Команда удаления пользователя.
        /// </summary>
        public ICommand<User> DeleteUserCommand { get; }

        /// <summary>
        /// Команда открытия логов входов.
        /// </summary>
        public ICommand<User> ShowLogsCommand { get; }

        /// <summary>
        /// Открыть окно регистрации.
        /// </summary>
        private async void SignUp()
        {
            var editor = await dialogManager.SignUp();
            if (editor == null)
                return;

            var res = await dataBase.SignUpAsync(editor.User, editor.Mode);
            var msg = res ? "Пользователь добавлен" : "Пользователь не добавлен";

            User user = null;
            if (res)
            {
                user = await dataBase.GetUserAsync(editor.User.Login, null, false);
                Users.Add(user);
            }

            Log(msg, user);
        }

        /// <summary>
        /// Сохранить новые данные пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        private async Task SaveUser(User user)
        {
            if (!user.IsValid)
                return;

            var res = await dataBase.SaveUserAsync(user);
            var msg = res ? "Пользователь сохранен" : "Пользователь не сохранен";

            Log(msg, user);
        }

        /// <summary>
        /// Удаление пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        private async Task DeleteUser(User user)
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

        async void Log(string msg, User user)
        {
            Logger.Log.Info($"{msg}: {{login: {user.Login}, mode: {user.Mode}}}");
            await dialogIdentifier.ShowMessageBoxAsync(msg, MaterialMessageBoxButtons.Ok);
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
        private async void Load()
        {
            Users.Clear();
            var res = await dataBase.GetUsersAsync();
            Users.AddRange(res);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            Load();
        }
    }
}
