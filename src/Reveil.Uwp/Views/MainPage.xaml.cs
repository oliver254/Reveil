using System;

using Reveil.Uwp.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Reveil.Uwp.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel
        {
            get { return ViewModelLocator.Current.MainViewModel; }
        }

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
