using ISTraining_Part.Core.Models;
using System.Threading.Tasks;

namespace ISTraining_Part.Core.ServerMethods
{
    /// <summary>
    /// Список методов хаба авторизации.
    /// </summary>
    public interface ILoginHub
    {
        /// <summary>
        /// Авторизация на сервере.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        /// <returns></returns>
        Task<ISTrainingPartResponse<User, LoginResponse>> LoginAsync(string login, string password);

        /// <summary>
        /// Выход.
        /// </summary>
        void Logout();
    }
}
