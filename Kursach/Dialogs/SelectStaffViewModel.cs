using DevExpress.Mvvm;
using Kursach.Client.Interfaces;
using Kursach.Core.Models;
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
        public SelectStaffViewModel(int currentId, IDialogIdentifier dialogIdentifier, IClient client)
            : base(currentId, dialogIdentifier, client)
        {
        }


        /// <summary>
        /// Загрузка всех сотрудников.
        /// </summary>
        public override async void Load(int currentId)
        {
            var res = await client.Staff.GetStaffsAsync();
            if (res)
            {
                Items.AddRange(res.Response);
                SelectedItem = Items.FirstOrDefault(x => x.Id == currentId);
            }
        }
    }
}
