using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Reveil.Configuration;
using System;

namespace Reveil.ViewModels
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Champs
        public const string RingPathPropertyName = "RingPath";

        private readonly ConfigurationStore _configuration;

        private TimeSpan? _duree;
        private string _ringPath;
        #endregion

        #region Constructeurs
        public MainViewModel(ConfigurationStore configuration)
        {
            _configuration = configuration;
            _ringPath = configuration.RingPath;

            //les commandes
            LongBreakCommand = new RelayCommand(() => ExecuteCommand(_configuration.LongBreak));
            MinimizeCommand = new RelayCommand(() => ExecuteMinimizeCommand());
            ShortBreakCommand = new RelayCommand(() => ExecuteCommand(_configuration.ShortBreak));
            SprintCommand = new RelayCommand(() => ExecuteCommand(_configuration.Sprint));
            StopCommand = new RelayCommand(ExecuteStopCommand);
        }
        #endregion

        #region Propriétés
        /// <summary>
        /// Obtient ou définit la durée.
        /// </summary>
        public TimeSpan? Duration
        {
            get
            {
                return _duree;
            }
            set
            {
                _duree = value;
                RaisePropertyChanged(nameof(Duration));
            }
        }

        /// <summary>
        /// Obtient la commande LongBreak.
        /// </summary>
        public RelayCommand LongBreakCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Obtient la commande Minimize
        /// </summary>
        public RelayCommand MinimizeCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Obtient l'état du mode Minimize
        /// </summary>
        public bool MinimizeMode
        {
            get
            {
                return GetMinimizeMode();
            }
            set
            {

            }
        }

        /// <summary>
        /// Retourne ou définit le chemin de la sonnerie
        /// </summary>
        public string RingPath
        {
            get
            {
                return _configuration.RingPath;
            }
            set
            {
                _ringPath = value;
                RaisePropertyChanged(nameof(RingPath));
            }
        }

        /// <summary>
        /// Obtient la commande ShortBreak.
        /// </summary>
        public RelayCommand ShortBreakCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Obtient la commande Sprint
        /// </summary>
        public RelayCommand SprintCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Obtient la commande Stop
        /// </summary>
        public RelayCommand StopCommand
        {
            get;
            private set;
        }
        #endregion

        #region Méthodes
        private static ConfigurationStore GetConfiguration()
        {
            return SimpleIoc.Default.GetInstance<ConfigurationStore>();
        }

        private void ExecuteCommand(int duration)
        {
            Duration = TimeSpan.FromMinutes(duration);
        }

        private void ExecuteMinimizeCommand()
        {
            SetMinimizeMode(!GetMinimizeMode());
        }

        private void ExecuteStopCommand()
        {
            Duration = null;
        }
        private bool GetMinimizeMode()
        {
            var configuration = GetConfiguration();
            if(configuration == null)
            {
                return false;
            }
            return configuration.Minimize;
        }

        private void SetMinimizeMode(bool value)
        {
            var configuration = GetConfiguration();
            if(configuration == null)
            {
                return;
            }
            configuration.Minimize = value;
        }
        #endregion
    }
}