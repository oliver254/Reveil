﻿using GalaSoft.MvvmLight.Ioc;
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

            Messenger.Default.Register<MoveDualMessage>(this, Window_MoveDualReceived);
        }
        #endregion

        #region Méthodes
        /// <summary>
        /// Obtient si le mode dual mode est actif.
        /// </summary>
        /// <returns></returns>
        private static bool GetDualMode()
        {
            return SimpleIoc.Default.GetInstance<ConfigurationStore>().DualMode;
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
            MoveDualScreen(bvaleur);

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
        private void Window_MoveDualReceived(MoveDualMessage message)
        {
            MoveDualScreen(message.Mode);
        }
        #endregion


    }
}