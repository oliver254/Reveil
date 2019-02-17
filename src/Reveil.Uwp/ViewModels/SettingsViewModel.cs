using System;
using System.Windows.Input;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Reveil.Uwp.Helpers;
using Reveil.Uwp.Services;

using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace Reveil.Uwp.ViewModels
{
    // TODO WTS: Add other settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/settings.md
    public class SettingsViewModel : ViewModelBase
    {

        #region Champs
        private ElementTheme _elementTheme = ThemeSelectorService.Theme;
        private string _versionDescription;
        private ICommand _switchThemeCommand;
        #endregion

        #region Constructeurs
        public SettingsViewModel()
        {
        }
        #endregion

        #region Propriétés
        public ElementTheme ElementTheme
        {
            get { return _elementTheme; }

            set { Set(ref _elementTheme, value); }
        }

        public string VersionDescription
        {
            get { return _versionDescription; }

            set { Set(ref _versionDescription, value); }
        }
        public ICommand SwitchThemeCommand
        {
            get
            {
                if (_switchThemeCommand == null)
                {
                    _switchThemeCommand = new RelayCommand<ElementTheme>(
                        async (param) =>
                        {
                            ElementTheme = param;
                            await ThemeSelectorService.SetThemeAsync(param);
                        });
                }

                return _switchThemeCommand;
            }
        }
        #endregion

        #region Méthodes
        public void Initialize()
        {
            VersionDescription = GetVersionDescription();
        }
        private string GetVersionDescription()
        {
            var appName = "AppDisplayName".GetLocalized();
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
        #endregion
    }
}
