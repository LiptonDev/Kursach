using Kursach.DataBase;

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
    }
}
