using DevExpress.Mvvm;
using Kursach.Client;
using MaterialDesignXaml.DialogsHelper;
using System.Collections.ObjectModel;
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
        public ObservableCollection<T> Items { get; }

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
        public BaseSelectorViewModel(int currentId, IDialogIdentifier dialogIdentifier, IClient client)
        {
            OwnerIdentifier = dialogIdentifier;
            this.client = client;

            Items = new ObservableCollection<T>();

            CloseDialogCommand = new DelegateCommand(CloseDialog);

            Load(currentId);
        }

        /// <summary>
        /// Команда закрытия диалога.
        /// </summary>
        public ICommand CloseDialogCommand { get; }

        /// <summary>
        /// Закрытие диалога.
        /// </summary>
        private void CloseDialog()
        {
            if (SelectedItem == null)
                return;

            this.Close(SelectedItem);
        }

        /// <summary>
        /// Загрузка данных.
        /// </summary>
        public abstract void Load(int currentId);
    }
}
