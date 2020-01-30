using Kursach.DataBase;
using Kursach.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace Kursach.Dialogs
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
        /// База данных.
        /// </summary>
        readonly IDataBase dataBase;

        /// <summary>
        /// Конструктор для DesignTime.
        /// </summary>
        public SignInLogsViewModel()
        {
            dataBase = new DesignDataBase();
            Load(null);
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public SignInLogsViewModel(User user, IDataBase dataBase)
        {
            this.dataBase = dataBase;

            Logs = new ObservableCollection<SignInLog>();

            Load(user);
        }

        /// <summary>
        /// Загрузка логов.
        /// </summary>
        private async void Load(User user)
        {
            var res = await dataBase.GetSignInLogsAsync(user);
            Logs.AddRange(res.OrderByDescending(x => x.Date));
        }
    }
}
