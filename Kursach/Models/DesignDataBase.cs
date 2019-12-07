using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kursach.Models
{
    class DesignDataBase : IDataBase
    {
        const int Delay = 100;

        public async Task AddSignInLogAsync(User user)
        {
            await Task.Delay(Delay);
        }

        public async Task<IEnumerable<SignInLog>> GetSignInLogsAsync()
        {
            await Task.Delay(Delay);

            List<SignInLog> logs = new List<SignInLog>();

            for (int i = 0; i < 50; i++)
            {
                logs.Add(new SignInLog { User = GetUser("root", "root"), UserId = 1337 });
            }

            return logs;
        }
        
        public async Task<User> GetUserAsync(string login, string password, bool usePassword)
        {
            await Task.Delay(Delay);

            return GetUser(login, password);
        }

        public async Task SignUpAsync(string login, string password, UserMode mode)
        {
            await Task.Delay(Delay);
        }

        private User GetUser(string login, string password) => new User { Id = 1337, Login = login, Password = password, Mode = UserMode.Admin };
    }
}
