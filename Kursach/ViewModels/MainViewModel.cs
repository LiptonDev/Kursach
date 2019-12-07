using DevExpress.Mvvm;
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

            ExitCommand = new DelegateCommand(Exit);
            GetLogsCommand = new DelegateCommand(dialogManager.ShowLogs);
            SignUpCommand = new DelegateCommand(dialogManager.SignUp);
        }

        /// <summary>
        /// Команда открытия окна регистрации нового пользователя.
        /// </summary>
        public ICommand SignUpCommand { get; }

        /// <summary>
        /// Команда открытия окна получения логов.
        /// </summary>
        public ICommand GetLogsCommand { get; }

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
            regionManager.RequestNavigateInRootRegion(RegionViews.LoginView);
        }
    }
}
