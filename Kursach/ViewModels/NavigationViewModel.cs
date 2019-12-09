using DevExpress.Mvvm;
using Prism.Regions;

namespace Kursach.ViewModels
{
    class NavigationViewModel : ViewModelBase, INavigationAware
    {
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
        }
    }
}
