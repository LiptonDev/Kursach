using DevExpress.Mvvm;
using DryIoc;
using Kursach.ViewModels;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using System;
using System.Windows.Input;

namespace Kursach.Dialogs
{
    /// <summary>
    /// Base edit mode view model.
    /// </summary>
    class BaseEditModeViewModel<T> : IClosableDialog, IDialogIdentifier, IEditMode<T> where T : ValidateViewModel, ICloneable
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
            if (!EditableObject.IsValid)
            {
                await this.ShowMessageBoxAsync(EditableObject.Error, MaterialMessageBoxButtons.Ok);
                return;
            }

            OwnerIdentifier.Close(EditableObject);
        }
    }
}
