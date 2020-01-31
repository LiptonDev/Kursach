namespace Kursach.Core.Models
{
    /// <summary>
    /// Результат авторизации.
    /// </summary>
    public enum LoginResponse
    {
        /// <summary>
        /// Авторизация прошла.
        /// </summary>
        Ok,

        /// <summary>
        /// Логин или пароль не правильный.
        /// </summary>
        Invalid,

        /// <summary>
        /// Ошибка сервера.
        /// </summary>
        ServerError
    }
}
