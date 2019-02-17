using System;

using Reveil.Uwp.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Reveil.Uwp.Views
{
    // TODO WTS: Change the icons and titles for all NavigationViewItems in ShellPage.xaml.
    public sealed partial class ShellPage : Page
    {
        #region Constructeurs
        public ShellPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Initialize(shellFrame, navigationView, KeyboardAccelerators);
        }
        #endregion

        #region Propriétés
        private ShellViewModel ViewModel
        {
            get { return ViewModelLocator.Current.ShellViewModel; }
        }
        #endregion
    }
}
