using System.Threading.Tasks;
using System.Windows.Input;

namespace ISTraining_Part.ViewModels
{
    /// <summary>
    /// Указывает, что ViewModel можешь импортировать данные из Excel.
    /// </summary>
    interface IExcelImporterViewModel
    {
        /// <summary>
        /// Команда экспорта.
        /// </summary>
        ICommand ImportFromExcelCommand { get; }

        /// <summary>
        /// Метод экспорта.
        /// </summary>
        Task ImportFromExcel();
    }
}
