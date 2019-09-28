using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

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

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                _mainDlg = new MainWindow();

                var value = SystemParameters.VirtualScreenLeft;
                if (value >= 0)
                {
                    value = SystemParameters.VirtualScreenWidth - _mainDlg.Width;
                }

                _mainDlg.Left = value;
                _mainDlg.Top = 16;
                _mainDlg.Show();
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }      

        private void Application_Exit(object sender, ExitEventArgs e)
        {

        }
    }
}
