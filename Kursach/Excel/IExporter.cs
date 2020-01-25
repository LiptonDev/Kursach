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
        void Export(T arg);
    }

    /// <summary>
    /// Экспорт данных.
    /// </summary>
    interface IExporter<T1, T2>
    {
        /// <summary>
        /// Экспорт данных.
        /// </summary>
        void Export(T1 arg, T2 arg2);
    }
}
