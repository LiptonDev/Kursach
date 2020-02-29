using DevExpress.Mvvm;
using ISTraining_Part.Core.Models;
using Prism.Regions;

namespace ISTraining_Part.ViewModels.Classes
{
    abstract class NavigationViewModel : ViewModelBase, INavigationAware
    {
        /// <summary>
        /// Текущий пользователь.
        /// </summary>
        public User User { get; set; }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("user"))
            {
                var user = navigationContext.Parameters["user"] as User;
                if (User == null)
                    User = new User();

                User.SetAllFields(user);
            }
        }
    }
}
