using DevExpress.Mvvm;
using DryIoc;
using Kursach.DataBase;
using Kursach.Dialogs;
using Kursach.Models;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using System.Windows.Input;

namespace Kursach.ViewModels
{
    /// <summary>
    /// Staff editor view model.
    /// </summary>
    [DialogName(nameof(Views.StaffEditorView))]
    class StaffEditorViewModel : IClosableDialog, IDialogIdentifier, IEditMode
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public string Identifier => "StaffEditorViewModel";

        /// <summary>
        /// Owner.
        /// </summary>
        public IDialogIdentifier OwnerIdentifier { get; }

        /// <summary>
        /// Менеджер диалогов.
        /// </summary>
        readonly IDialogManager dialogManager;

        /// <summary>
        /// True - Режим редактирования группы, иначе - добавление.
        /// </summary>
        public bool IsEditMode { get; }

        /// <summary>
        /// Сотрудник.
        /// </summary>
        public Staff Staff { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        public StaffEditorViewModel(Staff staff, bool isEditMode, IContainer container, IDialogManager dialogManager)
        {
            IsEditMode = isEditMode;
            OwnerIdentifier = container.ResolveRootDialogIdentifier();
            this.dialogManager = dialogManager;

            if (isEditMode)
                Staff = (Staff)staff.Clone();
            else Staff = new Staff();

            CloseDialogCommand = new DelegateCommand(CloseDialog);
        }

        /// <summary>
        /// Команда закрытия диалога.
        /// </summary>
        public ICommand CloseDialogCommand { get; }

        /// <summary>
        /// Закрытие диалога.
        /// </summary>
        private async void CloseDialog()
        {
            if (!Staff.IsValid)
            {
                await this.ShowMessageBoxAsync(Staff.Error, MaterialMessageBoxButtons.Ok);
                return;
            }

            OwnerIdentifier.Close(Staff);
        }
    }
}
