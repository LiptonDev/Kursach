using Kursach.Models;
using MaterialDesignXaml.DialogsHelper;
using System.Threading.Tasks;

namespace Kursach.Dialogs
{
    /// <summary>
    /// Менеджер диалогов.
    /// </summary>
    interface IDialogManager
    {
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
        Task<Group> GroupEditor(Group group, bool isEditMode);

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
        Task<Student> StudentEditor(Student student, bool isEditMode);
    }
}
