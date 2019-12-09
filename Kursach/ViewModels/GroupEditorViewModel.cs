using DevExpress.Mvvm;
using DryIoc;
using Kursach.DataBase;
using Kursach.Dialogs;
using Kursach.Models;
using MaterialDesignXaml.DialogsHelper;
using System.Windows.Input;

namespace Kursach.ViewModels
{
    /// <summary>
    /// Group editor view model.
    /// </summary>
    [DialogName(nameof(Views.GroupEditorView))]
    class GroupEditorViewModel : IClosableDialog, IDialogIdentifier
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public string Identifier => "GroupEditorViewModel";

        /// <summary>
        /// Owner.
        /// </summary>
        public IDialogIdentifier OwnerIdentifier { get; }

        /// <summary>
        /// Менеджер диалогов.
        /// </summary>
        readonly IDialogManager dialogManager;

        /// <summary>
        /// Группа.
        /// </summary>
        public Group Group { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        public GroupEditorViewModel(Group group, IContainer container, IDialogManager dialogManager)
        {
            OwnerIdentifier = container.ResolveRootDialogIdentifier();
            this.dialogManager = dialogManager;

            Group = (Group)group.Clone();

            CloseDialogCommand = new DelegateCommand(CloseDialog);
            OpenStaffSelectorCommand = new DelegateCommand(OpenStaffSelector);
        }

        /// <summary>
        /// Команда закрытия диалога.
        /// </summary>
        public ICommand CloseDialogCommand { get; }

        /// <summary>
        /// Команда открытия выбора куратора.
        /// </summary>
        public ICommand OpenStaffSelectorCommand { get; }

        /// <summary>
        /// Выбор куратора.
        /// </summary>
        private async void OpenStaffSelector()
        {
            var res = await dialogManager.SelectStaff(Group.Curator, this);

            if (res != null)
                Group.CuratorId = res.Id;
        }

        /// <summary>
        /// Закрытие диалога.
        /// </summary>
        private void CloseDialog()
        {
            OwnerIdentifier.Close(Group);
        }
    }
}
