using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using NLog;
using Reveil.Configuration;
using System;

namespace Reveil.ViewModels
{
    public class ConfigurationViewModel : ViewModelBase
    {
        #region Champs
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private MainViewModel _parentVM;
        private TimeSpan _selection;
        #endregion

        #region Constructeurs
        public ConfigurationViewModel()
        {
            SelectedTime = DateTime.Now.TimeOfDay;

            // les commandes
            ActivateCommand = new RelayCommand(ExecuteActivateCommand);
            ResetCommand = new RelayCommand(ExecuteResetCommand);
            TransparentCommand = new RelayCommand(() => ExecuteTransparentCommand());

        }
        #endregion

        #region Propriétés
        /// <summary>
        /// Obtient la commande Activate
        /// </summary>
        public RelayCommand ActivateCommand
        {
            get;
            private set;
        }
        public ConfigurationStore Configuration => ServiceLocator.Current.GetInstance<ConfigurationStore>();
        /// <summary>
        /// Obtient ou définit la durée d'une longue durée.
        /// </summary>
        public int LongBreak
        {
            get
            {
                return Configuration.LongBreak;
            }
            set
            {
                Configuration.LongBreak = value;
                RaisePropertyChanged(nameof(LongBreak));
            }
        }

        /// <summary>
        /// Obtient la commande Reset
        /// </summary>
        public RelayCommand ResetCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Obtient ou définit le chemin du fichier son.
        /// </summary>
        public string RingPath
        {
            get
            {
                return Configuration.RingPath;
            }
            set
            {
                string ringPath = value;
                Configuration.RingPath = ringPath;
                RaisePropertyChanged(nameof(RingPath));
                _parentVM.RaisePropertyChanged(nameof(RingPath));

            }
        }

        public TimeSpan SelectedTime
        {
            get
            {
                return _selection;
            }
            set
            {
                _selection = value;
                RaisePropertyChanged(nameof(SelectedTime));
            }
        }
        /// <summary>
        /// Obitent ou définit la durée d'une courte pause.
        /// </summary>
        public int ShortBreak
        {
            get
            {
                return Configuration.ShortBreak;
            }
            set
            {
                Configuration.ShortBreak = value;
                RaisePropertyChanged(nameof(ShortBreak));
            }
        }
        /// <summary>
        /// Obtient ou définit la durée d'un sprint.
        /// </summary>
        public int Sprint
        {
            get
            {
                return Configuration.Sprint;
            }
            set
            {
                Configuration.Sprint = value;
                RaisePropertyChanged(nameof(Sprint));
            }
        }
        public bool Transparent
        {
            get
            {
                return Configuration.Transparent;
            }
            set
            {
                Configuration.Transparent = value;
                RaisePropertyChanged(nameof(Transparent));
            }
        }
        /// <summary>
        /// Obtient la commande Transparent
        /// </summary>
        public RelayCommand TransparentCommand
        {
            get;
            private set;
        }
        #endregion

        #region Méthodes
        public void Initialize(MainViewModel parentViewModel)
        {
            _logger.Debug("Initializing...");
            _parentVM = parentViewModel;
        }
        /// <summary>
        /// Execute la commande Activate (l'alarme
        /// </summary>
        private void ExecuteActivateCommand()
        {
            _logger.Debug("Executing activate command...");
            if (SelectedTime.CompareTo(DateTime.Now.TimeOfDay) <= 0)
            {
                return;
            }

            TimeSpan duree = SelectedTime.Subtract(DateTime.Now.TimeOfDay);
            _parentVM.Duration = duree;
        }

        /// <summary>
        /// Execute la commande Reset.
        /// </summary>
        private void ExecuteResetCommand()
        {
            _logger.Debug("Executing reset command...");
            LongBreak = ConfigurationStore.DefaultLongBreak;
            ShortBreak = ConfigurationStore.DefaultShortBreak;
            Sprint = ConfigurationStore.DefaultSprint;
            RingPath = ConfigurationStore.DefaultRingPath;
        }
        /// <summary>
        /// Execute la commande Transparent
        /// </summary>
        private void ExecuteTransparentCommand()
        {
            _logger.Debug("Executing transparent command...");
            _parentVM.View.Activate();
            if (Transparent)
            {               
                _parentVM.View.ActivateTransparency();
            }
            else
            {
                _parentVM.View.DeactiveTransparency();
            }
            
        }
        #endregion
    }
}