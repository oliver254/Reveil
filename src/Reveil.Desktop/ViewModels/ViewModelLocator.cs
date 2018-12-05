using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using Reveil.Configuration;

namespace Reveil.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary> 
    public class ViewModelLocator
    {
        #region Champs
        private static ViewModelLocator _current;
        #endregion

        #region Constructeurs
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // services
            SimpleIoc.Default.Register<ConfigurationStore>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ConfigurationViewModel>();
        }
        #endregion

        #region Propriétés
        /// <summary>
        /// Obtient le localisateur actuel.
        /// </summary>
        public static ViewModelLocator Current => _current ?? (_current = new ViewModelLocator());

        /// <summary>
        /// Obtient le modèle de la vue Configuration
        /// </summary>
        public ConfigurationViewModel Configuration => ServiceLocator.Current.GetInstance<ConfigurationViewModel>();

        /// <summary>
        /// Obtient le modèle de la vue principale
        /// </summary>
        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        #endregion

        #region Méthodes
        public static void Cleanup()
        {
            SimpleIoc.Default.GetInstance<MainViewModel>().Cleanup();
        }
        #endregion

    }
}