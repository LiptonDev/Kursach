using DevExpress.Mvvm;
using DryIoc;
using Kursach.DataBase;
using Kursach.Dialogs;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Kursach.ViewModels
{
    /// <summary>
    /// Users view model.
    /// </summary>
    class UsersViewModel : ViewModelBase, INavigationAware
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
        }

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
        /// Сохранить новые данные пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        private async Task SaveUser(User user)
        {
            if (!user.IsValid)
                return;

            var res = await dataBase.SaveUserAsync(user);

            await dialogIdentifier.ShowMessageBoxAsync(res ? "Пользователь сохранен" : "Пользователь не сохранен", MaterialMessageBoxButtons.Ok);
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

            await dialogIdentifier.ShowMessageBoxAsync(msg, MaterialMessageBoxButtons.Ok);

            Logger.Log.Info($"{msg}: {{login: {user.Login}, mode: {user.Mode}}}");

            if (res)
                Users.Remove(user);
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

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Load();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
