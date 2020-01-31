using DryIoc;
using Kursach.Core.Models;
using MaterialDesignXaml.DialogsHelper;
using Microsoft.Win32;
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
        readonly IDialogsFactoryView viewFactory;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public DialogManager(IContainer container, IDialogsFactoryView viewFactory)
        {
            this.dialogIdentifier = container.ResolveRootDialogIdentifier();
            this.container = container;
            this.viewFactory = viewFactory;
        }

        /// <summary>
        /// Выбор файла для импорт данных.
        /// </summary>
        /// <returns></returns>
        public string SelectImportFile()
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Выберите файл группы для импорта",
                Filter = "xlsx files (*.xlsx)|*.xlsx"
            };

            if (ofd.ShowDialog() == true)
                return ofd.FileName;
            else return null;
        }

        /// <summary>
        /// Выбор файла для экспорта данных.
        /// </summary>
        /// <param name="defName">Изначальное название файла.</param>
        /// <returns></returns>
        public string SelectExportFileName(string defName)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Title = "Введите имя файла для сохранения",
                Filter = "xlsx files (*.xlsx)|*.xlsx",
                FileName = defName
            };

            if (sfd.ShowDialog() == true)
                return sfd.FileName;
            else return null;
        }

        /// <summary>
        /// Открыть диалог.
        /// </summary>
        /// <param name="args">Аргументы для VM.</param>
        /// <param name="dialogIdentifier">Идентификатор, где будет показан диалог.</param>
        /// <returns></returns>
        async Task<T> show<T, VM>(object[] args = null, IDialogIdentifier dialogIdentifier = null)
        {
            var vm = container.Resolve<VM>(args: args);
            var view = viewFactory.GetView(vm);

            dialogIdentifier = dialogIdentifier ?? this.dialogIdentifier;

            var res = await dialogIdentifier.ShowAsync<T>(view);

            return res;
        }

        /// <summary>
        /// Окно редактирования студента.
        /// </summary>
        /// <returns></returns>
        public async Task<Student> StudentEditor(Student student, bool isEditMode, int groupId)
        {
            return await show<Student, StudentEditorViewModel>(new object[] { student, isEditMode, groupId });
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
        public async Task<Group> GroupEditor(Group group, bool isEditMode, int division)
        {
            return await show<Group, GroupEditorViewModel>(new object[] { group, isEditMode, division });
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
        public async Task<User> SignUp(User user, bool isEditMode)
        {
            return await show<User, SignUpViewModel>(new object[] { user, isEditMode });
        }
    }
}
