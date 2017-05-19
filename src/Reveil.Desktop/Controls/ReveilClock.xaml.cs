using Reveil.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
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
        /// Retourne ou définit le temps
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
        /// Retourne ou définit la couleur des secondes
        /// </summary>
        public Brush SecondColor
        {
            get { return (Brush)GetValue(SecondColorProperty); }
            set { SetValue(SecondColorProperty, value); }
        }

        /// <summary>
        /// Retourne ou définit
        /// </summary>
        public int StrokeThickness
        {
            get { return (int)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        #endregion

        #region Méthodes
        private static void Clock_PropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ReveilClock circle = sender as ReveilClock;
            circle.RenderArc();
        }

        private static void Clock_DurationChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ReveilClock clock = (ReveilClock)sender;
            clock.SetEnd((TimeSpan?)args.NewValue);
            
        }


        private void Clock_Loaded(object sender, RoutedEventArgs e)
        {
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

        private void RenderArc()
        {
            MathUtil.ConvertToAngle(_end, out double minutes, out double secondes);
            RenderArc(minutes, Radius, pathMinutes, pfMinutes, arcMinutes);

            double radius = Radius - (double)StrokeThickness;
            RenderArc(secondes, radius, pathSeconds, pfSeconds, arcSeconds);

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

        private void SetEnd(TimeSpan? duration)
        {
            if(duration == null)
            {
                _end = null;
                return;
            }

            _end = DateTime.Now.Add(duration.Value);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            RenderArc();
        }
        #endregion

    }
}
