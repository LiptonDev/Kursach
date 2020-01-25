using DryIoc;
using Kursach.DataBase;
using Kursach.Dialogs;
using Kursach.Excel;
using Kursach.Models;
using Kursach.ViewModels;
using Kursach.Views;
using MaterialDesignXaml.DialogsHelper;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;

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

        protected override void OnStartup(StartupEventArgs e)
        {
            var culture = new CultureInfo("ru-RU");

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            base.OnStartup(e);
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.Register<MainWindow, MainWindowViewModel>();
            ViewModelLocationProvider.Register<LoginView, LoginViewModel>();
            ViewModelLocationProvider.Register<MainView, MainViewModel>();
            ViewModelLocationProvider.Register<GroupsView, GroupsViewModel>();
            ViewModelLocationProvider.Register<UsersView, UsersViewModel>();
            ViewModelLocationProvider.Register<StaffView, StaffViewModel>();
            ViewModelLocationProvider.Register<StudentsView, StudentsViewModel>();
            ViewModelLocationProvider.Register<WelcomeView, MainViewModel>();
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
            containerRegistry.RegisterSingleton<GroupsView>();
            containerRegistry.RegisterSingleton<StaffView>();
            containerRegistry.RegisterSingleton<StudentsView>();

            //vm's
            containerRegistry.RegisterSingleton<MainWindowViewModel>();
            containerRegistry.RegisterSingleton<LoginViewModel>();
            containerRegistry.RegisterSingleton<MainViewModel>();
            containerRegistry.RegisterSingleton<GroupsViewModel>();
            containerRegistry.RegisterSingleton<UsersViewModel>();
            containerRegistry.RegisterSingleton<StaffViewModel>();
            containerRegistry.RegisterSingleton<StudentsViewModel>();

            //dialogs
            containerRegistry.RegisterDelegate<IDialogIdentifier>(x => new DialogIdentifier("RootIdentifier"), Reuse.Singleton, "rootdialog");
            containerRegistry.RegisterSingleton<IViewFactory, ViewFactory>();
            containerRegistry.Register<FrameworkElement, SignInLogsView>(nameof(SignInLogsView));
            containerRegistry.Register<FrameworkElement, GroupEditorView>(nameof(GroupEditorView));
            containerRegistry.Register<FrameworkElement, SelectStaffView>(nameof(SelectStaffView));
            containerRegistry.Register<FrameworkElement, StaffEditorView>(nameof(StaffEditorView));
            containerRegistry.Register<FrameworkElement, StudentEditorView>(nameof(StudentEditorView));
            containerRegistry.Register<FrameworkElement, SignUpView>(nameof(SignUpView));

            //navigation
            containerRegistry.RegisterForNavigation<LoginView>(RegionViews.LoginView);
            containerRegistry.RegisterForNavigation<MainView>(RegionViews.MainView);
            containerRegistry.RegisterForNavigation<WelcomeView>(RegionViews.WelcomeView);
            containerRegistry.RegisterForNavigation<UsersView>(RegionViews.UsersView);
            containerRegistry.RegisterForNavigation<GroupsView>(RegionViews.GroupsView);
            containerRegistry.RegisterForNavigation<StaffView>(RegionViews.StaffView);
            containerRegistry.RegisterForNavigation<StudentsView>(RegionViews.StudentsView);

#if !design
            containerRegistry.RegisterSingleton<IDataBase, DataBase.DataBase>();
#else
            containerRegistry.RegisterSingleton<IDataBase, DesignDataBase>();
#endif

            //dialogs
            containerRegistry.RegisterSingleton<IDialogManager, DialogManager>();

            //excel
            containerRegistry.RegisterSingleton<IExporter<Group, IEnumerable<Student>>, GroupExporter>();
            containerRegistry.RegisterSingleton<IExporter<IEnumerable<Staff>>, StaffExporter>();
            containerRegistry.RegisterSingleton<IExporter<IEnumerable<Group>>, GroupsExporter>();
            containerRegistry.RegisterSingleton<IImporter<IEnumerable<Student>, Group>, GroupImporter>();
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
