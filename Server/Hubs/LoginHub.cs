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
    public class LoginHub : Hub, ILoginHub
    {
        /// <summary>
        /// Пользователи.
        /// </summary>
        public static ConcurrentDictionary<string, UserMode> Users { get; } = new ConcurrentDictionary<string, UserMode>();

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
            var id = Context.ConnectionId;
            var res = await dataBase.GetUserAsync(login, password, true);

            LoginResponse loginResponse = LoginResponse.Invalid;
            if (res && res.Response != null)
            {
                loginResponse = LoginResponse.Ok;

                if (res.Response.Mode == UserMode.Admin)
                    await HubHelper.AddToAdminGroup<UsersHub>(id);

                await HubHelper.AddToAuthorizedGroup<UsersHub>(id);
                await HubHelper.AddToAuthorizedGroup<StaffHub>(id);
                await HubHelper.AddToAuthorizedGroup<StudentsHub>(id);
                await HubHelper.AddToAuthorizedGroup<GroupsHub>(id);

                dataBase.AddSignInLogAsync(res.Response);

                Users[id] = res.Response.Mode;
            }

            Console.WriteLine($"{Context.ConnectionId} logged in");

            return new KursachResponse<User, LoginResponse>(res.Code, loginResponse, res.Response);
        }

        /// <summary>
        /// Пользователь подключен.
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {
            Console.WriteLine($"{Context.ConnectionId} connected");

            return base.OnConnected();
        }

        /// <summary>
        /// Пользователь отключен.
        /// </summary>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            var id = Context.ConnectionId;

            Users.TryRemove(id, out _);

            Console.WriteLine($"{id} disconnected");

            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// Выход.
        /// </summary>
        public async void Logout()
        {
            var id = Context.ConnectionId;

            Users.TryRemove(id, out _);

            Console.WriteLine($"{id} logged out");

            await HubHelper.RemoveFromAdminGroup<UsersHub>(id);
            await HubHelper.RemoveFromAuthorizedGroup<UsersHub>(id);
            await HubHelper.RemoveFromAuthorizedGroup<StaffHub>(id);
            await HubHelper.RemoveFromAuthorizedGroup<StudentsHub>(id);
            await HubHelper.RemoveFromAuthorizedGroup<GroupsHub>(id);
        }
    }
}
