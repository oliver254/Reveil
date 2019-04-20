using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

using GalaSoft.MvvmLight.Messaging;

using NLog;
using Reveil.Core;
using Reveil.Messages;

namespace Reveil.Controls
{
    /// <summary>
    /// Logique d'interaction pour ReveilClock.xaml
    /// </summary>
    public partial class ReveilClock : UserControl
    {
        #region Champs
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register(nameof(Duration),
                typeof(DateTime?),
                typeof(ReveilClock),
                new PropertyMetadata(new PropertyChangedCallback(Clock_DurationChanged)));

        public static readonly DependencyProperty RingPathProperty =
            DependencyProperty.Register(nameof(RingPath),
                typeof(string),
                typeof(ReveilClock),
                new PropertyMetadata(null, new PropertyChangedCallback(Clock_RingPathChanged)));

        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register(nameof(Time),
                typeof(string),
                typeof(ReveilClock),
                new PropertyMetadata(string.Empty));

        private const string DurationFormat = @"hh\:mm\:ss";
        private const string TimeFormat = "HH:mm:ss";
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private DateTime? _end;
        private ReveilState _state;
        private DispatcherTimer _timer;
        #endregion

        #region Constructeurs
        public ReveilClock()
        {
            InitializeComponent();
            State = ReveilState.Clock;
        }
        #endregion

        #region Evenements 
        public EventHandler<ReveilState> StateChanged;
        #endregion

        #region Propriétés
        /// <summary>
        /// Retourne ou définit la durée du chronomètre.
        /// </summary>
        public DateTime? Duration
        {
            get { return (DateTime?)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        /// <summary>
        /// Obtient ou définit le chemin du son de l'alarme.
        /// </summary>
        public string RingPath
        {
            get
            {
                return (string)GetValue(RingPathProperty);
            }
            set
            {
                SetValue(RingPathProperty, value);
            }
        }

        /// <summary>
        /// Obtient le temps de l'horloge.
        /// </summary>
        public string Time
        {
            get
            {
                return (string)GetValue(TimeProperty);
            }
            set
            {
                SetValue(TimeProperty, value);
            }
        }

        private ReveilState State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                OnStateChanged(_state);
            }
        }
        #endregion

        #region Méthodes
        public void Show()
        {
            _timer = new DispatcherTimer();
            _timer.Tick += Timer_Tick;
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();
        }
        private static void Clock_DurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var sender = d as ReveilClock;
            sender.SetDuration((DateTime?)args.NewValue);
        }

        private static void Clock_RingPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var sender = d as ReveilClock;
            sender.alarmMediaElement.Source = new Uri((string)args.NewValue);
        }

        private void OnStateChanged(ReveilState state)
        {
            StateChanged?.Invoke(this, state);
        }
        /// <summary>
        /// Plays the sound of the alarm.
        /// </summary>
        private void Play()
        {
            State = ReveilState.Alarm;
            alarmMediaElement.Play();
        }
        /// <summary>
        /// Sets the duration.
        /// </summary>
        /// <param name="duration"></param>
        private void SetDuration(DateTime? duration)
        {
            _timer.Stop();
            _end = duration;
            State = (duration != null) ? ReveilState.Timer : ReveilState.Clock;
            _timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            switch(State)
            {
                case ReveilState.Clock:
                    {
                        UpdateTime();
                        break;
                    }
                case ReveilState.Timer:
                    {
                        UpdateDuration();
                        break;
                    }
                case ReveilState.Play:
                    {
                        Play();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        /// <summary>
        /// Updates the duration.
        /// </summary>
        private void UpdateDuration()
        {
            DateTime now = DateTime.Now;
            if (_end.Value <= now)
            {
                State = ReveilState.Play;
            }
            var duration = _end.Value - DateTime.Now;
            secondTimeBar.Value = duration.Seconds;
            minuteTimeBar.Value = duration.Minutes;
            Time = duration.ToString(DurationFormat);
        }
        /// <summary>
        /// Updates the time.
        /// </summary>
        private void UpdateTime()
        {
            var time = DateTime.Now;
            secondTimeBar.Value = time.Second;
            minuteTimeBar.Value = time.Minute;
            Time = time.ToString(TimeFormat);
        }
        #endregion
    }
}
