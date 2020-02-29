using DevExpress.Mvvm;
using DryIoc;
using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Dialogs.Manager;
using ISTraining_Part.Providers;
using ISTraining_Part.ViewModels.Interfaces;
using MaterialDesignThemes.Wpf;
using MaterialDesignXaml.DialogsHelper;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ISTraining_Part.ViewModels.Classes
{
    abstract class BaseViewModel<T> : NavigationViewModel, IBaseViewModel<T>
    {
        /// <summary>
        /// Объекты.
        /// </summary>
        public ObservableCollection<T> Items { get; set; }

        /// <summary>
        /// Выбранный объект.
        /// </summary>
        public T SelectedItem { get; set; }

        /// <summary>
        /// Идентификатор диалогов.
        /// </summary>
        protected readonly IDialogIdentifier dialogIdentifier;

        /// <summary>
        /// Менеджер диалогов.
        /// </summary>
        protected readonly IDialogManager dialogManager;

        /// <summary>
        /// Очередь сообщений.
        /// </summary>
        protected readonly ISnackbarMessageQueue snackbarMessageQueue;

        /// <summary>
        /// Клиент сервера.
        /// </summary>
        protected readonly IClient client;

        /// <summary>
        /// Поставщик данных.
        /// </summary>
        protected readonly IDataProvider dataProvider;

        /// <summary>
        /// Конструктор для DesignTime.
        /// </summary>
        public BaseViewModel()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public BaseViewModel(IDialogManager dialogManager,
                             ISnackbarMessageQueue snackbarMessageQueue,
                             IClient client,
                             IDataProvider dataProvider,
                             IContainer container)
        {
            dialogIdentifier = container.ResolveRootDialogIdentifier();
            this.dialogManager = dialogManager;
            this.snackbarMessageQueue = snackbarMessageQueue;
            this.client = client;
            this.dataProvider = dataProvider;

            AddCommand = new DelegateCommand(Add);
            EditCommand = new AsyncCommand<T>(Edit, item => item != null);
            DeleteCommand = new AsyncCommand<T>(Delete, item => item != null);
        }

        /// <summary>
        /// Команда добавления.
        /// </summary>
        public ICommand AddCommand { get; }

        /// <summary>
        /// Команда открытия окна редактирования.
        /// </summary>
        public ICommand<T> EditCommand { get; }

        /// <summary>
        /// Команда удаления.
        /// </summary>
        public ICommand<T> DeleteCommand { get; }

        /// <summary>
        /// Добавить.
        /// </summary>
        public abstract void Add();

        /// <summary>
        /// Редактировать.
        /// </summary>
        public abstract Task Edit(T obj);

        /// <summary>
        /// Удалить.
        /// </summary>
        public abstract Task Delete(T obj);
    }
}
