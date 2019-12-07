using DryIoc;
using Kursach.Views;
using MaterialDesignXaml.DialogsHelper;

namespace Kursach.Dialogs
{
    /// <summary>
    /// Менеджер диалогов.
    /// </summary>
    class DialogManager : IDialogManager
    {
        /// <summary>
        /// Идентификатор диалоговых окон.
        /// </summary>
        readonly IDialogIdentifier dialogIdentifier;

        /// <summary>
        /// Контейнер.
        /// </summary>
        readonly IContainer container;

        /// <summary>
        /// Ctor.
        /// </summary>
        public DialogManager(IDialogIdentifier dialogIdentifier, IContainer container)
        {
            this.dialogIdentifier = dialogIdentifier;
            this.container = container;
        }

        /// <summary>
        /// Окно логов входа.
        /// </summary>
        public void ShowLogs()
        {
            dialogIdentifier.ShowAsync(container.Resolve<SignInLogsView>());
        }

        /// <summary>
        /// Окно регистрации нового пользователя.
        /// </summary>
        public void SignUp()
        {
            dialogIdentifier.ShowAsync(container.Resolve<SignUpView>());
        }
    }
}
