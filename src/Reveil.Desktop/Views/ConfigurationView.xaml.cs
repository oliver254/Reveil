using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

using Microsoft.Win32;

using Reveil.Desktop.Messages;
using Reveil.Desktop.ViewModels;

namespace Reveil.Desktop.Views
{
    /// <summary>
    /// Logique d'interaction pour ConfigurationView.xaml
    /// </summary>
    public partial class ConfigurationView : Window
    {
        #region Constructeurs
        public ConfigurationView()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
        #endregion

        #region Propriétés
        public ConfigurationViewModel ViewModel
        {
            get
            {
                return ViewModelLocator.Current.Configuration;
            }
        }
        #endregion

        #region Méthodes
        #endregion
    }
}
