using DryIoc;
using Kursach.DataBase;
using Kursach.Dialogs;
using Kursach.Models;
using Kursach.ViewModels;
using Kursach.Views;
using MaterialDesignXaml.DialogsHelper;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace Kursach
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
#if !design
            Container.Resolve<Context>().Users.AsNoTracking().Take(0);
#endif
            return Container.Resolve<MainWindow>();
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.Register<MainWindow>(() => Container.Resolve<MainWindowViewModel>());
            ViewModelLocationProvider.Register<LoginView>(() => Container.Resolve<LoginViewModel>());
            ViewModelLocationProvider.Register<MainView>(() => Container.Resolve<MainViewModel>());
            ViewModelLocationProvider.Register<SignUpView>(() => Container.Resolve<SignUpViewModel>());
            ViewModelLocationProvider.Register<GroupsView>(() => Container.Resolve<GroupsViewModel>());
            ViewModelLocationProvider.Register<UsersView, UsersViewModel>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //settings
            containerRegistry.RegisterDelegate<IProgramSettings>(x => ProgramSettings.Load(), Reuse.Singleton);

            //views
            containerRegistry.RegisterSingleton<LoginView>();
            containerRegistry.RegisterSingleton<MainView>();
            containerRegistry.RegisterSingleton<WelcomeView>();
            containerRegistry.RegisterSingleton<UsersView>();
            containerRegistry.RegisterSingleton<SignUpView>();
            containerRegistry.RegisterSingleton<GroupsView>();

            //vm's
            containerRegistry.RegisterSingleton<MainWindowViewModel>();
            containerRegistry.RegisterSingleton<LoginViewModel>();
            containerRegistry.RegisterSingleton<MainViewModel>();
            containerRegistry.RegisterSingleton<SignUpViewModel>();
            containerRegistry.RegisterSingleton<GroupsViewModel>();

            //dialogs
            containerRegistry.RegisterDelegate<IDialogIdentifier>(x => new DialogIdentifier("RootIdentifier"), Reuse.Singleton);
            containerRegistry.RegisterSingleton<IViewFactory, ViewFactory>();
            containerRegistry.Register<FrameworkElement, SignInLogsView>("SignInLogsView");

            //navigation
            containerRegistry.RegisterForNavigation<LoginView>(RegionViews.LoginView);
            containerRegistry.RegisterForNavigation<MainView>(RegionViews.MainView);
            containerRegistry.RegisterForNavigation<WelcomeView>(RegionViews.WelcomeView);
            containerRegistry.RegisterForNavigation<UsersView>(RegionViews.UsersView);
            containerRegistry.RegisterForNavigation<GroupsView>(RegionViews.GroupsView);

            //db
            containerRegistry.RegisterSingleton<Context>();
#if !design
            containerRegistry.RegisterSingleton<IDataBase, DataBase.DataBase>();
#else
            containerRegistry.RegisterSingleton<IDataBase, DesignDataBase>();
#endif

            //dialogs
            containerRegistry.RegisterSingleton<IDialogManager, DialogManager>();
        }
    }

    static class ContainerHelper
    {
        public static void RegisterDelegate<T>(this IContainerRegistry containerRegistry, Func<IResolverContext, T> func, IReuse reuse, string key = null)
        {
            containerRegistry.GetContainer().RegisterDelegate<T>(func, reuse, serviceKey: key);
        }
    }
}
