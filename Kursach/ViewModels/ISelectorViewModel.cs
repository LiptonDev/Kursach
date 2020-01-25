using System.Collections.ObjectModel;

namespace Kursach.ViewModels
{
    /// <summary>
    /// Указывает, что ViewModel - модель выбора данных.
    /// </summary>
    interface ISelectorViewModel<T>
    {
        /// <summary>
        /// Все данные.
        /// </summary>
        ObservableCollection<T> Items { get; }

        /// <summary>
        /// Выбранные данные.
        /// </summary>
        T SelectedItem { get; set; }

        /// <summary>
        /// Загрузка данных.
        /// </summary>
        void Load(int currentId);
    }
}
