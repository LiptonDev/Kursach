using DevExpress.Mvvm;
using Kursach.Models;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using Prism.Regions;
using System.Windows.Input;

namespace Kursach.ViewModels
{
    /// <summary>
    /// Login view model.
    /// </summary>
    class LoginViewModel : ViewModelBase
    {
        /// <summary>
        /// Пользователь для авторизации.
        /// </summary>
        public LoginUser User { get; }

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
        public LoginViewModel(IRegionManager regionManager, IDialogIdentifier dialogIdentifier)
        {
            this.regionManager = regionManager;
            this.dialogIdentifier = dialogIdentifier;

            TryLoginCommand = new DelegateCommand(TryLogin);

            User = new LoginUser();
        }

        /// <summary>
        /// Команда попытки входа в систему.
        /// </summary>
        public ICommand TryLoginCommand { get; }

        /// <summary>
        /// Попытка входа в систему.
        /// </summary>
        private void TryLogin()
        {
            if (!User.IsValid)
            {
                dialogIdentifier.ShowMessageBoxAsync(User.Error, MaterialMessageBoxButtons.Ok);
                return;
            }
        }
    }
}
