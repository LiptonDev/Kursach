using Kursach.DataBase;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Kursach.Models
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
        /// Получить пользователя.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        /// <returns></returns>
        public async Task<User> GetUserAsync(string login, string password, bool usePassword)
        {
            if (usePassword)
                return await context.Users.FirstOrDefaultAsync(x => x.Login == login && x.Password == password);
            return await context.Users.FirstOrDefaultAsync(x => x.Login == login);
        }

        /// <summary>
        /// Добавить лог входа в программу.
        /// </summary>
        /// <param name="user">Вошедший пользователь.</param>
        /// <returns></returns>
        public async Task AddSignInLogAsync(User user)
        {
            context.SignInLogs.Add(new SignInLog { User = user });
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Получить все логи входов.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SignInLog>> GetSignInLogsAsync()
        {
            return await context.SignInLogs.AsNoTracking().Include(x => x.User).ToListAsync();
        }

        /// <summary>
        /// Регистрация нового пользователя.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        /// <param name="mode">Права.</param>
        /// <returns></returns>
        public async Task SignUpAsync(string login, string password, UserMode mode)
        {
            context.Users.Add(new User { Login = login, Password = password, Mode = mode });
            await context.SaveChangesAsync();
        }
    }
}
