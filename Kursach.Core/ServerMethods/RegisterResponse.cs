namespace Kursach.Core.ServerMethods
{
    /// <summary>
    /// Результат регистрации.
    /// </summary>
    public enum RegisterResponse
    {
        /// <summary>
        /// Регистрация прошла.
        /// </summary>
        Ok,

        /// <summary>
        /// Логин занят.
        /// </summary>
        Exist
    }
}