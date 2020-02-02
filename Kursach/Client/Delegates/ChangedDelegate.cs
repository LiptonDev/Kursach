using Kursach.Core.ServerEvents;

namespace Kursach.Client.Delegates
{
    /// <summary>
    /// Делегат изменения объекта в базе.
    /// </summary>
    /// <param name="status">Статус.</param>
    /// <param name="arg">Объект.</param>
    delegate void OnChanged<T>(DbChangeStatus status, T arg);
}
