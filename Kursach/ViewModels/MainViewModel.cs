using DevExpress.Mvvm;
using Kursach.DataBase;
using Kursach.Dialogs;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using Prism.Regions;
using System.Windows.Input;

namespace Kursach.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Текущий пользователь.
        /// </summary>
        public User User { get; }

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
        public MainViewModel(IRegionManager regionManager, IDialogIdentifier dialogIdentifier, IDialogManager dialogManager, User me)
        {
            this.regionManager = regionManager;
            this.dialogIdentifier = dialogIdentifier;
            this.dialogManager = dialogManager;
            User = me;

            OpenUsersCommand = new DelegateCommand(OpenUsers);
            ExitCommand = new DelegateCommand(Exit);
        }

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
    }
}
