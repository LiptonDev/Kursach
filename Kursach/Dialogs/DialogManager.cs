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

        async Task<T> show<T, VM>(object[] args = null, IDialogIdentifier dialogIdentifier = null)
        {
            var vm = container.Resolve<VM>(args: args);
            var view = viewFactory.GetView(vm);

            dialogIdentifier = dialogIdentifier ?? this.dialogIdentifier;

            var res = await dialogIdentifier.ShowAsync<T>(view);

            return res;
        }

        /// <summary>
        /// Окно выбора группы.
        /// </summary>
        /// <returns></returns>
        public async Task<Group> SelectGroup(int currentId, IDialogIdentifier dialogIdentifier)
        {
            return await show<Group, SelectGroupViewModel>(new object[] { currentId, dialogIdentifier }, dialogIdentifier);
        }

        /// <summary>
        /// Окно редактирования студента.
        /// </summary>
        /// <returns></returns>
        public async Task<Student> StudentEditor(Student student, bool isEditMode)
        {
            return await show<Student, StudentEditorViewModel>(new object[] { student, isEditMode });
        }

        /// <summary>
        /// Окно редактирования сотрудника.
        /// </summary>
        /// <returns></returns>
        public async Task<Staff> StaffEditor(Staff staff, bool isEditMode)
        {
            return await show<Staff, StaffEditorViewModel>(new object[] { staff, isEditMode });
        }

        /// <summary>
        /// Окно выбора куратора.
        /// </summary>
        /// <returns></returns>
        public async Task<Staff> SelectStaff(int currentId, IDialogIdentifier dialogIdentifier)
        {
            return await show<Staff, SelectStaffViewModel>(new object[] { currentId, dialogIdentifier }, dialogIdentifier);
        }

        /// <summary>
        /// Окно редактирования группы.
        /// </summary>
        /// <returns></returns>
        public async Task<Group> GroupEditor(Group group, bool isEditMode)
        {
            return await show<Group, GroupEditorViewModel>(new object[] { group, isEditMode });
        }

        /// <summary>
        /// Окно логов входа.
        /// </summary>
        public async void ShowLogs(User user)
        {
            await show<string, SignInLogsViewModel>(new object[] { user });
        }

        /// <summary>
        /// Окно регистрации нового пользователя.
        /// </summary>
        public async Task<SignUpResult> SignUp()
        {
            return await dialogIdentifier.ShowAsync<SignUpResult>(container.Resolve<SignUpView>());
        }
    }
}
