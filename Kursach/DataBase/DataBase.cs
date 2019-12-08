using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq;
using Kursach.Models;
using System;
using System.Runtime.CompilerServices;

namespace Kursach.DataBase
{
    /// <summary>
    /// База данных.
    /// </summary>
    class DataBase : IDataBase
    {
        /// <summary>
        /// База данных.
        /// </summary>
        readonly Context context;

        /// <summary>
        /// Ctor.
        /// </summary>
        public DataBase(Context context)
        {
            this.context = context;
        }

        /// <summary>
        /// Запрос в базу с логированием.
        /// </summary>
        /// <returns></returns>
        private async Task<T> query<T>(Func<Task<T>> action, [CallerMemberName]string name = null)
        {
            try
            {
                return await action?.Invoke();
            }
            catch (Exception ex)
            {
                Logger.Log.Error($"Ошибка запроса к базе: {{ex: {ex.Message}, member: {name}}}");
                return default;
            }
        }

        /// <summary>
        /// Получить пользователя.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        /// <returns></returns>
        public async Task<User> GetUserAsync(string login, string password, bool usePassword)
        {
            return await query(async () =>
            {
                if (usePassword)
                    return await context.Users.FirstOrDefaultAsync(x => x.Login == login && x.Password == password);
                return await context.Users.FirstOrDefaultAsync(x => x.Login == login);
            });
        }

        /// <summary>
        /// Добавить лог входа в программу.
        /// </summary>
        /// <param name="user">Вошедший пользователь.</param>
        /// <returns></returns>
        public async Task<bool> AddSignInLogAsync(User user)
        {
            return await query(async () =>
            {
                context.SignInLogs.Add(new SignInLog { User = user });
                await context.SaveChangesAsync();

                return true;
            });
        }

        /// <summary>
        /// Получить логи входов пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        public async Task<IEnumerable<SignInLog>> GetSignInLogsAsync(User user)
        {
            return await query(async () => await context.SignInLogs.AsNoTracking().Where(x => x.UserId == user.Id).ToListAsync());
        }

        /// <summary>
        /// Регистрация нового пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <param name="mode">Права.</param>
        /// <returns></returns>
        public async Task<bool> SignUpAsync(LoginUser user, UserMode mode)
        {
            return await query(async () =>
            {
                context.Users.Add(user.ToUser(mode));
                await context.SaveChangesAsync();

                return true;
            });
        }

        /// <summary>
        /// Смена пароля на новый.
        /// </summary>
        /// <param name="loginUser">Пользователь.</param>
        /// <returns></returns>
        public async Task<bool> ChangePassword(LoginUser loginUser)
        {
            return await query(async () =>
            {
                var user = await GetUserAsync(loginUser.Login, null, false);

                if (user == null)
                    return false;

                user.Password = loginUser.Password;

                context.Users.Attach(user);
                context.Entry(user).Property(x => x.Password).IsModified = true;

                await context.SaveChangesAsync();

                return true;
            });
        }

        /// <summary>
        /// Получение списка всех пользователей.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await query(async () => await context.Users.AsNoTracking().ToListAsync());
        }

        /// <summary>
        /// Удаление пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        public async Task<bool> RemoveUserAsync(User user)
        {
            return await query(async () =>
            {
                context.Users.Attach(user);
                context.Entry(user).State = EntityState.Deleted;

                await context.SaveChangesAsync();

                return true;
            });
        }
    }
}
