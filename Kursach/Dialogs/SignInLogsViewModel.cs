using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Dialogs.Attributes;
using System.Collections.ObjectModel;
using System.Linq;

namespace ISTraining_Part.Dialogs
{
    /// <summary>
    /// Signin logs view model.
    /// </summary>
    [DialogName(nameof(SignInLogsView))]
    class SignInLogsViewModel
    {
        /// <summary>
        /// Логи входов.
        /// </summary>
        public ObservableCollection<SignInLog> Logs { get; }

        /// <summary>
        /// Конструктор для DesignTime.
        /// </summary>
        public SignInLogsViewModel()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public SignInLogsViewModel(User user, IClient client)
        {
            Logs = new ObservableCollection<SignInLog>();

            Load(client, user);
        }

        /// <summary>
        /// Загрузка логов.
        /// </summary>
        private async void Load(IClient client, User user)
        {
            var res = await client.Users.GetSignInLogsAsync(user.Id);
            if (res)
                Logs.AddRange(res.Response.OrderByDescending(x => x.Date));
        }
    }
}
