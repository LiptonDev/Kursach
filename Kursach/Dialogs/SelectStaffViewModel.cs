using DevExpress.Mvvm;
using Kursach.DataBase;
using Kursach.Models;
using MaterialDesignXaml.DialogsHelper;
using System.Collections.ObjectModel;
using System.Linq;

namespace Kursach.Dialogs
{
    /// <summary>
    /// Select staff view model.
    /// </summary>
    [DialogName(nameof(SelectStaffView))]
    class SelectStaffViewModel : BaseSelectorViewModel<Staff>
    {
        /// <summary>
        /// Конструктор для DesignTime.
        /// </summary>
        public SelectStaffViewModel()
        {

        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public SelectStaffViewModel(int currentId, IDialogIdentifier dialogIdentifier, IDataBase dataBase)
            : base(currentId, dialogIdentifier, dataBase)
        {
        }


        /// <summary>
        /// Загрузка всех сотрудников.
        /// </summary>
        public override async void Load(int currentId)
        {
            var res = await dataBase.GetStaffsAsync();
            Items.AddRange(res);

            SelectedItem = Items.FirstOrDefault(x => x.Id == currentId);
        }
    }
}
