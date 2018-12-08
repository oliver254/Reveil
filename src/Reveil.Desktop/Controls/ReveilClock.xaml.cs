using GalaSoft.MvvmLight.Messaging;
using NLog;
using Reveil.Messages;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

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

        // Using a DependencyProperty as the backing store for SegmentColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinuteColorProperty =
            DependencyProperty.Register(nameof(MinuteColor),
                typeof(Brush),
                typeof(ReveilClock),
                new PropertyMetadata(new SolidColorBrush(Colors.Orange)));

        // Using a DependencyProperty as the backing store for Radius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register(nameof(Radius),
                typeof(int),
                typeof(ReveilClock),
                new PropertyMetadata(100, new PropertyChangedCallback(Clock_PropertyChanged)));

        public static readonly DependencyProperty RingPathProperty =
            DependencyProperty.Register(nameof(RingPath),
                typeof(string),
                typeof(ReveilClock),
                new PropertyMetadata(null, new PropertyChangedCallback(Clock_RingPathChanged)));

        // Using a DependencyProperty as the backing store for SegmentColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SecondColorProperty =
            DependencyProperty.Register(nameof(SecondColor),
                typeof(Brush),
                typeof(ReveilClock),
                new PropertyMetadata(new SolidColorBrush(Colors.OrangeRed)));

        // Using a DependencyProperty as the backing store for StrokeThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(nameof(StrokeThickness),
                typeof(int), typeof(ReveilClock),
                new PropertyMetadata(20, new PropertyChangedCallback(Clock_PropertyChanged)));

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
        /// Retourne ou définit la couleur des minutes
        /// </summary>
        public Brush MinuteColor
        {
            get { return (Brush)GetValue(MinuteColorProperty); }
            set { SetValue(MinuteColorProperty, value); }
        }

        /// <summary>
        /// Retourne ou définit le radius
        /// </summary>
        public int Radius
        {
            get { return (int)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
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
        /// Retourne ou définit la couleur des secondes
        /// </summary>
        public Brush SecondColor
        {
            get { return (Brush)GetValue(SecondColorProperty); }
            set { SetValue(SecondColorProperty, value); }
        }

        /// <summary>
        /// Obtient ou définit la largeur de la trainée des minutes ou des secondes.
        /// </summary>
        public int StrokeThickness
        {
            get { return (int)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
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
        private static void Clock_DurationChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ReveilClock clock = (ReveilClock)sender;
            clock.SetDuration((TimeSpan?)args.NewValue);
        }

        private static void Clock_PropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ReveilClock clock = (ReveilClock)sender;
            clock.RenderArc();
        }

        private static void Clock_RingPathChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ReveilClock clock = (ReveilClock)sender;
            clock.alarmMediaElement.Source = new Uri((string)args.NewValue);
        }

        private static void Clock_TimeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ReveilClock reveil = (ReveilClock)sender;
            reveil.TimeTextBlock.Text = (string)args.NewValue;
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

        private Point ComputeCartesianCoordinate(double angle, double radius)
        {
            // convert to radians
            double angleRad = (Math.PI / 180.0) * (angle - 90);

            double x = radius * Math.Cos(angleRad);
            double y = radius * Math.Sin(angleRad);

            return new Point(x, y);
        }

        private void ConvertToAngle(DateTime? end, out double minute, out double second)
        {
            DateTime now = DateTime.Now;
            minute = 0;
            second = 0;

            if (end == null)
            {
                minute = Convert.ToDouble(now.Minute);
                second = Convert.ToDouble(now.Second);
                Time = $"{now:HH:mm:ss}";
            }
            else if (end > now)
            {
                TimeSpan duration = end.Value.Subtract(now);
                minute = Convert.ToDouble(duration.Minutes);
                second = Convert.ToDouble(duration.Seconds);
                Time = $"{duration:hh\\:mm\\:ss}";
            }
            else if (!_alarme)
            {
                _logger.Debug("Starting alarm...");
                Messenger.Default.Send<AlarmMessage>(new AlarmMessage
                {
                    Time = now
                });
                alarmMediaElement.Play();
                _alarme = true;
                _logger.Info("Alarm is started.");
            }
            minute *= 6;
            second *= 6;
        }

        private void RenderArc()
        {
            ConvertToAngle(_end, out double minutes, out double secondes);
            RenderArc(secondes, Radius, pathSeconds, pfSeconds, arcSeconds);
            double radius = Radius - (double)StrokeThickness;
            RenderArc(minutes, radius, pathMinutes, pfMinutes, arcMinutes);
        }

        private void RenderArc(double angle, double radius, Path pathRoot, PathFigure pathFigure, ArcSegment arcSegment)
        {
            Point startPoint = new Point(radius, 0);
            Point endPoint = ComputeCartesianCoordinate(angle, radius);
            endPoint.X += radius;
            endPoint.Y += radius;

            pathRoot.Width = radius * 2 + StrokeThickness;
            pathRoot.Height = radius * 2 + StrokeThickness;
            pathRoot.Margin = new Thickness(StrokeThickness, StrokeThickness, 0, 0);

            bool largeArc = angle > 180;

            Size outerArcSize = new Size(radius, radius);

            pathFigure.StartPoint = startPoint;

            if (startPoint.X == Math.Round(endPoint.X) && startPoint.Y == Math.Round(endPoint.Y))
            {
                endPoint.X -= 0.01;
            }

            arcSegment.Point = endPoint;
            arcSegment.Size = outerArcSize;
            arcSegment.IsLargeArc = largeArc;
        }

        private void SetDuration(TimeSpan? duration)
        {
            _alarme = false;
            alarmMediaElement.Stop();

            if (duration == null)
            {
                _end = null;
                return;
            }
            _end = DateTime.Now.Add(duration.Value).AddSeconds(1);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            RenderArc();
        }

        #endregion
    }
}