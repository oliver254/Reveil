using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Forms = System.Windows.Forms;


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
            var screens = Forms.Screen.AllScreens.OrderBy(s => s.WorkingArea.Left);
            var rightScreen = screens.Last();
            double value = rightScreen.WorkingArea.Right - mainDlg.Width;

            if(Forms.Screen.AllScreens.Length > 1 && rightScreen.Primary)
            {
                var leftScreen = screens.First();
                value = leftScreen.WorkingArea.Left;
            }
            mainDlg.Left = value;
            mainDlg.Top = 16;
            mainDlg.Show();
        }
    }
}
