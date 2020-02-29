using ISTraining_Part.Client.Design;
using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Dialogs.Attributes;
using ISTraining_Part.Dialogs.Classes;
using ISTraining_Part.Providers;
using MaterialDesignXaml.DialogsHelper;
using System.Collections.ObjectModel;
using System.Linq;

namespace ISTraining_Part.Dialogs
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
            Items = new ObservableCollection<Staff>();
            var res = new DesignStaff().GetStaffsAsync().Result;
            Items.AddRange(res.Response);
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public SelectStaffViewModel(int currentId, IDialogIdentifier dialogIdentifier, IClient client, IDataProvider dataProvider)
            : base(dialogIdentifier, client)
        {
            Items = dataProvider.Staff;

            SelectedItem = Items.FirstOrDefault(x => x.Id == currentId);
        }
    }
}
