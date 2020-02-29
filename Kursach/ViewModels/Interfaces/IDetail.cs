using DevExpress.Mvvm;
using ISTraining_Part.Core.Models;

namespace ISTraining_Part.ViewModels.Interfaces
{
    /// <summary>
    /// Указывает, что ViewModel может отображать детальную информацию о человеке.
    /// </summary>
    interface IDetail
    {
        /// <summary>
        /// Команда открытия детальной информации.
        /// </summary>
        ICommand<People> ShowDetailInfoCommand { get; }

        /// <summary>
        /// Команды открытия редактирования детальной информации.
        /// </summary>
        ICommand<People> ShowDetailInfoEditorCommand { get; }
    }
}
