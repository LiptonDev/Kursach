using DryIoc;
using Kursach.Core.Models;

namespace Kursach.Dialogs
{
    /// <summary>
    /// Signup view model.
    /// </summary>
    [DialogName(nameof(SignUpView))]
    class SignUpViewModel : BaseEditModeViewModel<User>
    {
        /// <summary>
        /// Конструктор для DesignTime.
        /// </summary>
        public SignUpViewModel()
        {

        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public SignUpViewModel(User user, bool isEditMode, IContainer container)
            : base(user, isEditMode, container) 
        { 
        }
    }
}
