﻿using System;

using CommonServiceLocator;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using NLog;

using Reveil.Configuration;
using Reveil.Messages;

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
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private DateTime? _duration;
        private MainWindow _view;
        #endregion

        #region Constructeurs
        public MainViewModel()
        {
            _logger.Debug("Buidling Main ViewMode...");
            //les commandes
            LongBreakCommand = new RelayCommand(() => ExecuteCommand(Configuration.LongBreak));
            ShortBreakCommand = new RelayCommand(() => ExecuteCommand(Configuration.ShortBreak));
            SprintCommand = new RelayCommand(() => ExecuteCommand(Configuration.Sprint));
            StopCommand = new RelayCommand(ExecuteStopCommand);

            Messenger.Default.Register<AlarmMessage>(this, HandleAlarmMessage);
            Messenger.Default.Register<RingMessage>(this, HandleRingMessage);
        }
        #endregion

        #region Propriétés
        public ConfigurationStore Configuration => ServiceLocator.Current.GetInstance<ConfigurationStore>();

        /// <summary>
        /// Obtient ou définit la durée.
        /// </summary>
        public DateTime? Duration
        {
            get
            {
                return _duration;
            }
            set
            {
                _duration = value;
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
        /// Retourne ou d�finit le chemin de la sonnerie
        /// </summary>
        public string RingPath
        {
            get
            {
                return Configuration.RingPath;
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

        public MainWindow View => _view;
        #endregion

        #region Méthodes
        /// <summary>
        /// Initialise le modèle de vue avec la vue spécifiée.
        /// </summary>
        /// <param name="view"></param>
        public void Initialize(MainWindow view)
        {
            _logger.Debug("Initializing...");
            _view = view;

            if(Configuration.Transparent)
            {
                _view.WindowStyle = System.Windows.WindowStyle.None;
                _view.AllowsTransparency = true;
                _view.ActivateTransparency();
            }
            _logger.Debug("Main ViewModel is initialized.");
        }

        /// <summary>
        /// Ex�cute la commande.
        /// </summary>
        /// <param name="duration"></param>
        private void ExecuteCommand(int duration)
        {
            _logger.Debug("Executing command...");
            Duration = DateTime.Now.AddMinutes(duration++);
        }


        /// <summary>
        /// Execute la commande Stop.
        /// </summary>
        private void ExecuteStopCommand()
        {
            _logger.Debug("Executing stop command...");
            Duration = null;
        }

        private void HandleAlarmMessage(AlarmMessage message)
        {
            if(message == null)
            {
                _logger.Warn("Alarm message is empty.");
                return;
            }
            Duration = message.Alarm;
        }

        private void HandleRingMessage(RingMessage message)
        {
            if(message == null)
            {
                _logger.Warn("Ring path message is empty.");
                return;
            }
            RaisePropertyChanged(nameof(RingPath));
        }
        #endregion
    }
}
