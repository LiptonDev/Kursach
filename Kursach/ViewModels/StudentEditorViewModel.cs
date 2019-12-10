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
    /// Student editor view model.
    /// </summary>
    [DialogName(nameof(Views.StudentEditorView))]
    class StudentEditorViewModel : IClosableDialog, IDialogIdentifier, IEditMode
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public string Identifier => "StudentEditorViewModel";

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
        /// Студент.
        /// </summary>
        public Student Student { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        public StudentEditorViewModel(Student student, bool isEditMode, IContainer container, IDialogManager dialogManager)
        {
            IsEditMode = isEditMode;
            OwnerIdentifier = container.ResolveRootDialogIdentifier();
            this.dialogManager = dialogManager;

            if (isEditMode)
                Student = (Student)student.Clone();
            else Student = new Student();

            CloseDialogCommand = new DelegateCommand(CloseDialog);
            OpenGroupSelectorCommand = new DelegateCommand(OpenGroupSelector);
        }

        /// <summary>
        /// Команда закрытия диалога.
        /// </summary>
        public ICommand CloseDialogCommand { get; }

        /// <summary>
        /// Команда открытия выбора группы.
        /// </summary>
        public ICommand OpenGroupSelectorCommand { get; }

        /// <summary>
        /// Выбор группы.
        /// </summary>
        private async void OpenGroupSelector()
        {
            var res = await dialogManager.SelectGroup(Student.GroupId, this);

            if (res != null)
                Student.GroupId = res.Id;
        }

        /// <summary>
        /// Закрытие диалога.
        /// </summary>
        private async void CloseDialog()
        {
            if (!Student.IsValid)
            {
                await this.ShowMessageBoxAsync(Student.Error, MaterialMessageBoxButtons.Ok);
                return;
            }

            OwnerIdentifier.Close(Student);
        }
    }
}
