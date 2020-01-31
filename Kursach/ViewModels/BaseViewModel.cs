using DevExpress.Mvvm;
using DryIoc;
using Kursach.Client;
using Kursach.Dialogs;
using MaterialDesignThemes.Wpf;
using MaterialDesignXaml.DialogsHelper;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kursach.ViewModels
{
    abstract class BaseViewModel<T> : NavigationViewModel, IBaseViewModel<T>
    {
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
        /// Клиент сервера уведомлений.
        /// </summary>
        protected readonly IClient client;

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
                             IContainer container)
        {
            dialogIdentifier = container.ResolveRootDialogIdentifier();
            this.dialogManager = dialogManager;
            this.snackbarMessageQueue = snackbarMessageQueue;
            this.client = client;

            AddCommand = new DelegateCommand(Add);
            EditCommand = new AsyncCommand<T>(Edit);
            DeleteCommand = new AsyncCommand<T>(Delete);
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
