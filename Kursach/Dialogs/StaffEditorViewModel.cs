using DryIoc;
using Kursach.Core.Models;

namespace Kursach.Dialogs
{
    /// <summary>
    /// Staff editor view model.
    /// </summary>
    [DialogName(nameof(StaffEditorView))]
    class StaffEditorViewModel : BaseEditModeViewModel<Staff>
    {
        /// <summary>
        /// Конструктор для DesignTime.
        /// </summary>
        public StaffEditorViewModel()
        {

        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public StaffEditorViewModel(Staff staff, bool isEditMode, IContainer container)
            : base(staff, isEditMode, container)
        {
        }
    }
}
