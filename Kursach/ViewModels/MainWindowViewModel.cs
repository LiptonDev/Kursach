using DevExpress.Mvvm;
using DryIoc;
using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Dialogs.Manager;
using MaterialDesignThemes.Wpf;
using MaterialDesignXaml.DialogsHelper;
using Prism.Regions;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ISTraining_Part.ViewModels
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
        /// Менеджер диалогов.
        /// </summary>
        readonly IDialogManager dialogManager;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public MainWindowViewModel(IRegionManager regionManager,
                                   ISnackbarMessageQueue messageQueue,
                                   IClient client,
                                   IDialogManager dialogManager,
                                   IContainer container)
        {
            DialogIdentifier = container.ResolveRootDialogIdentifier();
            MessageQueue = messageQueue;

            this.client = client;
            this.regionManager = regionManager;
            this.dialogManager = dialogManager;

            this.client.HubConfigurator.Connected += Client_Connected;
            this.client.HubConfigurator.Disconnected += Client_Disconnected;
            this.client.HubConfigurator.Reconnected += Client_Reconnected;
            this.client.HubConfigurator.Reconnecting += Client_Reconnecting;

            this.client.HubConfigurator.ConnectAsync();
        }

        private void Client_Reconnecting()
        {
            Debug.WriteLine("Reconnecting");
            dialogManager.CloseChatWindow();
            DialogHelper.CloseAll();
            regionManager.RequestNavigateInRootRegion(RegionViews.ConnectingView);
        }

        private void Client_Reconnected()
        {
            Debug.WriteLine("Reconnected");
            regionManager.RequestNavigateInRootRegion(RegionViews.LoginView, NavigationParametersFluent.GetNavigationParameters().SetValue("fromConnecting", null));
        }

        private async void Client_Disconnected()
        {
            Debug.WriteLine("Disconnected");
            await Task.Delay(2000);
            await client.HubConfigurator.ConnectAsync();
        }

        private void Client_Connected()
        {
            Debug.WriteLine("Connected");
            regionManager.RequestNavigateInRootRegion(RegionViews.LoginView, NavigationParametersFluent.GetNavigationParameters().SetValue("fromConnecting", null));
        }
    }
}
