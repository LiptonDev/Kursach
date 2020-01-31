using Kursach.Core.Models;
using System.Threading.Tasks;

namespace Kursach.Core.ServerMethods
{
    /// <summary>
    /// Список методов хаба авторизации.
    /// </summary>
    public interface LoginMethods
    {
        /// <summary>
        /// Авторизация на сервере.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        /// <returns></returns>
        Task<KursachResponse<User, LoginResponse>> LoginAsync(string login, string password);

        /// <summary>
        /// Выход.
        /// </summary>
        void Logout();
    }
}
