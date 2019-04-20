using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

using GalaSoft.MvvmLight.Messaging;

using NLog;

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
                typeof(TimeSpan?),
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
                new PropertyMetadata(string.Empty, new PropertyChangedCallback(Clock_TimeChanged)));

        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private bool _alarme = false;
        private DateTime? _end;
        private DispatcherTimer _timer;
        #endregion

        #region Constructeurs
        public ReveilClock()
        {
            InitializeComponent();
        }
        #endregion

        #region Propriétés
        /// <summary>
        /// Retourne ou définit la durée du chronomètre.
        /// </summary>
        public TimeSpan? Duration
        {
            get { return (TimeSpan?)GetValue(DurationProperty); }
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

        #endregion

        #region Méthodes
        private static void Clock_DurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var sender = d as ReveilClock;
            sender.SetDuration((TimeSpan?)args.NewValue);
        }

        private static void Clock_RingPathChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ReveilClock clock = (ReveilClock)sender;
            //clock.alarmMediaElement.Source = new Uri((string)args.NewValue);
        }

        private static void Clock_TimeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ReveilClock reveil = (ReveilClock)sender;
            //reveil.TimeTextBlock.Text = (string)args.NewValue;
        }

        private void Clock_Loaded(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }
            _timer = new DispatcherTimer();
            _timer.Tick += Timer_Tick;
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if(_end != null)
            {
                UpdateDuration();
            }
            else
            {
                UpdateTime();
            }
        }

        private void UpdateTime()
        {
            var time = DateTime.Now;
            secondTimeBar.Value = time.Second;
            minuteTimeBar.Value = time.Minute;
            Time = time.ToString("HH:mm:ss");
        }

        private void UpdateDuration()
        {
            var duration = _end.Value - DateTime.Now;
            secondTimeBar.Value = duration.Seconds;
            minuteTimeBar.Value = duration.Minutes;
        }


        private void SetDuration(TimeSpan? duration)
        {
            _alarme = false;

            if (duration == null)
            {
                _end = null;
                return;
            }
            _end = DateTime.Now.Add(duration.Value).AddSeconds(1);
        }
        #endregion
    }
}
