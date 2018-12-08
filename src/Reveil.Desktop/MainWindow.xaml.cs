using Reveil.ViewModels;
using Reveil.Views;
using System.Windows;

namespace Reveil
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructeurs
        public MainWindow()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Initialize(this);
        }
        #endregion

        #region Propriétés        
        public MainViewModel ViewModel
        {
            get
            {
                return ViewModelLocator.Current.Main;
            }
        }
        #endregion

        #region Méthodes
        public void ActivateTransparency()
        {
            Activated += Window_Activated;
            Deactivated += Window_Deactivated;
        }

        public void DeactiveTransparency()
        {
            Activated -= Window_Activated;
            Deactivated -= Window_Deactivated;
        }

        private void MenuItemConfiguration_Click(object sender, RoutedEventArgs e)
        {
            ConfigurationView configDlg;

            configDlg = new ConfigurationView();
            configDlg.ViewModel.Initialize(ViewModel);
            configDlg.Owner = this;
            configDlg.ShowDialog();
        }

        private void Window_Activated(object sender, System.EventArgs e)
        {
            Opacity = 1d;
            sprintButton.Visibility = Visibility.Visible;
            shortBreakButton.Visibility = Visibility.Visible;
            longBreakButton.Visibility = Visibility.Visible;
            stopButton.Visibility = Visibility.Visible;        }

        private void Window_CloseClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Deactivated(object sender, System.EventArgs e)
        {
            Opacity = 0.5d;
            sprintButton.Visibility = Visibility.Collapsed;
            shortBreakButton.Visibility = Visibility.Collapsed;
            longBreakButton.Visibility = Visibility.Collapsed;
            stopButton.Visibility = Visibility.Collapsed;
        }


        #endregion
    }
}