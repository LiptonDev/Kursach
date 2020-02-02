namespace Kursach.Core.ServerEvents
{
    /// <summary>
    /// Событие изменения объекта.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IChangedEvent<T>
    {
        /// <summary>
        /// Изменение объекта.
        /// </summary>
        /// <param name="status">Статус.</param>
        /// <param name="arg">Объект.</param>
        void OnChanged(DbChangeStatus status, T arg);
    }
}
