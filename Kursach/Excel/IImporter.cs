using System.Threading.Tasks;

namespace Kursach.Excel
{
    /// <summary>
    /// Импорт данных.
    /// </summary>
    interface IAsyncImporter<T>
    {
        /// <summary>
        /// Импорт данных из Excel файла.
        /// </summary>
        /// <returns></returns>
        Task<T> Import();
    }

    /// <summary>
    /// Импорт данных.
    /// </summary>
    interface IAsyncImporter<T, TArg>
    {
        /// <summary>
        /// Импорт данных из Excel файла.
        /// </summary>
        /// <returns></returns>
        Task<T> Import(TArg arg);
    }

    /// <summary>
    /// Импорт данных.
    /// </summary>
    interface IImporter<T>
    {
        /// <summary>
        /// Импорт данных из Excel файла.
        /// </summary>
        /// <returns></returns>
        T Import();
    }
}
