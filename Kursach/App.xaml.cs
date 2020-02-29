using DryIoc;
using ISTraining_Part.Client.Classes;
using ISTraining_Part.Client.Design;
using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Dialogs;
using ISTraining_Part.Dialogs.Manager;
using ISTraining_Part.Excel;
using ISTraining_Part.Properties;
using ISTraining_Part.Providers;
using ISTraining_Part.UI;
using ISTraining_Part.ViewModels;
using ISTraining_Part.Views;
using MaterialDesignThemes.Wpf;
using MaterialDesignXaml.DialogsHelper;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace ISTraining_Part
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

            bool isDark = Settings.Default.isDarkTheme;
            SetThemeColor.SetTheme(isDark);
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.Register<MainWindow, MainWindowViewModel>();
            ViewModelLocationProvider.Register<ChatWindow, ChatViewModel>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //client
#if !design
            containerRegistry.RegisterSingleton<IUsers, Users>();
            containerRegistry.RegisterSingleton<IStaff, Staffs>();
            containerRegistry.RegisterSingleton<IGroups, Groups>();
            containerRegistry.RegisterSingleton<IStudents, Students>();
            containerRegistry.RegisterSingleton<ILogin, Login>();
            containerRegistry.RegisterSingleton<IChat, Chat>();
            containerRegistry.RegisterSingleton<IDetailInfo, Client.Classes.DetailInfo>();
            containerRegistry.RegisterSingleton<IHubConfigurator, HubConfigurator>();
#else
            containerRegistry.RegisterSingleton<IUsers, DesignUsers>();
            containerRegistry.RegisterSingleton<IStaff, DesignStaff>();
            containerRegistry.RegisterSingleton<IGroups, DesignGroups>();
            containerRegistry.RegisterSingleton<IStudents, DesignStudents>();
            containerRegistry.RegisterSingleton<ILogin, DesignLogin>();
            containerRegistry.RegisterSingleton<IChat, DesignChat>();
            containerRegistry.RegisterSingleton<IDetailInfo, DesignDetailInfo>();
            containerRegistry.RegisterSingleton<IHubConfigurator, DesignConfigurator>();
#endif
            containerRegistry.RegisterSingleton<IClient, Client.Classes.Client>();

            //data provider
            containerRegistry.RegisterSingleton<IDataProvider, DataProvider>();

            //sync taskfactory
            containerRegistry.RegisterDelegate<TaskFactory>(x => new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext()), Reuse.Singleton);

            //snackbar
            containerRegistry.RegisterSingleton<ISnackbarMessageQueue, SnackbarMessageQueue>();

            //dialogs
            containerRegistry.RegisterDelegate<IDialogIdentifier>(x => new DialogIdentifier("RootIdentifier"), Reuse.Singleton, "rootdialog");
            containerRegistry.RegisterSingleton<IDialogManager, DialogManager>();
            containerRegistry.RegisterSingleton<IDialogsFactoryView, DialogsFactoryView>();
            containerRegistry.Register<FrameworkElement, SignInLogsView>(nameof(SignInLogsView));
            containerRegistry.Register<FrameworkElement, GroupEditorView>(nameof(GroupEditorView));
            containerRegistry.Register<FrameworkElement, SelectStaffView>(nameof(SelectStaffView));
            containerRegistry.Register<FrameworkElement, StaffEditorView>(nameof(StaffEditorView));
            containerRegistry.Register<FrameworkElement, StudentEditorView>(nameof(StudentEditorView));
            containerRegistry.Register<FrameworkElement, SignUpView>(nameof(SignUpView));
            containerRegistry.Register<FrameworkElement, DetailInfoView>(nameof(DetailInfoView));
            containerRegistry.Register<FrameworkElement, DetailInfoEditorView>(nameof(DetailInfoEditorView));

            //vm's
            containerRegistry.RegisterSingleton<MainViewModel>();
            containerRegistry.RegisterSingleton<ChatViewModel>();

            //navigation
            containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>(RegionViews.LoginView);
            containerRegistry.RegisterForNavigation<MainView, MainViewModel>(RegionViews.MainView);
            containerRegistry.RegisterForNavigation<UsersView, UsersViewModel>(RegionViews.UsersView);
            containerRegistry.RegisterForNavigation<GroupsView, GroupsViewModel>(RegionViews.GroupsView);
            containerRegistry.RegisterForNavigation<StaffView, StaffViewModel>(RegionViews.StaffView);
            containerRegistry.RegisterForNavigation<StudentsView, StudentsViewModel>(RegionViews.StudentsView);
            containerRegistry.RegisterForNavigation<ConnectingView>(RegionViews.ConnectingView);

            //excel
            containerRegistry.RegisterSingleton<IAsyncExporter<IEnumerable<IGrouping<Group, Student>>>, StudentsExporter>();
            containerRegistry.RegisterSingleton<IAsyncExporter<IEnumerable<IGrouping<Group, Student>>>, MinorStudentsExporter>("minor");
            containerRegistry.RegisterSingleton<IExporter<IEnumerable<Staff>>, StaffExporter>();
            containerRegistry.RegisterSingleton<IAsyncExporter<IEnumerable<Group>>, DivisionsContingentExporter>();
            containerRegistry.RegisterSingleton<IAsyncImporter<IEnumerable<Student>>, StudentsImporter>();
            containerRegistry.RegisterSingleton<IAsyncImporter<IEnumerable<Group>>, DivisionsContingentImporter>();

            //windows
            containerRegistry.Register<ChatWindow>();
        }
    }

    static class ContainerHelper
    {
        public static void RegisterDelegate<T>(this IContainerRegistry containerRegistry, Func<IResolverContext, T> func, IReuse reuse, string key = null)
        {
            containerRegistry.GetContainer().RegisterDelegate<T>(func, reuse, serviceKey: key);
        }

        public static IDialogIdentifier ResolveRootDialogIdentifier(this IContainer container)
        {
            return container.Resolve<IDialogIdentifier>("rootdialog");
        }
    }
}
