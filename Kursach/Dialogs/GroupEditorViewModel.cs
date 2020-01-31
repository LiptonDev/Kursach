using DevExpress.Mvvm;
using DryIoc;
using Kursach.Core.Models;
using System.Windows.Input;

namespace Kursach.Dialogs
{
    /// <summary>
    /// Group editor view model.
    /// </summary>
    [DialogName(nameof(GroupEditorView))]
    class GroupEditorViewModel : BaseEditModeViewModel<Group>
    {
        /// <summary>
        /// Менеджер диалогов.
        /// </summary>
        readonly IDialogManager dialogManager;

        /// <summary>
        /// Конструктор для DesignTime.
        /// </summary>
        public GroupEditorViewModel()
        {

        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public GroupEditorViewModel(Group group,
                                    bool isEditMode,
                                    int division,
                                    IContainer container,
                                    IDialogManager dialogManager)
            : base(group, isEditMode, container)
        {
            this.dialogManager = dialogManager;

            EditableObject.Division = division;

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
