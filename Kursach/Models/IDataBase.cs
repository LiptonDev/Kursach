using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kursach.Models
{
    /// <summary>
    /// База данных.
    /// </summary>
    interface IDataBase
    {
        /// <summary>
        /// Получить пользователя.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        /// <param name="usePassword">Проверять пароль.</param>
        /// <returns></returns>
        Task<User> GetUserAsync(string login, string password, bool usePassword);

        /// <summary>
        /// Добавить лог входа в программу.
        /// </summary>
        /// <param name="user">Вошедший пользователь.</param>
        /// <returns></returns>
        Task AddSignInLogAsync(User user);

        /// <summary>
        /// Получить все логи входов.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SignInLog>> GetSignInLogsAsync();

        /// <summary>
        /// Регистрация нового пользователя.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        /// <param name="mode">Права.</param>
        /// <returns></returns>
        Task SignUpAsync(string login, string password, UserMode mode);
    }
}
