using DevExpress.Mvvm;
using Kursach.DataBase;
using Kursach.Dialogs;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using Prism.Regions;
using System.Windows.Input;

namespace Kursach.ViewModels
{
    class MainViewModel : ViewModelBase, INavigationAware
    {
        /// <summary>
        /// Текущий пользователь.
        /// </summary>
        public User User { get; private set; }

        /// <summary>
        /// Статус меню.
        /// </summary>
        public bool LeftMenuOpened { get; set; }

        /// <summary>
        /// Менеджер регионов.
        /// </summary>
        readonly IRegionManager regionManager;

        /// <summary>
        /// Идентификатор диалоговых окон.
        /// </summary>
        readonly IDialogIdentifier dialogIdentifier;

        /// <summary>
        /// Менеджер диалогов.
        /// </summary>
        readonly IDialogManager dialogManager;

        /// <summary>
        /// Ctor.
        /// </summary>
        public MainViewModel(IRegionManager regionManager, IDialogIdentifier dialogIdentifier, IDialogManager dialogManager)
        {
            this.regionManager = regionManager;
            this.dialogIdentifier = dialogIdentifier;
            this.dialogManager = dialogManager;

            SignUpCommand = new DelegateCommand(dialogManager.SignUp);
            OpenUsersCommand = new DelegateCommand(OpenUsers);
            ExitCommand = new DelegateCommand(Exit);
            GroupsCommand = new DelegateCommand(Groups);
        }

        /// <summary>
        /// Команда перехода на страницу групп.
        /// </summary>
        public ICommand GroupsCommand { get; }

        /// <summary>
        /// Команда открытия регистрации.
        /// </summary>
        public ICommand SignUpCommand { get; }

        /// <summary>
        /// Команда открытия базы данных пользователей.
        /// </summary>
        public ICommand OpenUsersCommand { get; }

        /// <summary>
        /// Команда выхода.
        /// </summary>
        public ICommand ExitCommand { get; }

        /// <summary>
        /// Выход.
        /// </summary>
        private async void Exit()
        {
            var res = await dialogIdentifier.ShowMessageBoxAsync("Вы действительно хотите выйти?", MaterialMessageBoxButtons.Yes | MaterialMessageBoxButtons.Cancel);
            if (res != MaterialMessageBoxButtons.Yes)
                return;

            LeftMenuOpened = false;
            Logger.Log.Info("Выход из приложения");
            regionManager.RequestNavigateInRootRegion(RegionViews.LoginView);
        }

        /// <summary>
        /// Открыть базу пользователей.
        /// </summary>
        private void OpenUsers()
        {
            regionManager.RequstNavigateInMainRegion(RegionViews.UsersView);
            LeftMenuOpened = false;
        }

        /// <summary>
        /// Переход на страницу групп.
        /// </summary>
        private void Groups()
        {
            regionManager.RequstNavigateInMainRegion(RegionViews.GroupsView);
            LeftMenuOpened = false;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (!navigationContext.Parameters.ContainsKey("fromLogin"))
                return;

            User = navigationContext.Parameters["user"] as User; ;
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
