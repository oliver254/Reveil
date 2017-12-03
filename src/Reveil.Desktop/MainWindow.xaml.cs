using GalaSoft.MvvmLight.Ioc;
using Reveil.Core;
using Reveil.Configuration;
using Reveil.Views;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using Reveil.Messages;

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
        }
        #endregion

        #region Méthodes
        private void ConfigureMinimizeMode()
        {
            var configuration = SimpleIoc.Default.GetInstance<ConfigurationStore>();
            if (configuration == null || !configuration.Minimize)
            {
                Activated -= Window_Activated;
                Deactivated -= Window_Deactivated;
                return;
            }

            Activated += Window_Activated;
            Deactivated += Window_Deactivated;
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
            Height = 390;
            Opacity = 1d;
            Width = 260;
        }

        private void Window_CloseClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Deactivated(object sender, System.EventArgs e)
        {
            Height = 260;
            Opacity = 0.5d;
            Width = 200;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ConfigureMinimizeMode();
        }
        #endregion
    }
}
