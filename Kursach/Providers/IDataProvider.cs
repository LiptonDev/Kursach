using ISTraining_Part.Core.Models;
using ISTraining_Part.Models;
using System.Collections.ObjectModel;

namespace ISTraining_Part.Providers
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
        /// Сообщения в чате.
        /// </summary>
        ObservableCollection<ChatMessage> ChatMessages { get; }

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
