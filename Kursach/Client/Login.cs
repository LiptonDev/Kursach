using Kursach.Core;
using Kursach.Core.Models;
using Microsoft.AspNet.SignalR.Client;
using System.Threading.Tasks;

namespace Kursach.Client
{
    class Login : ILogin
    {
        /// <summary>
        /// Прокси.
        /// </summary>
        IHubProxy proxy;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Login(IHubConfigurator hubConfigurator)
        {
            proxy = hubConfigurator.Hub.CreateHubProxy(HubNames.LoginHub);
        }

        /// <summary>
        /// Авторизация на сервере.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        /// <returns></returns>
        public Task<KursachResponse<User, LoginResponse>> LoginAsync(string login, string password)
        {
            return proxy.TryInvokeAsync<User, LoginResponse>(argDefault: LoginResponse.ServerError, args: new object[] { login, password });
        }

        /// <summary>
        /// Выход.
        /// </summary>
        public void Logout()
        {
            proxy.TryInvokeAsync();
        }
    }
}
