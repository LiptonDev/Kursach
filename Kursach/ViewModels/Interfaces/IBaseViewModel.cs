using DevExpress.Mvvm;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ISTraining_Part.ViewModels.Interfaces
{
    /// <summary>
    /// Указывает, что ViewModel содержит в себе команды добавления, удаления, редактирования данных.
    /// </summary>
    interface IBaseViewModel<T>
    {
        /// <summary>
        /// Объекты.
        /// </summary>
        ObservableCollection<T> Items { get; set; }

        /// <summary>
        /// Выбранный объект.
        /// </summary>
        T SelectedItem { get; set; }

        /// <summary>
        /// Команда добавления.
        /// </summary>
        ICommand AddCommand { get; }

        /// <summary>
        /// Команда открытия окна редактирования.
        /// </summary>
        ICommand<T> EditCommand { get; }

        /// <summary>
        /// Команда удаления.
        /// </summary>
        ICommand<T> DeleteCommand { get; }

        /// <summary>
        /// Добавление.
        /// </summary>
        void Add();

        /// <summary>
        /// Редактирование.
        /// </summary>
        Task Edit(T obj);

        /// <summary>
        /// Удаление.
        /// </summary>
        Task Delete(T obj);
    }
}
