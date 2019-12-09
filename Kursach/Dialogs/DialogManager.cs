using DryIoc;
using Kursach.DataBase;
using Kursach.Models;
using Kursach.ViewModels;
using Kursach.Views;
using MaterialDesignXaml.DialogsHelper;
using System.Threading.Tasks;

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
        public DialogManager(IContainer container, IViewFactory viewFactory)
        {
            this.dialogIdentifier = container.ResolveRootDialogIdentifier();
            this.container = container;
            this.viewFactory = viewFactory;
        }

        /// <summary>
        /// Окно выбора куратора.
        /// </summary>
        /// <returns></returns>
        public async Task<Staff> SelectStaff(int currentId, IDialogIdentifier dialogIdentifier)
        {
            var vm = container.Resolve<SelectStaffViewModel>(args: new object[] { currentId, dialogIdentifier });
            var view = viewFactory.GetView(vm);

            var res = await dialogIdentifier.ShowAsync<Staff>(view);

            return res;
        }

        /// <summary>
        /// Окно редактирования группы.
        /// </summary>
        /// <returns></returns>
        public async Task<Group> GroupEditor(Group group, bool isEditMode)
        {
            var vm = container.Resolve<GroupEditorViewModel>(args: new object[] { group, isEditMode });
            var view = viewFactory.GetView(vm);

            var res = await dialogIdentifier.ShowAsync<Group>(view);

            return res;
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
