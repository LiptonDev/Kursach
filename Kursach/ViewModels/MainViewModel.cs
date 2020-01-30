using DevExpress.Mvvm;
using DryIoc;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using Prism.Regions;
using System.Diagnostics;
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
        /// Конструктор для DesignTime.
        /// </summary>
        public MainViewModel()
        {
            User = new Models.User
            {
                Login = "DESIGN TIME USER",
                Mode = Models.UserMode.Admin
            };
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public MainViewModel(IRegionManager regionManager, IContainer container)
        {
            this.regionManager = regionManager;
            dialogIdentifier = container.ResolveRootDialogIdentifier();

            OpenUsersCommand = new DelegateCommand(OpenUsers);
            ExitCommand = new DelegateCommand(Exit);
            GroupsCommand = new DelegateCommand(Groups);
            StaffCommand = new DelegateCommand(GoToStaff);
            StudentsCommand = new DelegateCommand(Students);
            HomeCommand = new DelegateCommand(Home);
            OpenVkCommand = new DelegateCommand(OpenVk);
        }

        /// <summary>
        /// Команда перехода на стартовую страницу.
        /// </summary>
        public ICommand HomeCommand { get; }

        /// <summary>
        /// Команда перехода на страницу студентов.
        /// </summary>
        public ICommand StudentsCommand { get; }

        /// <summary>
        /// Команда перехода на страницу сотрудников.
        /// </summary>
        public ICommand StaffCommand { get; }

        /// <summary>
        /// Команда перехода на страницу групп.
        /// </summary>
        public ICommand GroupsCommand { get; }

        /// <summary>
        /// Команда открытия базы данных пользователей.
        /// </summary>
        public ICommand OpenUsersCommand { get; }

        /// <summary>
        /// Команда выхода.
        /// </summary>
        public ICommand ExitCommand { get; }

        /// <summary>
        /// Открыть мой ВК.
        /// </summary>
        public ICommand OpenVkCommand { get; }

        /// <summary>
        /// Открыть мой ВК.
        /// </summary>
        private void OpenVk()
        {
            Process.Start("https://vk.com/id99551920");
        }

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
            regionManager.ReqeustNavigateInMainRegion(RegionViews.UsersView);
            LeftMenuOpened = false;
        }

        /// <summary>
        /// Переход на страницу групп.
        /// </summary>
        private void Groups()
        {
            regionManager.RequstNavigateInMainRegionWithUser(RegionViews.GroupsView, User);
            LeftMenuOpened = false;
        }

        /// <summary>
        /// Переход на страницу сотрудников.
        /// </summary>
        private void GoToStaff()
        {
            regionManager.RequstNavigateInMainRegionWithUser(RegionViews.StaffView, User);
            LeftMenuOpened = false;
        }

        /// <summary>
        /// Переход на страницу студентов.
        /// </summary>
        private void Students()
        {
            regionManager.RequstNavigateInMainRegionWithUser(RegionViews.StudentsView, User);
            LeftMenuOpened = false;
        }

        /// <summary>
        /// Переход на стартовую страницу.
        /// </summary>
        private void Home()
        {
            regionManager.ReqeustNavigateInMainRegion(RegionViews.WelcomeView);
            LeftMenuOpened = false;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (!navigationContext.Parameters.ContainsKey("fromLogin"))
                return;

            base.OnNavigatedTo(navigationContext);
        }
    }
}
