using DevExpress.Mvvm;
using DryIoc;
using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Dialogs;
using ISTraining_Part.Providers;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using Prism.Regions;
using System.Diagnostics;
using System.Windows.Input;

namespace ISTraining_Part.ViewModels
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
        /// Управление диалогами.
        /// </summary>
        readonly IDialogManager dialogManager;

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
        public MainViewModel(IRegionManager regionManager,
                             IClient client,
                             IDataProvider dataProvider,
                             IDialogManager dialogManager,
                             IContainer container)
        {
            this.regionManager = regionManager;
            dialogIdentifier = container.ResolveRootDialogIdentifier();
            this.client = client;
            this.dataProvider = dataProvider;
            this.dialogManager = dialogManager;

            NavigateCommand = new DelegateCommand<string>(Navigate);
            OpenChatWindowCommand = new DelegateCommand(OpenChatWindow);
            ExitCommand = new DelegateCommand(Exit);
            OpenVkCommand = new DelegateCommand(OpenVk);
        }

        /// <summary>
        /// Команда навигации.
        /// </summary>
        public ICommand<string> NavigateCommand { get; }

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
            dialogManager.ShowChatWindow();
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
            client.Login.Logout();
            Consts.LoginStatus = false;
            dialogManager.CloseChatWindow();
            regionManager.RequestNavigateInRootRegion(RegionViews.LoginView);
        }

        /// <summary>
        /// Переход на другую страницу.
        /// </summary>
        /// <param name="view">Страница.</param>
        private void Navigate(string view)
        {
            regionManager.ReqeustNavigateInMainRegion(view, NavigationParametersFluent.GetNavigationParameters().SetUser(User));
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
