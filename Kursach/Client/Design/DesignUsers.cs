using ISTraining_Part.Client.Delegates;
using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.ServerEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISTraining_Part.Client.Design
{
    class DesignUsers : IUsers
    {
        public DesignUsers()
        {
        }

        public event OnChanged<User> OnChanged;

        public Task<KursachResponse<bool>> AddUserAsync(User user)
        {
            OnChanged?.Invoke(DbChangeStatus.Add, user);
            return Task.FromResult(new KursachResponse<bool>(KursachResponseCode.Ok, true));
        }

        public Task<KursachResponse<IEnumerable<SignInLog>>> GetSignInLogsAsync(int userId)
        {
            var logs = Enumerable.Range(0, 15).Select(x => new SignInLog
            {
                Date = DateTime.Now,
                UserId = userId
            });

            return Task.FromResult(new KursachResponse<IEnumerable<SignInLog>>(KursachResponseCode.Ok, logs));
        }

        public Task<KursachResponse<User>> GetUserAsync(string login, string password, bool usePassword)
        {
            return Task.FromResult(new KursachResponse<User>(KursachResponseCode.Ok, new User { Login = login, Password = password }));
        }

        public Task<KursachResponse<IEnumerable<User>>> GetUsersAsync()
        {
            var users = Enumerable.Range(0, 10).Select(x => new User
            {
                Login = "login",
                Password = "password",
                Id = x,
                Mode = x % 2 == 0 ? UserMode.Admin : UserMode.Read
            });

            return Task.FromResult(new KursachResponse<IEnumerable<User>>(KursachResponseCode.Ok, users));
        }

        public Task<KursachResponse<bool>> RemoveUserAsync(User user)
        {
            OnChanged?.Invoke(DbChangeStatus.Remove, user);
            return Task.FromResult(new KursachResponse<bool>(KursachResponseCode.Ok, true));
        }

        public Task<KursachResponse<bool>> SaveUserAsync(User user)
        {
            OnChanged?.Invoke(DbChangeStatus.Update, user);
            return Task.FromResult(new KursachResponse<bool>(KursachResponseCode.Ok, true));
        }
    }
}
