using Kursach.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kursach.DataBase
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
        Task<bool> AddSignInLogAsync(User user);

        /// <summary>
        /// Получить логи входов пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        Task<IEnumerable<SignInLog>> GetSignInLogsAsync(User user);

        /// <summary>
        /// Регистрация нового пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <param name="mode">Права.</param>
        /// <returns></returns>
        Task<bool> SignUpAsync(LoginUser user, UserMode mode);

        /// <summary>
        /// Получение списка всех пользователей.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<User>> GetUsersAsync();

        /// <summary>
        /// Удаление пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        Task<bool> RemoveUserAsync(User user);

        /// <summary>
        /// Сохранить пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        Task<bool> SaveUserAsync(User user);

        /// <summary>
        /// Получение всех групп.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Group>> GetGroupsAsync();

        /// <summary>
        /// Удаление группы.
        /// </summary>
        /// <param name="group">Группа.</param>
        /// <returns></returns>
        Task<bool> RemoveGroupAsync(Group group);

        /// <summary>
        /// Получение всех работников.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Staff>> GetStaffsAsync();

        /// <summary>
        /// Сохранить группу.
        /// </summary>
        /// <param name="group">Группа.</param>
        /// <returns></returns>
        Task<bool> SaveGroupAsync(Group group);

        /// <summary>
        /// Добавить группу.
        /// </summary>
        /// <param name="group">Группа.</param>
        /// <returns></returns>
        Task<bool> AddGroupAsync(Group group);

    }
}
