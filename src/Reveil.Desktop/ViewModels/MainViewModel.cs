using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Reveil.Configuration;
using Reveil.Core;
using Reveil.Messages;
using System;
using System.Windows.Threading;

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
        private bool _dualMode;
        #endregion

        #region Constructeurs
        public MainViewModel(ConfigurationStore configuration)
        {
            _configuration = configuration;
            _ringPath = configuration.RingPath;
            _dualMode = _configuration.DualMode;

            //les commandes
            SprintCommand = new RelayCommand(() => ExecuteCommand(_configuration.Sprint));
            LongBreakCommand = new RelayCommand(() => ExecuteCommand(_configuration.LongBreak));
            ShortBreakCommand = new RelayCommand(() => ExecuteCommand(_configuration.ShortBreak));
            DualModeCommand = new RelayCommand(ExecuteDualModeCommand);
            StopCommand = new RelayCommand(ExecuteStopCommand);
        }
        #endregion

        #region Propriétés
        /// <summary>
        /// Obtient ou définit le mode double écran.
        /// </summary>
        public bool DualMode
        {
            get
            {
                return _dualMode;
            }
            set
            {
                _dualMode = value;
                _configuration.DualMode = _dualMode;
                MessengerInstance.Send<DualMessage>(new DualMessage
                {
                    Move = _dualMode
                });
                RaisePropertyChanged(nameof(DualMode));

            }
        }

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
        /// Obtient la commande DualMode
        /// </summary>
        public RelayCommand DualModeCommand
        {
            get;
            private set;
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
        private void ExecuteCommand(int duration)
        {
            Duration = TimeSpan.FromMinutes(duration);

        }

        private void ExecuteDualModeCommand()
        {
            DualMode = !_dualMode;
        }

        private void ExecuteStopCommand()
        {
            Duration = null;

        }
        #endregion

    }
}