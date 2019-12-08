using DevExpress.Mvvm;
using Kursach.DataBase;
using System.Collections.ObjectModel;

namespace Kursach.ViewModels
{
    /// <summary>
    /// Users view model.
    /// </summary>
    class UsersViewModel : ViewModelBase
    {
        /// <summary>
        /// Пользователи.
        /// </summary>
        public ObservableCollection<User> Users { get; }

        /// <summary>
        /// База данных.
        /// </summary>
        readonly IDataBase dataBase;

        /// <summary>
        /// Ctor.
        /// </summary>
        public UsersViewModel(IDataBase dataBase)
        {
            this.dataBase = dataBase;

            Users = new ObservableCollection<User>();

            Load();
        }

        /// <summary>
        /// Загрузка всех пользователей.
        /// </summary>
        private async void Load()
        {
            var res = await dataBase.GetUsersAsync();
            Users.AddRange(res);
        }
    }
}
