using Kursach.Core;
using Kursach.Core.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Kursach.Client.Design
{
    class DesignLogin : ILogin
    {
        public DesignLogin(IHubConfigurator hubConfigurator)
        {

        }

        public Task<KursachResponse<User, LoginResponse>> LoginAsync(string login, string password)
        {
            return Task.FromResult(new KursachResponse<User, LoginResponse>(KursachResponseCode.Ok, LoginResponse.Ok, new User
            {
                Login = "root",
                Password = "root",
                Mode = UserMode.Admin
            }));
        }

        public void Logout()
        {
            Debug.WriteLine("LOG OUT!");
        }
    }
}
