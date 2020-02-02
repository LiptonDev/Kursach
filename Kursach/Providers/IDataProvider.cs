using Kursach.Core.Models;
using System.Collections.ObjectModel;

namespace Kursach.Providers
{
    /// <summary>
    /// Поставщик данных.
    /// </summary>
    interface IDataProvider
    {
        /// <summary>
        /// Пользователи.
        /// </summary>
        ObservableCollection<User> Users { get; }

        /// <summary>
        /// Сотрудники.
        /// </summary>
        ObservableCollection<Staff> Staff { get; }

        /// <summary>
        /// Группы.
        /// </summary>
        ObservableCollection<Group> Groups { get; }

        /// <summary>
        /// Студенты.
        /// </summary>
        ObservableCollection<Student> Students { get; }

        /// <summary>
        /// Загрузка данных.
        /// </summary>
        void Load(UserMode mode);

        /// <summary>
        /// Очистка данных.
        /// </summary>
        void Clear();
    }
}
