using DevExpress.Mvvm;
using DryIoc;
using ISTraining_Part.Core.ViewModels;
using ISTraining_Part.Dialogs.Interfaces;
using MaterialDesignXaml.DialogsHelper;
using System;
using System.Windows.Input;

namespace ISTraining_Part.Dialogs.Classes
{
    /// <summary>
    /// Base edit mode view model.
    /// </summary>
    class BaseEditModeViewModel<T> : ViewModelBase, IClosableDialog, IDialogIdentifier, IEditMode<T> where T : ValidateViewModel, ICloneable
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public string Identifier => nameof(BaseEditModeViewModel<T>);

        /// <summary>
        /// Owner.
        /// </summary>
        public IDialogIdentifier OwnerIdentifier { get; }

        /// <summary>
        /// Объект для редактирования.
        /// </summary>
        public T EditableObject { get; }

        /// <summary>
        /// True - Режим редактирования группы, иначе - добавление.
        /// </summary>
        public bool IsEditMode { get; }

        /// <summary>
        /// Конструктор для DesignTime.
        /// </summary>
        public BaseEditModeViewModel()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public BaseEditModeViewModel(T obj, bool isEditMode, IContainer container)
        {
            IsEditMode = isEditMode;
            OwnerIdentifier = container.ResolveRootDialogIdentifier();

            if (isEditMode)
                EditableObject = (T)obj.Clone();
            else EditableObject = container.Resolve<T>();

            CloseDialogCommand = new DelegateCommand(CloseDialog, IsObjectValid);
        }

        /// <summary>
        /// Указывает, прошел ли редактируемый объект валидацию.
        /// </summary>
        /// <returns></returns>
        private bool IsObjectValid() => EditableObject.IsValid;

        /// <summary>
        /// Команда закрытия диалога.
        /// </summary>
        public ICommand CloseDialogCommand { get; }

        /// <summary>
        /// Закрытие диалога.
        /// </summary>
        private void CloseDialog()
        {
            OwnerIdentifier.Close(EditableObject);
        }
    }
}
