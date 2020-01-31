using DevExpress.Mvvm;
using DryIoc;
using Kursach.Client.Interfaces;
using MaterialDesignThemes.Wpf;
using MaterialDesignXaml.DialogsHelper;
using Prism.Regions;
using System.Diagnostics;
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
            this.client.HubConfigurator.Reconnecting += HubConfigurator_Reconnecting;

            this.client.HubConfigurator.ConnectAsync();
        }

        private void HubConfigurator_Reconnecting()
        {
            Debug.WriteLine("Reconnecting");
            DialogHelper.CloseAll();
            regionManager.RequestNavigateInRootRegion(RegionViews.ConnectingView);
        }

        private void NotifyClient_Reconnected()
        {
            Debug.WriteLine("Reconnected");
            regionManager.RequestNavigateInRootRegion(RegionViews.LoginView, NavigationParametersFluent.GetNavigationParameters().SetValue("fromConnecting", null));
        }

        private async void NotifyClient_Disconnected()
        {
            Debug.WriteLine("Disconnected");
            await Task.Delay(2000);
            await client.HubConfigurator.ConnectAsync();
        }

        private void NotifyClient_Connected()
        {
            Debug.WriteLine("Connected");
            regionManager.RequestNavigateInRootRegion(RegionViews.LoginView, NavigationParametersFluent.GetNavigationParameters().SetValue("fromConnecting", null));
        }
    }
}
