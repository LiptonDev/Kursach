using Kursach.Core.ViewModels;

namespace Kursach.Dialogs
{
    /// <summary>
    /// Указывает, что ViewModel - модель редактирования объекта.
    /// </summary>
    interface IEditMode<T> where T : ValidateViewModel
    {
        /// <summary>
        /// True - режим редактирования, false - добавления.
        /// </summary>
        bool IsEditMode { get; }

        /// <summary>
        /// Объект для редактирования.
        /// </summary>
        T EditableObject { get; }
    }
}
