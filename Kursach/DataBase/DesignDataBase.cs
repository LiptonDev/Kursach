using System.Collections.Generic;
using System.Threading.Tasks;
using Kursach.Models;

namespace Kursach.DataBase
{
    class DesignDataBase : IDataBase
    {
        const int Delay = 100;

        public async Task<bool> AddSignInLogAsync(User user)
        {
            await Task.Delay(Delay);

            return true;
        }

        public async Task<bool> ChangePassword(LoginUser user)
        {
            await Task.Delay(Delay);

            return true;
        }

        public async Task<IEnumerable<SignInLog>> GetSignInLogsAsync(User user)
        {
            await Task.Delay(Delay);
            List<SignInLog> logs = new List<SignInLog>();
            for (int i = 0; i < 50; i++)
            {
                logs.Add(new SignInLog { User = user });
            }
            return logs;
        }

        public async Task<User> GetUserAsync(string login, string password, bool usePassword)
        {
            await Task.Delay(Delay);
            return GetUser(login, password);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            await Task.Delay(Delay);

            List<User> users = new List<User>();
            for (int i = 0; i < 50; i++)
            {
                users.Add(GetUser(i.ToString(), "password", UserMode.ReadWrite));
            }

            return users;
        }

        public async Task<bool> RemoveUserAsync(User user)
        {
            await Task.Delay(Delay);
            return true;
        }

        public async Task<bool> SignUpAsync(LoginUser user, UserMode mode)
        {
            await Task.Delay(Delay);
            return true;
        }

        private User GetUser(string login, string password, UserMode mode = UserMode.Admin) => new User { Id = 1337, Login = login, Password = password, Mode = mode };
    }
}
