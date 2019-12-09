using Kursach.DataBase;
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
        /// Окно логов входа.
        /// </summary>
        void ShowLogs(User user);

        /// <summary>
        /// Окно регистрации нового пользователя.
        /// </summary>
        void SignUp();

        /// <summary>
        /// Окно редактирования группы.
        /// </summary>
        /// <returns></returns>
        Task<Group> GroupEditor(Group group);

        /// <summary>
        /// Окно выбора куратора.
        /// </summary>
        /// <returns></returns>
        Task<Staff> SelectStaff(Staff currentStaff, IDialogIdentifier dialogIdentifier);
    }
}
