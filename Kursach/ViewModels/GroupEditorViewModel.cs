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
    /// Group editor view model.
    /// </summary>
    [DialogName(nameof(Views.GroupEditorView))]
    class GroupEditorViewModel : IClosableDialog, IDialogIdentifier, IEditMode
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public string Identifier => nameof(GroupEditorViewModel);

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
        /// True - Режим редактирования группы, иначе - добавление.
        /// </summary>
        public bool IsEditMode { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        public GroupEditorViewModel(Group group, bool isEditMode, IContainer container, IDialogManager dialogManager)
        {
            IsEditMode = isEditMode;
            OwnerIdentifier = container.ResolveRootDialogIdentifier();
            this.dialogManager = dialogManager;

            if (isEditMode)
                Group = (Group)group.Clone();
            else Group = new Group();

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
            var res = await dialogManager.SelectStaff(Group.CuratorId, this);

            if (res != null)
                Group.CuratorId = res.Id;
        }

        /// <summary>
        /// Закрытие диалога.
        /// </summary>
        private async void CloseDialog()
        {
            if (!Group.IsValid)
            {
                await this.ShowMessageBoxAsync(Group.Error, MaterialMessageBoxButtons.Ok);
                return;
            }

            OwnerIdentifier.Close(Group);
        }
    }
}
