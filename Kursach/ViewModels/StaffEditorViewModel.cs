using DryIoc;
using Kursach.Models;
using Kursach.Dialogs;

namespace Kursach.ViewModels
{
    /// <summary>
    /// Staff editor view model.
    /// </summary>
    [DialogName(nameof(Views.StaffEditorView))]
    class StaffEditorViewModel : BaseEditModeViewModel<Staff>
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        public StaffEditorViewModel(Staff staff, bool isEditMode, IContainer container)
            : base(staff, isEditMode, container)
        {
        }
    }
}
