using DryIoc;
using Kursach.DataBase;
using Kursach.ViewModels;
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
        /// View factory.
        /// </summary>
        readonly IViewFactory viewFactory;

        /// <summary>
        /// Ctor.
        /// </summary>
        public DialogManager(IDialogIdentifier dialogIdentifier, IContainer container, IViewFactory viewFactory)
        {
            this.dialogIdentifier = dialogIdentifier;
            this.container = container;
            this.viewFactory = viewFactory;
        }

        /// <summary>
        /// Окно логов входа.
        /// </summary>
        public void ShowLogs(User user)
        {
            var vm = container.Resolve<SignInLogsViewModel>(args: new[] { user });
            var view = viewFactory.GetView(vm);

            dialogIdentifier.ShowAsync(view);
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
