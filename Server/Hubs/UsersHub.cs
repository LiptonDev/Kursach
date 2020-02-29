using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.ServerEvents;
using ISTraining_Part.Core.ServerMethods;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Server.DataBase.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Hubs
{
    /// <summary>
    /// Хаб пользователей.
    /// </summary>
    [AuthorizeUser]
    [AdminHub]
    [HubName(HubNames.UsersHub)]
    public class UsersHub : Hub<IUsersHubEvents>, IUsersHub
    {
        /// <summary>
        /// Репозиторий пользователей.
        /// </summary>
        readonly IUsersRepository repository;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public UsersHub(IUsersRepository repository)
        {
            this.repository = repository;
        }

        #region Get region
        /// <summary>
        /// Получение списка всех пользователей.
        /// </summary>
        /// <returns></returns>
        public Task<KursachResponse<IEnumerable<User>>> GetUsersAsync()
        {
            return repository.GetUsersAsync();
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
            return repository.GetUserAsync(login, password, usePassword);
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
            return repository.GetSignInLogsAsync(userId);
        }
        #endregion

        #region CUD region
        /// <summary>
        /// Регистрация нового пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        public async Task<KursachResponse<bool>> AddUserAsync(User user)
        {
            Logger.Log.Info($"Add user: {user.Login}");

            var res = await repository.AddUserAsync(user);

            if (res)
                Clients.Group(Consts.AdminGroup).OnChanged(DbChangeStatus.Add, user);

            return res;
        }

        /// <summary>
        /// Сохранить пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        public async Task<KursachResponse<bool>> SaveUserAsync(User user)
        {
            Logger.Log.Info($"Save user: {user.Login}");

            var res = await repository.SaveUserAsync(user);

            if (res)
                Clients.Group(Consts.AdminGroup).OnChanged(DbChangeStatus.Update, user);

            return res;
        }

        /// <summary>
        /// Удаление пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        public async Task<KursachResponse<bool>> RemoveUserAsync(User user)
        {
            Logger.Log.Info($"Remove user: {user.Login}");

            var res = await repository.RemoveUserAsync(user);

            if (res)
                Clients.Group(Consts.AdminGroup).OnChanged(DbChangeStatus.Remove, user);

            return res;
        }
        #endregion
    }
}
