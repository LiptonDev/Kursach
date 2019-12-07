using DryIoc;
using Kursach.Models;
using Kursach.ViewModels;
using Kursach.Views;
using MaterialDesignXaml.DialogsHelper;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Mvvm;
using System;
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
            return Container.Resolve<MainWindow>();
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.Register<MainWindow>(() => Container.Resolve<MainWindowViewModel>());
            ViewModelLocationProvider.Register<LoginView>(() => Container.Resolve<LoginViewModel>());
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //settings
            containerRegistry.RegisterDelegate<IProgramSettings>(x => ProgramSettings.Load(), Reuse.Singleton);

            //views
            containerRegistry.RegisterSingleton<LoginView>();

            //vm's
            containerRegistry.RegisterSingleton<MainWindowViewModel>();
            containerRegistry.RegisterSingleton<LoginViewModel>();

            //dialogs
            containerRegistry.RegisterDelegate<IDialogIdentifier>(x => new DialogIdentifier("RootIdentifier"), Reuse.Singleton);

            //navigation
            containerRegistry.RegisterForNavigation<LoginView>(RegionViews.LoginView);
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
