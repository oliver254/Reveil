using Reveil.Core;
using Reveil.Views;
using System.Windows;

namespace Reveil
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Champs
        #endregion

        #region Méthodes
        public MainWindow()
        {
            InitializeComponent();
        }
        private void DoDualScreen(bool valeur)
        {
            this.Left = valeur ? System.Windows.SystemParameters.VirtualScreenWidth - this.Width : System.Windows.SystemParameters.WorkArea.Width - this.Width;
        }
        private void MenuItemConfiguration_Click(object sender, RoutedEventArgs e)
        {
            ConfigurationView configDlg;

            configDlg = new ConfigurationView();
            configDlg.Owner = this;
            configDlg.ShowDialog();

        }
        private void MenuItemDual_Click(object sender, RoutedEventArgs e)
        {
            bool bvaleur;

            bvaleur = MenuItemDual.IsChecked;
            this.DoDualScreen(bvaleur);
            ConfigManager.Instance.SetDualScreen(bvaleur);
        }
        private void OnCloseClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            bool bdual;

            Top = 16;
            bdual = ConfigManager.Instance.GetDualScreen();
            DoDualScreen(bdual);
            MenuItemDual.IsChecked = bdual;

        }
        #endregion
    }
}
