using DryIoc;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.Models.Enums;
using ISTraining_Part.Views;
using MaterialDesignXaml.DialogsHelper;
using Microsoft.Win32;
using System.Threading.Tasks;

namespace ISTraining_Part.Dialogs.Manager
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

        bool chatWindowOpened = false;
        ChatWindow currentChatWindow;

        /// <summary>
        /// Закрыть окно чата.
        /// </summary>
        public void CloseChatWindow()
        {
            if (!chatWindowOpened)
                return;

            currentChatWindow.Close();
        }

        /// <summary>
        /// Открыть окно чата.
        /// </summary>
        public void ShowChatWindow()
        {
            if (chatWindowOpened)
                return;

            currentChatWindow = container.Resolve<ChatWindow>();
            currentChatWindow.Closed += WindowClosed;
            currentChatWindow.Show();
            chatWindowOpened = true;
        }

        /// <summary>
        /// Окно чата закрыто.
        /// </summary>
        void WindowClosed(object sender, object e)
        {
            chatWindowOpened = false;
            currentChatWindow.Closed -= WindowClosed;
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
                Filter = "Excel files|*.xlsx;*xls"
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
                Filter = "Excel files|*.xlsx;*xls",
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
        /// Окно редактирования детальной информации.
        /// </summary>
        /// <param name="id">ИД человека.</param>
        /// <param name="type">Тип человека.</param>
        /// <returns></returns>
        public Task<DetailInfo> ShowDetailInfoEditor(int id, DetailInfoType type)
        {
            return show<DetailInfo, DetailInfoEditorViewModel>(new object[] { id, type });
        }

        /// <summary>
        /// Окно детальной информации.
        /// </summary>
        /// <param name="id">ИД человека.</param>
        /// <param name="type">Тип человека.</param>
        /// <returns></returns>
        public Task ShowDetailInfo(int id, DetailInfoType type)
        {
            return show<DetailInfo, DetailInfoViewModel>(new object[] { id, type });
        }

        /// <summary>
        /// Окно редактирования студента.
        /// </summary>
        /// <returns></returns>
        public Task<Student> StudentEditor(Student student, bool isEditMode, int groupId)
        {
            return show<Student, StudentEditorViewModel>(new object[] { student, isEditMode, groupId });
        }

        /// <summary>
        /// Окно редактирования сотрудника.
        /// </summary>
        /// <returns></returns>
        public Task<Staff> StaffEditor(Staff staff, bool isEditMode)
        {
            return show<Staff, StaffEditorViewModel>(new object[] { staff, isEditMode });
        }

        /// <summary>
        /// Окно выбора куратора.
        /// </summary>
        /// <returns></returns>
        public Task<Staff> SelectStaff(int currentId, IDialogIdentifier dialogIdentifier)
        {
            return show<Staff, SelectStaffViewModel>(new object[] { currentId, dialogIdentifier }, dialogIdentifier);
        }

        /// <summary>
        /// Окно редактирования группы.
        /// </summary>
        /// <returns></returns>
        public Task<Group> GroupEditor(Group group, bool isEditMode, int division)
        {
            return show<Group, GroupEditorViewModel>(new object[] { group, isEditMode, division });
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
        public Task<User> SignUp(User user, bool isEditMode)
        {
            return show<User, SignUpViewModel>(new object[] { user, isEditMode });
        }
    }
}
