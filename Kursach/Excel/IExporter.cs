namespace Kursach.Excel
{
    /// <summary>
    /// Экспорт данных.
    /// </summary>
    interface IExporter<T>
    {
        /// <summary>
        /// Экспорт данных.
        /// </summary>
        /// <param name="arg">Объект.</param>
        void Export(T arg);
    }
}
