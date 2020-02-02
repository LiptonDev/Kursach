using System.Windows.Data;

namespace Kursach.Dialogs
{
    /// <summary>
    /// Указывает, что ViewModel - модель выбора данных.
    /// </summary>
    interface ISelectorViewModel<T>
    {
        /// <summary>
        /// Все данные.
        /// </summary>
        ListCollectionView Items { get; }

        /// <summary>
        /// Выбранные данные.
        /// </summary>
        T SelectedItem { get; set; }
    }
}
