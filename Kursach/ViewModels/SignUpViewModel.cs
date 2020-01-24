using DevExpress.Mvvm;
using DryIoc;
using Kursach.DataBase;
using Kursach.Models;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using System.Windows.Input;

namespace Kursach.ViewModels
{
    /// <summary>
    /// Signup view model.
    /// </summary>
    class SignUpViewModel : ViewModelBase, IDialogIdentifier, IClosableDialog
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public string Identifier => nameof(SignUpViewModel);

        /// <summary>
        /// Owner.
        /// </summary>
        public IDialogIdentifier OwnerIdentifier { get; }

        /// <summary>
        /// Пользователь для регистрации.
        /// </summary>
        public LoginUser User { get; }

        /// <summary>
        /// Права пользователя.
        /// </summary>
        public UserMode Mode { get; set; } = UserMode.Read;

        /// <summary>
        /// Ctor.
        /// </summary>
        public SignUpViewModel(IContainer container)
        {
            OwnerIdentifier = container.ResolveRootDialogIdentifier();

            User = new LoginUser();

            CloseDialogCommand = new DelegateCommand(CloseDialog);
        }

        /// <summary>
        /// Команда закрытия диалога.
        /// </summary>
        public ICommand CloseDialogCommand { get; }

        /// <summary>
        /// Регистрация.
        /// </summary>
        private async void CloseDialog()
        {
            if (!User.IsValid)
            {
                await this.ShowMessageBoxAsync(User.Error, MaterialMessageBoxButtons.Ok);
                return;
            }

            OwnerIdentifier.Close(new SignUpResult(User, Mode));
        }
    }
}
