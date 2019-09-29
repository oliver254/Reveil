using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using NLog;
using Reveil.Desktop.Configuration;
using Reveil.Desktop.Messages;
using System;

namespace Reveil.Desktop.ViewModels
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
        private readonly ConfigurationViewModel _configViewModel;
        private DateTime? _duration;
        private MainWindow _view;
        #endregion

        #region Constructeurs
        public MainViewModel(ConfigurationViewModel configViewModel)
        {
            _logger.Debug("Buidling Main ViewMode...");
            _configViewModel = configViewModel;
            _configViewModel.PropertyChanged += ConfigViewModel_PropertyChanged;

            //les commandes
            LongBreakCommand = new RelayCommand(() => ExecuteCommand(Configuration.LongBreak));
            ShortBreakCommand = new RelayCommand(() => ExecuteCommand(Configuration.ShortBreak));
            SprintCommand = new RelayCommand(() => ExecuteCommand(Configuration.Sprint));
            StopCommand = new RelayCommand(ExecuteStopCommand);

            Messenger.Default.Register<AlarmMessage>(this, HandleAlarmMessage);
        }
        #endregion

        #region Propriétés
        public ConfigurationStore Configuration => SimpleIoc.Default.GetInstance<ConfigurationStore>();

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

            OnTransparencyChange(Configuration.Transparent);

            OnTopMostChange();
            _logger.Debug("Main ViewModel is initialized.");
        }

        /// <summary>
        /// Exécute la commande.
        /// </summary>
        /// <param name="duration"></param>
        private void ExecuteCommand(int duration)
        {
            _logger.Debug("Executing command...");
            Duration = DateTime.Now.AddMinutes(duration++);
            OnTopMostChange();
        }

        /// <summary>
        /// Execute la commande Stop.
        /// </summary>
        private void ExecuteStopCommand()
        {
            _logger.Debug("Executing stop command...");
            Duration = null;
            OnTopMostChange();
        }

        private void HandleAlarmMessage(AlarmMessage message)
        {
            if (message == null)
            {
                _logger.Warn("Alarm message is empty.");
                return;
            }
            Duration = message.Alarm;
        }

        private void ConfigViewModel_PropertyChanged(object d, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var sender = d as ConfigurationViewModel;
            if (e.PropertyName == nameof(ConfigurationViewModel.RingPath))
            {
                OnRingPathChange();
            }
            else if (e.PropertyName == nameof(ConfigurationViewModel.Transparent))
            {
                OnTransparencyChange(sender.Transparent);
            }
            else if(e.PropertyName == nameof(ConfigurationViewModel.TopMost))
            {
                OnTopMostChange();
            }
        }

        private void OnRingPathChange()
        {
            RaisePropertyChanged(nameof(RingPath));
        }

        private void OnTransparencyChange(bool transparent)
        {
            if (transparent)
            {
                _view.ActivateTransparency();
            }
            else
            {
                _view.DeactiveTransparency();
            }
        }

        private void OnTopMostChange()
        {
            bool topMost = Configuration.TopMost;
            if(_view.Topmost != topMost)
            {
                _view.Topmost = topMost;
            }
        }
        #endregion
    }
}