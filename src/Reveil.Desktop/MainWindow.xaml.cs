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
        /// <summary>
        /// Obtient si le mode dual mode est actif.
        /// </summary>
        /// <returns></returns>
        private static bool GetDualMode()
        {
            bool reponse = SimpleIoc.Default.GetInstance<ConfigurationStore>().DualMode;
            return reponse;
        }


        private void MenuItemConfiguration_Click(object sender, RoutedEventArgs e)
        {
            ConfigurationView configDlg;

            configDlg = new ConfigurationView();
            configDlg.Owner = this;
            configDlg.ShowDialog();

        }


        private void Window_CloseClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion

    }
}
