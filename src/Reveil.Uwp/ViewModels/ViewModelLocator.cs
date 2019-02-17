using System;

using GalaSoft.MvvmLight.Ioc;

using Reveil.Uwp.Services;
using Reveil.Uwp.Views;

namespace Reveil.Uwp.ViewModels
{
    [Windows.UI.Xaml.Data.Bindable]
    public class ViewModelLocator
    {
        #region Champs
        private static ViewModelLocator _current;
        #endregion

        #region Constructeurs
        private ViewModelLocator()
        {
            SimpleIoc.Default.Register(() => new NavigationServiceEx());
            SimpleIoc.Default.Register<ShellViewModel>();
            Register<MainViewModel, MainPage>();
            Register<SettingsViewModel, SettingsPage>();
        }
        #endregion

        #region Propriétés
        public static ViewModelLocator Current => _current ?? (_current = new ViewModelLocator());
        public MainViewModel MainViewModel => SimpleIoc.Default.GetInstance<MainViewModel>();
        public NavigationServiceEx NavigationService => SimpleIoc.Default.GetInstance<NavigationServiceEx>();
        public SettingsViewModel SettingsViewModel => SimpleIoc.Default.GetInstance<SettingsViewModel>();
        public ShellViewModel ShellViewModel => SimpleIoc.Default.GetInstance<ShellViewModel>();
        #endregion

        #region Méthodes
        public void Register<VM, V>()
            where VM : class
        {
            SimpleIoc.Default.Register<VM>();

            NavigationService.Configure(typeof(VM).FullName, typeof(V));
        }
        #endregion
    }
}
