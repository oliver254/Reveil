using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Reveil.Configuration;
using Reveil.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reveil.ViewModels
{
    public class ConfigurationViewModel : ViewModelBase
    {
        #region Champs
        private readonly MainViewModel _parentVM;
        private readonly ConfigurationStore _configuration;

        private TimeSpan _selection;
        #endregion

        #region Constructeurs
        public ConfigurationViewModel(MainViewModel parentVM, ConfigurationStore configuration)
        {
            _parentVM = parentVM;
            _configuration = configuration;
            SelectedTime = DateTime.Now.TimeOfDay;

            // les commandes
            ActivateCommand = new RelayCommand(ExecuteActivateCommand);
            ResetCommand = new RelayCommand(ExecuteResetCommand);
            
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
        /// <summary>
        /// Obtient ou définit si le mode double écran est actif.
        /// </summary>
        public bool DualMode
        {
            get
            {
                return _configuration.DualMode;
            }
            set
            {
                _configuration.DualMode = value;
                RaisePropertyChanged(nameof(DualMode));
            }
        }

        /// <summary>
        /// Obtient ou définit la durée d'une longue durée.
        /// </summary>
        public int LongBreak
        {
            get
            {
                return _configuration.LongBreak;
            }
            set
            {
                _configuration.LongBreak = value;
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
                return _configuration.RingPath;
            }
            set
            {
                string ringPath = value;
                _configuration.RingPath = ringPath;
                RaisePropertyChanged(nameof(RingPath));
                _parentVM.RingPath = ringPath;
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
                return _configuration.ShortBreak;
            }
            set
            {
                _configuration.ShortBreak = value;
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
                return _configuration.Sprint;
            }
            set
            {
                _configuration.Sprint = value;
                RaisePropertyChanged(nameof(Sprint));
            }
        }
        #endregion

        #region Méthodes
        /// <summary>
        /// Execute la commande Activate (l'alarme
        /// </summary>
        private void ExecuteActivateCommand()
        {
            if(SelectedTime.CompareTo(DateTime.Now.TimeOfDay) <= 0)
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
            LongBreak = ConfigurationStore.DefaultLongBreak;
            ShortBreak = ConfigurationStore.DefaultShortBreak;
            Sprint = ConfigurationStore.DefaultSprint;
            RingPath = ConfigurationStore.DefaultRingPath;

        }
        #endregion
    }
}
