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

        public Task<ISTrainingPartResponse<User, LoginResponse>> LoginAsync(string login, string password)
        {
            User user = new User
            {
                Login = login,
                Password = password,
                Name = login,
                Mode = UserMode.Admin
            };
            if (login == "read")
                user.Mode = UserMode.Read;

            return Task.FromResult(new ISTrainingPartResponse<User, LoginResponse>(ISTrainingPartResponseCode.Ok, LoginResponse.Ok, user));
        }

        public void Logout()
        {
            Debug.WriteLine("LOG OUT!");
        }
    }
}
