using DevExpress.Mvvm;
using Kursach.DataBase;
using Kursach.Dialogs;
using MaterialDesignXaml.DialogsHelper;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Kursach.ViewModels
{
    /// <summary>
    /// Select staff view model.
    /// </summary>
    [DialogName(nameof(Views.SelectStaffView))]
    class SelectStaffViewModel : ViewModelBase, IClosableDialog
    {
        /// <summary>
        /// Список всех 
        /// </summary>
        public ObservableCollection<Staff> Staff { get; }

        /// <summary>
        /// Выбранный куратор.
        /// </summary>
        public Staff SelectedStaff { get; set; }

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
        public SelectStaffViewModel(Staff currentStaff, IDialogIdentifier dialogIdentifier, IDataBase dataBase)
        {
            SelectedStaff = currentStaff;
            this.dataBase = dataBase;
            OwnerIdentifier = dialogIdentifier;

            Staff = new ObservableCollection<Staff>();

            CloseDialogCommand = new DelegateCommand(CloseDialog);

            LoadStaff();
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
            if (SelectedStaff == null)
                return;

            this.Close(SelectedStaff);
        }


        /// <summary>
        /// Загрузка всех работников.
        /// </summary>
        private async void LoadStaff()
        {
            var res = await dataBase.GetStaffsAsync();
            Staff.AddRange(res);
        }
    }
}
