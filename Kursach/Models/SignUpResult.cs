using Kursach.DataBase;

namespace Kursach.Models
{
    /// <summary>
    /// Результат регистрации.
    /// </summary>
    class SignUpResult
    {
        /// <summary>
        /// Пользователь.
        /// </summary>
        public LoginUser User { get; }

        /// <summary>
        /// Права.
        /// </summary>
        public UserMode Mode { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        public SignUpResult(LoginUser user, UserMode mode)
        {
            User = user;
            Mode = mode;
        }
    }
}
