using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using System.Threading.Tasks;

namespace ISTraining_Part.Client.Classes
{
    /// <summary>
    /// Управление авторизацией.
    /// </summary>
    class Login : Invoker, ILogin
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public Login(IHubConfigurator hubConfigurator) : base(hubConfigurator, HubNames.LoginHub)
        {
        }

        /// <summary>
        /// Авторизация на сервере.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        /// <returns></returns>
        public Task<KursachResponse<User, LoginResponse>> LoginAsync(string login, string password)
        {
            return TryInvokeAsync<User, LoginResponse>(argDefault: LoginResponse.ServerError, args: new object[] { login, password });
        }

        /// <summary>
        /// Выход.
        /// </summary>
        public void Logout()
        {
            TryInvokeAsync();
        }
    }
}
