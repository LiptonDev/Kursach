using DevExpress.Mvvm;
using DryIoc;
using Kursach.Models;
using Kursach.Dialogs;
using MaterialDesignXaml.DialogsHelper;
using System.Windows.Input;
using Kursach.DataBase;

namespace Kursach.ViewModels
{
    abstract class BaseViewModel<T> : NavigationViewModel, IBaseViewModel<T>
    {
        /// <summary>
        /// База данных.
        /// </summary>
        protected readonly IDataBase dataBase;

        /// <summary>
        /// Идентификатор диалогов.
        /// </summary>
        protected readonly IDialogIdentifier dialogIdentifier;

        /// <summary>
        /// Менеджер диалогов.
        /// </summary>
        protected readonly IDialogManager dialogManager;

        public BaseViewModel(IDataBase dataBase, IDialogManager dialogManager, IContainer container)
        {
            this.dataBase = dataBase;
            dialogIdentifier = container.ResolveRootDialogIdentifier();
            this.dialogManager = dialogManager;

            AddCommand = new DelegateCommand(Add);
            EditCommand = new DelegateCommand<T>(Edit);
            DeleteCommand = new DelegateCommand<T>(Delete);
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
        public abstract void Edit(T obj);

        /// <summary>
        /// Удалить.
        /// </summary>
        public abstract void Delete(T obj);
    }
}
