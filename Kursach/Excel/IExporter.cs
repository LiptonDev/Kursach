using System.Threading.Tasks;

namespace Kursach.Excel
{
    /// <summary>
    /// Экспорт данных.
    /// </summary>
    interface IAsyncExporter<T>
    {
        /// <summary>
        /// Экспорт данных.
        /// </summary>
        Task<bool> Export(T arg);
    }

    /// <summary>
    /// Экспорт данных.
    /// </summary>
    interface IExporter<T>
    {
        /// <summary>
        /// Экспорт данных.
        /// </summary>
        bool Export(T arg);
    }
}
