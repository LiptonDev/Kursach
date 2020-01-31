using Kursach.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kursach.Core.ServerMethods
{
    /// <summary>
    /// Список методов хаба пользователей.
    /// </summary>
    public interface UsersMethods
    {
        #region Get region
        /// <summary>
        /// Получение списка всех пользователей.
        /// </summary>
        /// <returns></returns>
        Task<KursachResponse<IEnumerable<User>>> GetUsersAsync();

        /// <summary>
        /// Получить пользователя.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        /// <param name="usePassword">Проверять пароль.</param>
        /// <returns></returns>
        Task<KursachResponse<User>> GetUserAsync(string login, string password, bool usePassword);
        #endregion

        #region Log region
        /// <summary>
        /// Получить логи входов пользователя.
        /// </summary>
        /// <param name="userId">ИД пользователя.</param>
        /// <returns></returns>
        Task<KursachResponse<IEnumerable<SignInLog>>> GetSignInLogsAsync(int userId);
        #endregion

        #region CUD region
        /// <summary>
        /// Регистрация нового пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        Task<KursachResponse<bool>> AddUserAsync(User user);

        /// <summary>
        /// Сохранить пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        Task<KursachResponse<bool>> SaveUserAsync(User user);

        /// <summary>
        /// Удаление пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        Task<KursachResponse<bool>> RemoveUserAsync(User user);
        #endregion
    }
}
