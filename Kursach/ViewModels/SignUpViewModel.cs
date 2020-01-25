using DryIoc;
using Kursach.Models;
using Kursach.Dialogs;

namespace Kursach.ViewModels
{
    /// <summary>
    /// Signup view model.
    /// </summary>
    [DialogName(nameof(Views.SignUpView))]
    class SignUpViewModel : BaseEditModeViewModel<User>
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        public SignUpViewModel(User user, bool isEditMode, IContainer container)
            : base(user, isEditMode, container) 
        { 
        }
    }
}
