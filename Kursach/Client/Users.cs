using Kursach.Core;
using Kursach.Core.Models;
using Kursach.Core.ServerEvents;
using Microsoft.AspNet.SignalR.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kursach.Client
{
    /// <summary>
    /// Управление пользователями.
    /// </summary>
    class Users : IUsers
    {
        /// <summary>
        /// Прокси.
        /// </summary>
        IHubProxy proxy;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Users(IHubConfigurator hubConfigurator, TaskFactory sync)
        {
            proxy = hubConfigurator.Hub.CreateHubProxy(HubNames.UsersHub);

            proxy.On<DbChangeStatus, User>(nameof(UsersEvents.UserChanged),
                (status, user) => sync.StartNew(() => OnChanged?.Invoke(status, user)));
        }

        /// <summary>
        /// Изменения пользователя.
        /// </summary>
        public event OnChanged<User> OnChanged;

        #region Get region
        /// <summary>
        /// Получение списка всех пользователей.
        /// </summary>
        /// <returns></returns>
        public Task<KursachResponse<IEnumerable<User>>> GetUsersAsync()
        {
            return proxy.TryInvokeAsync<IEnumerable<User>>();
        }

        /// <summary>
        /// Получить пользователя.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        /// <param name="usePassword">Проверять пароль.</param>
        /// <returns></returns>
        public Task<KursachResponse<User>> GetUserAsync(string login, string password, bool usePassword)
        {
            return proxy.TryInvokeAsync<User>(args: new object[] { login, password, usePassword });
        }
        #endregion

        #region Log region
        /// <summary>
        /// Получить логи входов пользователя.
        /// </summary>
        /// <param name="userId">ИД пользователя.</param>
        /// <returns></returns>
        public Task<KursachResponse<IEnumerable<SignInLog>>> GetSignInLogsAsync(int userId)
        {
            return proxy.TryInvokeAsync<IEnumerable<SignInLog>>(args: userId);
        }
        #endregion

        #region CUD region
        /// <summary>
        /// Регистрация нового пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        public Task<KursachResponse<bool>> AddUserAsync(User user)
        {
            return proxy.TryInvokeAsync<bool>(args: user);
        }

        /// <summary>
        /// Сохранить пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        public Task<KursachResponse<bool>> SaveUserAsync(User user)
        {
            return proxy.TryInvokeAsync<bool>(args: user);
        }

        /// <summary>
        /// Удаление пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        public Task<KursachResponse<bool>> RemoveUserAsync(User user)
        {
            return proxy.TryInvokeAsync<bool>(args: user);
        }
        #endregion
    }
}
