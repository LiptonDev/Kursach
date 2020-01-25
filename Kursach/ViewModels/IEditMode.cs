namespace Kursach.ViewModels
{
    /// <summary>
    /// Указывает, что ViewModel - модель редактирования объекта.
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
