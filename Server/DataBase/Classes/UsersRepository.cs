using Dapper;
using Dapper.Contrib.Extensions;
using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using Server.DataBase.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.DataBase.Classes
{
    /// <summary>
    /// Репозиторий пользователей.
    /// </summary>
    class UsersRepository : IUsersRepository
    {
        /// <summary>
        /// Репозиторий.
        /// </summary>
        readonly IRepository repository;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public UsersRepository(IRepository repository)
        {
            this.repository = repository;
        }

        #region Get region
        /// <summary>
        /// Получение списка всех пользователей.
        /// </summary>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<IEnumerable<User>>> GetUsersAsync()
        {
            return repository.QueryAsync(con =>
            {
                return con.GetAllAsync<User>();
            }, Enumerable.Empty<User>());
        }

        /// <summary>
        /// Получить пользователя.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        /// <param name="usePassword">Проверять пароль.</param>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<User>> GetUserAsync(string login, string password, bool usePassword)
        {
            return repository.QueryAsync(con =>
            {
                if (usePassword)
                    return con.QueryFirstOrDefaultAsync<User>("SELECT * FROM users WHERE login = @login AND password = @password", new { login, password });
                return con.QueryFirstOrDefaultAsync<User>("SELECT * FROM users WHERE login = @login", new { login });
            });
        }
        #endregion

        #region Log region
        /// <summary>
        /// Получить логи входов пользователя.
        /// </summary>
        /// <param name="userId">ИД пользователя.</param>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<IEnumerable<SignInLog>>> GetSignInLogsAsync(int userId)
        {
            return repository.QueryAsync(con =>
            {
                return con.QueryAsync<SignInLog>("SELECT * FROM signinlogs WHERE userId = @userId", new { userId });
            }, Enumerable.Empty<SignInLog>());
        }

        /// <summary>
        /// Добавить лог авторизации пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        public async void AddSignInLogAsync(User user)
        {
            await repository.QueryAsync<long>(async con =>
            {
                return await con.InsertAsync(new SignInLog { UserId = user.Id });
            });
        }
        #endregion

        #region CUD region
        /// <summary>
        /// Регистрация нового пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<bool>> AddUserAsync(User user)
        {
            return repository.QueryAsync(async con =>
            {
                await con.InsertAsync(user);
                return true;
            });
        }

        /// <summary>
        /// Сохранить пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<bool>> SaveUserAsync(User user)
        {
            return repository.QueryAsync(con =>
            {
                return con.UpdateAsync(user);
            });
        }

        /// <summary>
        /// Удаление пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<bool>> RemoveUserAsync(User user)
        {
            return repository.QueryAsync(con =>
            {
                return con.DeleteAsync(user);
            });
        }
        #endregion
    }
}
