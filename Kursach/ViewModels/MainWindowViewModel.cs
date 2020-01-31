using DevExpress.Mvvm;
using DryIoc;
using Kursach.Client;
using MaterialDesignThemes.Wpf;
using MaterialDesignXaml.DialogsHelper;
using Prism.Regions;
using System.Threading.Tasks;

namespace Kursach.ViewModels
{
    /// <summary>
    /// MainWindow view model.
    /// </summary>
    class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Очередь сообщений.
        /// </summary>
        public ISnackbarMessageQueue MessageQueue { get; }

        /// <summary>
        /// Идентификатор диалоговых окон.
        /// </summary>
        public IDialogIdentifier DialogIdentifier { get; }

        /// <summary>
        /// Клиент.
        /// </summary>
        readonly IClient client;

        /// <summary>
        /// Менеджер регионов.
        /// </summary>
        readonly IRegionManager regionManager;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public MainWindowViewModel(IRegionManager regionManager,
                                   ISnackbarMessageQueue messageQueue,
                                   IClient client,
                                   IContainer container)
        {
            DialogIdentifier = container.ResolveRootDialogIdentifier();
            MessageQueue = messageQueue;

            this.client = client;
            this.regionManager = regionManager;

            this.client.HubConfigurator.Connected += NotifyClient_Connected;
            this.client.HubConfigurator.Disconnected += NotifyClient_Disconnected;
            this.client.HubConfigurator.Reconnected += NotifyClient_Reconnected;

            this.client.HubConfigurator.ConnectAsync();
        }

        private void NotifyClient_Reconnected()
        {
            System.Console.WriteLine("Reconnected!");
            regionManager.RequestNavigateInRootRegion(RegionViews.LoginView);
            //configurator.SetStatus(Consts.LoginStatus);
        }

        private async void NotifyClient_Disconnected()
        {
            System.Console.WriteLine("Disconnected!");
            regionManager.RequestNavigateInRootRegion(RegionViews.ConnectingView);
            await Task.Delay(2000);
            await client.HubConfigurator.ConnectAsync();
        }

        private void NotifyClient_Connected()
        {
            System.Console.WriteLine("Connected!");
            regionManager.RequestNavigateInRootRegion(RegionViews.LoginView);
            //configurator.SetStatus(Consts.LoginStatus);
        }
    }
}
