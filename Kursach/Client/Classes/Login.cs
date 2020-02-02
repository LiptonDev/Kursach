using Kursach.Client.Interfaces;
using Kursach.Core;
using Kursach.Core.Models;
using Kursach.Core.ServerEvents;
using Microsoft.AspNet.SignalR.Client;
using System.Threading.Tasks;

namespace Kursach.Client.Classes
{
    /// <summary>
    /// Управление авторизацией.
    /// </summary>
    class Login : Invoker, ILogin
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public Login(IHubConfigurator hubConfigurator, TaskFactory sync) : base(hubConfigurator, HubNames.LoginHub)
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
