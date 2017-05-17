using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Reveil.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reveil.ViewModels
{
    public class PomodoroViewModel : ViewModelBase
    {
        #region Champs
        public const string AlarmPropertyName = "Alarm";
        public const string IsPomodoroPropertyName = "IsPomodoroMode";
        public const string LongBreakPropertyName = "LongBreak";
        public const string RingPathPropertyName = "RingPath";
        public const string ShortBreakPropertyName = "ShortBreak";
        public const string TimePropertyName = "Time";

        private bool _pomodoroMode, _alarm;
        private DateTime _dtTime, _alarme;
        private ClockState _etat;
        private string _ringPath;
        private int _sprint;
        private int _longBreak;
        private int _shortBreak;
        #endregion

        #region Constructeurs
        public PomodoroViewModel()
        {
            _etat = ClockState.Clock;
            _pomodoroMode = ConfigManager.Instance.GetPomodoroMode();
            _ringPath = ConfigManager.Instance.GetRingPath();
            _sprint = ConfigManager.Instance.GetSprint();
            _longBreak = ConfigManager.Instance.GetLongBreak();
            _shortBreak = ConfigManager.Instance.GetShortBreak();


            //les commandes 
            this.PomodoroModeCommand = new RelayCommand(() => this.ExecutePomodoroModeCommand());
            this.PomodoroCommand = new RelayCommand(() => this.ExecutePodomodoroCommand(_sprint));
            this.ShortBreakCommand = new RelayCommand(() => this.ExecutePodomodoroCommand(_shortBreak));
            this.LongBreakCommand = new RelayCommand(() => this.ExecutePodomodoroCommand(_longBreak));
            this.StopCommand = new RelayCommand(() => this.ExecuteStopCommand());
        }
        #endregion

        #region Propriétés      
        public bool Alarm
        {
            get { return _alarm; }
            set
            {
                _alarm = value;
                this.RaisePropertyChanged(AlarmPropertyName);
            }
        }
        /// <summary>
        /// Retourne ou définit si l'application est en mode Pomodoro
        /// </summary>
        public bool IsPomodoroMode
        {
            get { return _pomodoroMode; }
            set
            {
                _pomodoroMode = value;
                this.RaisePropertyChanged(IsPomodoroPropertyName);
            }
        }

        /// <summary>
        /// Retourne ou définit la durée d'un long break
        /// </summary>
        public int LongBreak
        {
            get { return _longBreak; }
            set
            {
                _longBreak = value;
                RaisePropertyChanged(LongBreakPropertyName);
                ConfigManager.Instance.SetLongBreak(_longBreak);
            }
        }

        /// <summary>
        /// Retourne la commande LongBreak
        /// </summary>
        public RelayCommand LongBreakCommand
        {
            get;
            private set;
        }
        /// <summary>
        /// Retourne ou définit le chemin du bruit
        /// </summary>
        public string RingPath
        {
            get { return _ringPath; }
            set
            {
                _ringPath = value;
                RaisePropertyChanged(RingPathPropertyName);
            }

        }
        /// <summary>
        /// Retouren la commande Pomodoro
        /// </summary>
        public RelayCommand PomodoroCommand
        {
            get;
            private set;
        }
        /// <summary>
        /// Retourne la commande PomodoroMode
        /// </summary>
        public RelayCommand PomodoroModeCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Retourne ou définit la durée d'un short break
        /// </summary>
        public int ShortBreak
        {
            get { return _shortBreak; }
            set
            {
                _shortBreak = value;
                RaisePropertyChanged(ShortBreakPropertyName);
                ConfigManager.Instance.SetShortBreak(_shortBreak);
            }
        }

        /// <summary>
        /// Retourne la commande ShortBreak
        /// </summary>
        public RelayCommand ShortBreakCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Retourne le temps défini pour un sprint
        /// </summary>
        public int Sprint
        {
            get { return _sprint; }
            set
            {
                _sprint = value;
            }
        }
        /// <summary>
        /// Retourne ou définit l'état 
        /// </summary>
        private ClockState State
        {
            get { return _etat; }
            set
            {
                _etat = value;
                this.OnState();
            }
        }
        /// <summary>
        /// Retourne la commande Stop
        /// </summary>
        public RelayCommand StopCommand
        {
            get;
            private set;
        }
        /// <summary>
        /// Retourne ou définit l'horloge
        /// </summary>
        public DateTime Time
        {
            get { return _dtTime; }
            set
            {
                _dtTime = value;
                RaisePropertyChanged(TimePropertyName);
            }
        }
        #endregion

        #region Méthodes
        public void OnTick(object sender, System.EventArgs e)
        {
            DateTime dtResult;
            TimeSpan ts;

            dtResult = DateTime.Now;
            if (this.State == ClockState.StopWatch)
            {
                ts = _alarme - DateTime.Now;
                if (ts.Ticks > 0)
                {
                    dtResult = new DateTime(ts.Ticks);
                }
                else
                {
                    dtResult = new DateTime();
                    this.State = ClockState.Alarm;
                }
            }
            else if (this.State == ClockState.Alarm)
            {
                dtResult = new DateTime();
            }
            this.Time = dtResult;
        }
        /// <summary>
        /// Remet les durées par défaut
        /// </summary>
        public void Reset()
        {
            this.Sprint = 25;
            this.LongBreak = 15;
            this.ShortBreak = 5;
            IsPomodoroMode = true;
        }

        /// <summary>
        /// Exécute la commande PomodoroMode
        /// </summary>
        private void ExecutePomodoroModeCommand()
        {
            this.IsPomodoroMode = !_pomodoroMode;
            ConfigManager.Instance.Config.AppSettings.Settings[Constants.ModePomodoro].Value = _pomodoroMode.ToString();
            ConfigManager.Instance.Config.Save(ConfigurationSaveMode.Modified);
        }
        private void ExecutePodomodoroCommand(int valeur)
        {
            _alarme = DateTime.Now.AddMinutes(valeur).AddSeconds(1);
            this.State = ClockState.StopWatch;
        }
        /// <summary>
        /// Exécute la commande Stop
        /// </summary>
        private void ExecuteStopCommand()
        {
            this.State = ClockState.Clock;
        }
        /// <summary>
        /// Modifie l'état du composant
        /// </summary>
        private void OnState()
        {
            this.Alarm = _etat == ClockState.Alarm ? true : false;

        }
        #endregion
    }
}
