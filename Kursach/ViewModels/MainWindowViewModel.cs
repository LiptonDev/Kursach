using DevExpress.Mvvm;
using DryIoc;
using Kursach.NotifyClient;
using MaterialDesignThemes.Wpf;
using MaterialDesignXaml.DialogsHelper;
using Prism.Regions;

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
        /// Клиент сервера уведомлений.
        /// </summary>
        readonly INotifyClient notifyClient;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public MainWindowViewModel(IRegionManager regionManager,
                                   ISnackbarMessageQueue messageQueue,
                                   INotifyClient notifyClient,
                                   IContainer container)
        {
            DialogIdentifier = container.ResolveRootDialogIdentifier();
            MessageQueue = messageQueue;

            this.notifyClient = notifyClient;

            notifyClient.Connected += NotifyClient_Connected;
            notifyClient.Disconnected += NotifyClient_Disconnected;
            notifyClient.Reconnected += NotifyClient_Reconnected;

            notifyClient.ConnectAsync();
        }

        private void NotifyClient_Reconnected()
        {
            MessageQueue.Enqueue("Переподключен к серверу уведомлений");

            notifyClient.SetStatus(Consts.LoginStatus);
        }

        private void NotifyClient_Disconnected()
        {
            MessageQueue.Enqueue("Сервер уведомлений не доступен");

            notifyClient.ConnectAsync();
        }

        private void NotifyClient_Connected()
        {
            MessageQueue.Enqueue("Подключен к серверу уведомлений");

            notifyClient.SetStatus(Consts.LoginStatus);
        }
    }
}
