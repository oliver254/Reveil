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

            Messenger.Default.Register<DualMessage>(this, Window_MoveDualReceived);
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

        /// <summary>
        /// Déplace la fenêtre sur le deuxième écran.
        /// </summary>
        /// <param name="valeur"></param>
        private void MoveDualScreen(bool valeur)
        {
            Left = valeur ? SystemParameters.VirtualScreenWidth - Width : SystemParameters.WorkArea.Width - Width;
        }

        private void Window_CloseClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Top = 16;
            MoveDualScreen(GetDualMode());
        }

        /// <summary>
        /// Déplace la fenêtre sur le deuxième écran.
        /// </summary>
        /// <param name="message"></param>
        private void Window_MoveDualReceived(DualMessage message)
        {
            MoveDualScreen(message.Move);
        }
        #endregion

    }
}
