using DevExpress.Mvvm;
using Kursach.Models;
using System.Linq;
using System.Collections.ObjectModel;

namespace Kursach.ViewModels
{
    /// <summary>
    /// SignIn logs view model.
    /// </summary>
    class SignInLogsViewModel
    {
        /// <summary>
        /// Логи входов.
        /// </summary>
        public ObservableCollection<SignInLog> Logs { get; } = new ObservableCollection<SignInLog>();

        /// <summary>
        /// База данных.
        /// </summary>
        readonly IDataBase dataBase;

        /// <summary>
        /// Ctor.
        /// </summary>
        public SignInLogsViewModel(IDataBase dataBase)
        {
            this.dataBase = dataBase;

            GetLogs();
        }

        /// <summary>
        /// Загрузка логов.
        /// </summary>
        private async void GetLogs()
        {
            var res = await dataBase.GetSignInLogsAsync();
            Logs.AddRange(res.Reverse());

            Logger.Log.Info("Получен список входов пользователей");
        }
    }
}
