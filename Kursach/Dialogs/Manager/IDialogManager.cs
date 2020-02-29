using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.Models.Enums;
using MaterialDesignXaml.DialogsHelper;
using System.Threading.Tasks;

namespace ISTraining_Part.Dialogs.Manager
{
    /// <summary>
    /// Менеджер диалогов.
    /// </summary>
    interface IDialogManager
    {
        /// <summary>
        /// Открыть окно чата.
        /// </summary>
        void ShowChatWindow();

        /// <summary>
        /// Закрыть окно чата.
        /// </summary>
        void CloseChatWindow();

        /// <summary>
        /// Выбор файла для экспорта данных.
        /// </summary>
        /// <param name="defName">Изначальное название файла.</param>
        /// <returns></returns>
        string SelectExportFileName(string defName);

        /// <summary>
        /// Выбор файла для импорта данных.
        /// </summary>
        /// <returns></returns>
        string SelectImportFile();

        /// <summary>
        /// Окно редактирования детальной информации.
        /// </summary>
        /// <param name="id">ИД человека.</param>
        /// <param name="type">Тип человека.</param>
        /// <returns></returns>
        Task<DetailInfo> ShowDetailInfoEditor(int id, DetailInfoType type);

        /// <summary>
        /// Окно детальной информации.
        /// </summary>
        /// <param name="id">ИД человека.</param>
        /// <param name="type">Тип человека.</param>
        /// <returns></returns>
        Task ShowDetailInfo(int id, DetailInfoType type);

        /// <summary>
        /// Окно логов входа.
        /// </summary>
        void ShowLogs(User user);

        /// <summary>
        /// Окно редактирования пользователя.
        /// </summary>
        Task<User> SignUp(User user, bool isEditMode);

        /// <summary>
        /// Окно редактирования группы.
        /// </summary>
        /// <returns></returns>
        Task<Group> GroupEditor(Group group, bool isEditMode, int division);

        /// <summary>
        /// Окно выбора куратора.
        /// </summary>
        /// <returns></returns>
        Task<Staff> SelectStaff(int currentId, IDialogIdentifier dialogIdentifier);

        /// <summary>
        /// Окно редактирования сотрудника.
        /// </summary>
        /// <returns></returns>
        Task<Staff> StaffEditor(Staff staff, bool isEditMode);

        /// <summary>
        /// Окно редактирования студента.
        /// </summary>
        /// <returns></returns>
        Task<Student> StudentEditor(Student student, bool isEditMode, int groupId);
    }
}
