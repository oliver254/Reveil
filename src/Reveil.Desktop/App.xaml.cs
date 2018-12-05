using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Reveil
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow mainDlg = new MainWindow();

            var value = SystemParameters.VirtualScreenLeft;
            if(value >= 0)
            {
                value = SystemParameters.VirtualScreenWidth - mainDlg.Width; 
            }
          


            mainDlg.Left = value;
            mainDlg.Top = 16;
            mainDlg.Show();
        }
    }
}
