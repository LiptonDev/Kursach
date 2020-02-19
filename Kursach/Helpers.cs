using ISTraining_Part.Core.Models;
using Prism.Regions;

namespace ISTraining_Part
{
    static class NavigationParametersFluent
    {
        public static NavigationParameters GetNavigationParameters()
        {
            return new NavigationParameters();
        }

        public static NavigationParameters SetUser(this NavigationParameters @params, User user)
        {
            @params.Add("user", user);
            return @params;
        }

        public static NavigationParameters SetValue(this NavigationParameters @params, string key, object value)
        {
            @params.Add(key, value);
            return @params;
        }
    }

    static class RegionManagerHelper
    {
        public static void RequestNavigateInRootRegion(this IRegionManager regionManager, string view, NavigationParameters parameters = null)
        {
            regionManager.RequestNavigate(RegionNames.RootRegion, view, parameters);
        }

        public static void ReqeustNavigateInMainRegion(this IRegionManager regionManager, string view, NavigationParameters parameters = null)
        {
            regionManager.RequestNavigate(RegionNames.MainRegion, view, parameters);
        }

        public static void RequstNavigateInMainRegionWithUser(this IRegionManager regionManager, string view, User user)
        {
            regionManager.ReqeustNavigateInMainRegion(view, NavigationParametersFluent.GetNavigationParameters().SetUser(user));
        }
    }
}
