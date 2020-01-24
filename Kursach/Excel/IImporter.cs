namespace Kursach.Excel
{
    /// <summary>
    /// Импорт данных.
    /// </summary>
    interface IImporter<T, TTarget>
    {
        /// <summary>
        /// Импорт данных из Excel файла.
        /// </summary>
        /// <returns></returns>
        T Import(TTarget target);
    }
}
