﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Forms = System.Windows.Forms;

using NLog;

namespace Reveil
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private MainWindow _mainDlg;
        private Forms.NotifyIcon _notifyIcon;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                _mainDlg = new MainWindow();

                _notifyIcon = new Forms.NotifyIcon();
                _notifyIcon.DoubleClick += (s, args) => ActivateWindow();
                _notifyIcon.Icon = Reveil.Properties.Resources.Reveil;
                _notifyIcon.Visible = true;
                _mainDlg.Show();
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }      
        private void ActivateWindow()
        {
            _logger.Debug("Activating window...");
            _mainDlg.Activate();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            _notifyIcon.Visible = false;
            _notifyIcon.Icon = null;
        }
    }
}
