using Kursach.Core;
using Kursach.Core.Models;
using Kursach.Core.ServerMethods;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Server.DataBase;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Server.Hubs
{
    /// <summary>
    /// Хаб авторизации.
    /// </summary>
    [HubName(HubNames.LoginHub)]
    public class LoginHub : Hub, LoginMethods
    {
        /// <summary>
        /// Пользователи.
        /// </summary>
        public static ConcurrentDictionary<string, bool> Users { get; } = new ConcurrentDictionary<string, bool>();

        /// <summary>
        /// База данных.
        /// </summary>
        readonly IDataBase dataBase;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public LoginHub(IDataBase dataBase)
        {
            this.dataBase = dataBase;
        }

        /// <summary>
        /// Авторизация на сервере.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        /// <returns></returns>
        public async Task<KursachResponse<User, LoginResponse>> LoginAsync(string login, string password)
        {
            var res = await dataBase.GetUserAsync(login, password, true);

            LoginResponse loginResponse = LoginResponse.Invalid;
            Users[Context.ConnectionId] = res && res.Response != null;
            if (res && res.Response != null)
            {
                loginResponse = LoginResponse.Ok;
                await HubHelper.GetHubContext<UsersHub>().Groups.Add(Context.ConnectionId, Consts.AuthorizedGroup);
                await HubHelper.GetHubContext<StaffHub>().Groups.Add(Context.ConnectionId, Consts.AuthorizedGroup);
                await HubHelper.GetHubContext<GroupsHub>().Groups.Add(Context.ConnectionId, Consts.AuthorizedGroup);
                await HubHelper.GetHubContext<StudentsHub>().Groups.Add(Context.ConnectionId, Consts.AuthorizedGroup);
            }

            Console.WriteLine($"Set status: {Context.ConnectionId} => {Users[Context.ConnectionId]}");

            return new KursachResponse<User, LoginResponse>(res.Code, loginResponse, res.Response);
        }

        /// <summary>
        /// Пользователь отключен.
        /// </summary>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            Users.TryRemove(Context.ConnectionId, out _);

            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// Выход.
        /// </summary>
        public void Logout()
        {
            Users.TryRemove(Context.ConnectionId, out _);

            Console.WriteLine($"Set status: {Context.ConnectionId} => False");

            HubHelper.GetHubContext<UsersHub>().Groups.Remove(Context.ConnectionId, Consts.AuthorizedGroup);
            HubHelper.GetHubContext<StaffHub>().Groups.Remove(Context.ConnectionId, Consts.AuthorizedGroup);
            HubHelper.GetHubContext<GroupsHub>().Groups.Remove(Context.ConnectionId, Consts.AuthorizedGroup);
            HubHelper.GetHubContext<StudentsHub>().Groups.Remove(Context.ConnectionId, Consts.AuthorizedGroup);
        }
    }
}
