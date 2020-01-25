using DevExpress.Mvvm;
using DryIoc;
using Kursach.Models;
using Kursach.Dialogs;
using System.Windows.Input;

namespace Kursach.ViewModels
{
    /// <summary>
    /// Group editor view model.
    /// </summary>
    [DialogName(nameof(Views.GroupEditorView))]
    class GroupEditorViewModel : BaseEditModeViewModel<Group>
    {
        /// <summary>
        /// Менеджер диалогов.
        /// </summary>
        readonly IDialogManager dialogManager;

        /// <summary>
        /// Ctor.
        /// </summary>
        public GroupEditorViewModel(Group group, bool isEditMode, IContainer container, IDialogManager dialogManager)
            : base(group, isEditMode, container)
        {
            this.dialogManager = dialogManager;

            OpenStaffSelectorCommand = new DelegateCommand(OpenStaffSelector);
        }

        /// <summary>
        /// Команда открытия выбора куратора.
        /// </summary>
        public ICommand OpenStaffSelectorCommand { get; }

        /// <summary>
        /// Выбор куратора.
        /// </summary>
        private async void OpenStaffSelector()
        {
            var res = await dialogManager.SelectStaff(EditableObject.CuratorId, this);

            if (res != null)
                EditableObject.CuratorId = res.Id;
        }
    }
}
