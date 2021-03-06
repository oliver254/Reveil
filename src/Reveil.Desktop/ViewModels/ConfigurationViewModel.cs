﻿using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using NLog;
using Reveil.Configuration;
using Reveil.Messages;
using System;

namespace Reveil.ViewModels
{
    public class ConfigurationViewModel : ViewModelBase
    {
        #region Champs
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private TimeSpan _selection;
        #endregion

        #region Constructeurs
        public ConfigurationViewModel()
        {
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

        public ConfigurationStore Configuration => ServiceLocator.Current.GetInstance<ConfigurationStore>();

        /// <summary>
        /// Obtient ou définit la position horizontale de la fenêtre 
        /// </summary>
        public double Left
        {
            get
            {
                return Configuration.Left;
            }
            set
            {
                Configuration.Left = value;
                RaisePropertyChanged(nameof(Left));
            }
        }
                

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
                Configuration.RingPath = value;
                RaisePropertyChanged(nameof(RingPath));
            }
        }


        /// <summary>
        /// Obtient ou définit la position verticale de la fenêtre
        /// </summary>
        public double Top
        {
            get { return Configuration.Top; }
            set 
            {
                Configuration.Top = value;
                RaisePropertyChanged(nameof(Top));
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

        /// <summary>
        /// Obtient ou définit le mode transparent est actif.
        /// </summary>
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
        #endregion

        #region Méthodes
        /// <summary>
        /// Execute la commande Activate
        /// </summary>
        private void ExecuteActivateCommand()
        {
            _logger.Debug("Executing activate command...");
            try
            {
                var alarmDate = DateTime.Today.Add(SelectedTime);
                if (alarmDate < DateTime.Now)
                {
                    alarmDate = alarmDate.AddDays(1d);
                }

                Messenger.Default.Send<AlarmMessage>(new AlarmMessage(alarmDate));
                _logger.Info("Alarm is activated.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        /// <summary>
        /// Execute la commande Reset.
        /// </summary>
        private void ExecuteResetCommand()
        {
            _logger.Debug("Executing reset command...");
            try
            {
                LongBreak = ConfigurationStore.DefaultLongBreak;
                ShortBreak = ConfigurationStore.DefaultShortBreak;
                Sprint = ConfigurationStore.DefaultSprint;
                RingPath = ConfigurationStore.DefaultRingPath;
                _logger.Info("Reset is activated.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }
        #endregion
    }
}