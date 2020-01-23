using DevExpress.Mvvm;
using Kursach.DataBase;
using Kursach.Dialogs;
using MaterialDesignXaml.DialogsHelper;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Kursach.ViewModels
{
    /// <summary>
    /// Select group view model.
    /// </summary>
    [DialogName(nameof(Views.SelectGroupView))]
    class SelectGroupViewModel : ViewModelBase, IClosableDialog
    {
        /// <summary>
        /// Список всех групп.
        /// </summary>
        public ObservableCollection<Group> Groups { get; }

        /// <summary>
        /// Выбранная группа.
        /// </summary>
        public Group SelectedGroup { get; set; }

        /// <summary>
        /// Owner.
        /// </summary>
        public IDialogIdentifier OwnerIdentifier { get; }

        /// <summary>
        /// База данных.
        /// </summary>
        readonly IDataBase dataBase;

        /// <summary>
        /// Ctor.
        /// </summary>
        public SelectGroupViewModel(int currentId, IDialogIdentifier dialogIdentifier, IDataBase dataBase)
        {
            this.dataBase = dataBase;
            OwnerIdentifier = dialogIdentifier;

            Groups = new ObservableCollection<Group>();

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
            if (SelectedGroup == null)
                return;

            this.Close(SelectedGroup);
        }


        /// <summary>
        /// Загрузка всех групп.
        /// </summary>
        private async void Load(int currentId)
        {
            var res = await dataBase.GetGroupsAsync();
            Groups.AddRange(res);

            SelectedGroup = Groups.FirstOrDefault(x => x.Id == currentId);
        }
    }
}
