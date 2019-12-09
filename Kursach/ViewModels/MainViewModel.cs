using DevExpress.Mvvm;
using DryIoc;
using Kursach.DataBase;
using Kursach.Dialogs;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using Prism.Regions;
using System.Windows.Input;

namespace Kursach.ViewModels
{
    class MainViewModel : NavigationViewModel
    {
        /// <summary>
        /// Номер слайда на странице приветствия.
        /// </summary>
        public int SlideNumber { get; set; }

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
        /// Ctor.
        /// </summary>
        public MainViewModel(IRegionManager regionManager, IContainer container, IDialogManager dialogManager)
        {
            this.regionManager = regionManager;
            dialogIdentifier = container.ResolveRootDialogIdentifier();

            SignUpCommand = new DelegateCommand(dialogManager.SignUp);
            OpenUsersCommand = new DelegateCommand(OpenUsers);
            ExitCommand = new DelegateCommand(Exit);
            GroupsCommand = new DelegateCommand(Groups);
            StaffCommand = new DelegateCommand(GoToStaff);
            HomeCommand = new DelegateCommand(Home);
        }

        /// <summary>
        /// Команда перехода на стартовую страницу.
        /// </summary>
        public ICommand HomeCommand { get; }

        /// <summary>
        /// Команда перехода на страницу сотрудников.
        /// </summary>
        public ICommand StaffCommand { get; }

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
            SlideNumber = 0;
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
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add("user", User);
            regionManager.RequstNavigateInMainRegion(RegionViews.GroupsView, parameters);
            LeftMenuOpened = false;
        }

        /// <summary>
        /// Переход на страницу сотрудников.
        /// </summary>
        private void GoToStaff()
        {
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add("user", User);
            regionManager.RequstNavigateInMainRegion(RegionViews.StaffView, parameters);
            LeftMenuOpened = false;
        }

        /// <summary>
        /// Переход на стартовую страницу.
        /// </summary>
        private void Home()
        {
            regionManager.RequstNavigateInMainRegion(RegionViews.WelcomeView);
            LeftMenuOpened = false;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (!navigationContext.Parameters.ContainsKey("fromLogin"))
                return;

            User = navigationContext.Parameters["user"] as User;
        }
    }
}
