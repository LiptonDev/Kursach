using DevExpress.Mvvm;
using Kursach.Client.Interfaces;
using MaterialDesignXaml.DialogsHelper;
using System.Windows.Data;
using System.Windows.Input;

namespace Kursach.Dialogs
{
    /// <summary>
    /// Base selector view model.
    /// </summary>
    abstract class BaseSelectorViewModel<T> : ViewModelBase, IClosableDialog, ISelectorViewModel<T>
    {
        /// <summary>
        /// Все данные.
        /// </summary>
        public ListCollectionView Items { get; protected set; }

        /// <summary>
        /// Выбранные данные.
        /// </summary>
        public T SelectedItem { get; set; }

        /// <summary>
        /// Owner.
        /// </summary>
        public IDialogIdentifier OwnerIdentifier { get; }

        /// <summary>
        /// Клиент.
        /// </summary>
        protected readonly IClient client;

        /// <summary>
        /// Конструктор для DesignTime.
        /// </summary>
        public BaseSelectorViewModel()
        {

        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public BaseSelectorViewModel(IDialogIdentifier dialogIdentifier, IClient client)
        {
            OwnerIdentifier = dialogIdentifier;
            this.client = client;

            CloseDialogCommand = new DelegateCommand(CloseDialog, IsSelected);
        }

        private bool IsSelected() => SelectedItem != null;

        /// <summary>
        /// Команда закрытия диалога.
        /// </summary>
        public ICommand CloseDialogCommand { get; }

        /// <summary>
        /// Закрытие диалога.
        /// </summary>
        private void CloseDialog()
        {
            this.Close(SelectedItem);
        }
    }
}
