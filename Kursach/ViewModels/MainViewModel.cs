using DevExpress.Mvvm;
using DryIoc;
using Kursach.Client.Interfaces;
using Kursach.Core.Models;
using Kursach.Providers;
using Kursach.Views;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using Prism.Regions;
using System;
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
        /// Клиент.
        /// </summary>
        readonly IClient client;

        /// <summary>
        /// Поставщик данных.
        /// </summary>
        readonly IDataProvider dataProvider;

        /// <summary>
        /// Контейнер.
        /// </summary>
        readonly IContainer container;

        /// <summary>
        /// Конструктор для DesignTime.
        /// </summary>
        public MainViewModel()
        {
            User = new User
            {
                Login = "DESIGN TIME USER",
                Mode = UserMode.Admin
            };
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public MainViewModel(IRegionManager regionManager, IClient client, IDataProvider dataProvider, IContainer container)
        {
            this.regionManager = regionManager;
            dialogIdentifier = container.ResolveRootDialogIdentifier();
            this.client = client;
            this.dataProvider = dataProvider;
            this.container = container;

            OpenUsersCommand = new DelegateCommand<string>(Navigate);
            GroupsCommand = new DelegateCommand<string>(Navigate);
            StaffCommand = new DelegateCommand<string>(Navigate);
            StudentsCommand = new DelegateCommand<string>(Navigate);
            HomeCommand = new DelegateCommand<string>(Navigate);
            OpenChatWindowCommand = new DelegateCommand(OpenChatWindow);
            ExitCommand = new DelegateCommand(Exit);
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
        /// Команда открытия окна чата.
        /// </summary>
        public ICommand OpenChatWindowCommand { get; }

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
        /// Открыть окно чата.
        /// </summary>
        private void OpenChatWindow()
        {
            container.Resolve<ChatWindow>().Show();
            LeftMenuOpened = false;
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
            client.Login.Logout();
            Consts.LoginStatus = false;
        }

        /// <summary>
        /// Переход на другую страницу.
        /// </summary>
        /// <param name="view">Страница.</param>
        private void Navigate(string view)
        {
            regionManager.ReqeustNavigateInMainRegion(view);
            LeftMenuOpened = false;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            LeftMenuOpened = false;

            if (!navigationContext.Parameters.ContainsKey("fromLogin"))
                return;

            base.OnNavigatedTo(navigationContext);

            dataProvider.Load(User.Mode);
        }
    }
}
