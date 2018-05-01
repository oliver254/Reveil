using GalaSoft.MvvmLight.Ioc;
using Reveil.Core;
using Reveil.Configuration;
using Reveil.Views;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using Reveil.Messages;
using Reveil.ViewModels;

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
            ChangeTransparent();
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.Transparent))
            {
                ChangeTransparent();
            }
        }
        #endregion

        #region Propriétés
        public MainViewModel ViewModel
        {
            get
            {
                return DataContext as MainViewModel;
            }

        }
        #endregion


        #region Méthodes
        private void ChangeTransparent()
        {

            if (!ViewModel.Transparent)
            {
                Activated -= Window_Activated;
                Deactivated -= Window_Deactivated;
            }
            else
            {
                Activated += Window_Activated;
                Deactivated += Window_Deactivated;
            }
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
            ChangeTransparent();
        }
        #endregion
    }
}
