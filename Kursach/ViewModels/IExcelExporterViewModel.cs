using System.Windows.Input;

namespace Kursach.ViewModels
{
    /// <summary>
    /// Указывает, что ViewModel можешь экспортировать данные в Excel.
    /// </summary>
    interface IExcelExporterViewModel
    {
        /// <summary>
        /// Команда экспорта.
        /// </summary>
        ICommand ExportToExcelCommand { get; }

        /// <summary>
        /// Метод экспорта.
        /// </summary>
        void ExportToExcel();
    }
}
