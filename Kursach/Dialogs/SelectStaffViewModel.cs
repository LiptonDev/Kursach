using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Dialogs.Attributes;
using ISTraining_Part.Dialogs.Classes;
using ISTraining_Part.Providers;
using MaterialDesignXaml.DialogsHelper;
using System.Linq;
using System.Windows.Data;

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
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public SelectStaffViewModel(int currentId, IDialogIdentifier dialogIdentifier, IClient client, IDataProvider dataProvider)
            : base(dialogIdentifier, client)
        {
            var staff = dataProvider.Staff;
            Items = new ListCollectionView(staff);

            SelectedItem = staff.FirstOrDefault(x => x.Id == currentId);
        }
    }
}
