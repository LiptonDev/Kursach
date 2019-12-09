using DevExpress.Mvvm;
using DryIoc;
using Kursach.Models;
using MaterialDesignXaml.DialogsHelper;
using Prism.Regions;
using System.Windows.Input;

namespace Kursach.ViewModels
{
    /// <summary>
    /// MainWindow view model.
    /// </summary>
    class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Настройки программы.
        /// </summary>
        public IProgramSettings Settings { get; }

        /// <summary>
        /// Идентификатор диалоговых окон.
        /// </summary>
        public IDialogIdentifier DialogIdentifier { get; }

        /// <summary>
        /// Менеджер регионов.
        /// </summary>
        readonly IRegionManager regionManager;

        /// <summary>
        /// Ctor.
        /// </summary>
        public MainWindowViewModel(IProgramSettings settings, IRegionManager regionManager, IContainer container)
        {
            Settings = settings;
            this.regionManager = regionManager;
            DialogIdentifier = container.ResolveRootDialogIdentifier();

            LoadCommand = new DelegateCommand(OnLoad);
            CloseCommand = new DelegateCommand(OnClose);
        }

        /// <summary>
        /// Команда открытия программы.
        /// </summary>
        public ICommand LoadCommand { get; }

        /// <summary>
        /// Команда закрытия программы.
        /// </summary>
        public ICommand CloseCommand { get; }

        /// <summary>
        /// Вызывается при закрытии окна программы.
        /// </summary>
        private void OnClose()
        {
            ProgramSettings.Save(Settings);

            Logger.Log.Info("Закрытие программы");
        }

        /// <summary>
        /// Вызывается при открытии окна программы.
        /// </summary>
        private void OnLoad()
        {
            regionManager.RequestNavigateInRootRegion(RegionViews.LoginView);

            Logger.Log.Info("Программа открыта");
        }
    }
}
