using System.Threading.Tasks;

namespace ISTraining_Part.Excel.Interfaces
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
    interface IImporter<T>
    {
        /// <summary>
        /// Импорт данных из Excel файла.
        /// </summary>
        /// <returns></returns>
        T Import();
    }
}
