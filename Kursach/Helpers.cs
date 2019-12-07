using Prism.Regions;

namespace Kursach
{
    static class RegionManagerHelper
    {
        public static void RequestNavigateInRootRegion(this IRegionManager regionManager, string view)
        {
            regionManager.RequestNavigate(RegionNames.RootRegion, view);
        }

        public static void RequstNavigateInMainRegion(this IRegionManager regionManager, string view)
        {
            regionManager.RequestNavigate(RegionNames.MainRegion, view);
        }
    }
}
