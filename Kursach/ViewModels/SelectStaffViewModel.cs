using DevExpress.Mvvm;
using Kursach.Models;
using Kursach.Dialogs;
using MaterialDesignXaml.DialogsHelper;
using System.Collections.ObjectModel;
using System.Linq;
using Kursach.DataBase;

namespace Kursach.ViewModels
{
    /// <summary>
    /// Select staff view model.
    /// </summary>
    [DialogName(nameof(Views.SelectStaffView))]
    class SelectStaffViewModel : BaseSelectorViewModel<Staff>
    {
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
