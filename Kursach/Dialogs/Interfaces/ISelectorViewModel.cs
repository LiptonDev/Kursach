using System.Collections.ObjectModel;

namespace ISTraining_Part.Dialogs.Interfaces
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
    }
}
