using ISTraining_Part.Properties;
using Prism.Regions;
using System.Windows;

namespace ISTraining_Part
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Менеджер регионов.
        /// </summary>
        readonly IRegionManager regionManager;

        public MainWindow(IRegionManager regionManager)
        {
            InitializeComponent();

            this.regionManager = regionManager;

            var settings = Settings.Default;
            Left = settings.left;
            Top = settings.top;
            Width = settings.width;
            Height = settings.height;

            if (settings.state == WindowState.Minimized)
                settings.state = WindowState.Normal;

            WindowState = settings.state;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var settings = Settings.Default;
            settings.left = Left;
            settings.top = Top;
            settings.width = Width;
            settings.height = Height;
            settings.state = WindowState;

            settings.Save();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            regionManager.RequestNavigateInRootRegion(RegionViews.ConnectingView);
        }
    }
}
