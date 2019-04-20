using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using Reveil.Core;
using Reveil.Messages;
using Reveil.ViewModels;
using Reveil.Views;

namespace Reveil
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Champs
        private const double Opaque = 1d;
        private const double Transparency = 0.5d;
        private ReveilState _state;
        #endregion

        #region Constructeurs
        public MainWindow()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Initialize(this);
            _state = ReveilState.Clock;
            reveil.Show();
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
            Opacity = Transparency;
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
            configDlg.Owner = this;
            configDlg.ShowDialog();
        }

        private void Window_Activated(object sender, System.EventArgs e)
        {
            Opacity = Opaque;
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
            Opacity = Transparency;
            sprintButton.Visibility = Visibility.Collapsed;
            shortBreakButton.Visibility = Visibility.Collapsed;
            longBreakButton.Visibility = Visibility.Collapsed;
            stopButton.Visibility = Visibility.Collapsed;
        }
        #endregion
    }
}
