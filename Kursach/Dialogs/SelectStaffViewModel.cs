using Kursach.Client.Interfaces;
using Kursach.Core.Models;
using Kursach.Providers;
using MaterialDesignXaml.DialogsHelper;
using System.Linq;
using System.Windows.Data;

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
        public SelectStaffViewModel(int currentId, IDialogIdentifier dialogIdentifier, IClient client, IDataProvider dataProvider)
            : base(dialogIdentifier, client)
        {
            var staff = dataProvider.Staff;
            Items = new ListCollectionView(staff);

            SelectedItem = staff.FirstOrDefault(x => x.Id == currentId);
        }
    }
}
