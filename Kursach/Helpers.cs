using DryIoc;
using MaterialDesignXaml.DialogsHelper;
using Prism.Regions;

namespace Kursach
{
    static class RegionManagerHelper
    {
        public static void RequestNavigateInRootRegion(this IRegionManager regionManager, string view, NavigationParameters parameters = null)
        {
            regionManager.RequestNavigate(RegionNames.RootRegion, view, parameters);
        }

        public static void RequstNavigateInMainRegion(this IRegionManager regionManager, string view, NavigationParameters parameters = null)
        {
            regionManager.RequestNavigate(RegionNames.MainRegion, view, parameters);
        }

        public static IDialogIdentifier ResolveRootDialogIdentifier(this IContainer container)
        {
            return container.Resolve<IDialogIdentifier>("rootdialog");
        }
    }
}
