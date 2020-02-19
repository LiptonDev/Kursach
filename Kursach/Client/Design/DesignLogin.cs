using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ISTraining_Part.Client.Design
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
