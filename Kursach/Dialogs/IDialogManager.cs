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
        void ShowLogs();

        /// <summary>
        /// Окно регистрации нового пользователя.
        /// </summary>
        void SignUp();

        /// <summary>
        /// Окно сброса пароля.
        /// </summary>
        void ResetPassword();
    }
}
