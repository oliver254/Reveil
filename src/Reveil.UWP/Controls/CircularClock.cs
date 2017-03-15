using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Reveil.UWP.Controls
{
    [TemplatePart(Name = ContainerPartName, Type = typeof(Grid))]
    [TemplatePart(Name = ScalePartName, Type = typeof(Path))]
    [TemplatePart(Name = TrailPartName, Type = typeof(Path))]
    public sealed class CircularClock : Control
    {
        #region Champs
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register(nameof(Duration), typeof(TimeSpan?), typeof(CircularClock), new PropertyMetadata(null));

        public static readonly DependencyProperty MinuteAngleProperty =
            DependencyProperty.Register(nameof(MinuteAngle), typeof(double), typeof(CircularClock), new PropertyMetadata(0d, OnMinuteAngleChanged));

        public static readonly DependencyProperty RadiusProperty = 
            DependencyProperty.Register(nameof(Radius), typeof(int), typeof(CircularClock), new PropertyMetadata(25));

        public static readonly DependencyProperty ScaleBrushProperty =
            DependencyProperty.Register(nameof(ScaleBrush), typeof(Brush), typeof(CircularClock), new PropertyMetadata(new SolidColorBrush(Colors.DarkGray)));

        public static readonly DependencyProperty SecondAngleProperty =
            DependencyProperty.Register(nameof(SecondAngle), typeof(double), typeof(CircularClock), new PropertyMetadata(0d, OnSecondAngleChanged));


        public static readonly DependencyProperty StrokeThicknessProperty = 
            DependencyProperty.Register("StrokeThickness", typeof(int), typeof(CircularClock), new PropertyMetadata(5));


        public static readonly DependencyProperty TrailBrushProperty =
            DependencyProperty.Register(nameof(TrailBrush), typeof(Brush), typeof(CircularClock), new PropertyMetadata(new SolidColorBrush(Colors.DarkBlue)));

        private const string ContainerPartName = "PART_Container";
        private const string ScalePartName = "PART_Scale";
        private const string TrailPartName = "PART_Trail";

        private DateTime? _end;
        private DispatcherTimer _timer;
        #endregion

        #region Constructeurs
        public CircularClock()
        {
            DefaultStyleKey = typeof(CircularClock);
            _timer = new DispatcherTimer();
            _timer.Tick += Timer_Tick;
        }


        #endregion

        #region Propriétés
        /// <summary>
        /// Obtient ou définit la durée
        /// </summary>
        public TimeSpan? Duration
        {
            get { return (TimeSpan?)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        /// <summary>
        /// Obtient ou définit l'angle minute
        /// </summary>
        public double MinuteAngle
        {
            get { return (double)GetValue(MinuteAngleProperty); }
            set { SetValue(MinuteAngleProperty, value); }
        }

        /// <summary>
        /// Obtient ou définit le rayon
        /// </summary>
        public int Radius
        {
            get { return (int)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }


        public Brush ScaleBrush
        {
            get { return (Brush)GetValue(ScaleBrushProperty); }
            set { SetValue(ScaleBrushProperty, value); }
        }
        
        /// <summary>
        /// Obtient ou définit l'angle seconde
        /// </summary>
        public double SecondAngle
        {
            get { return (double)GetValue(SecondAngleProperty); }            
            set { SetValue(SecondAngleProperty, value); }
        }

        public int StrokeThickness
        {
            get { return (int)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public Brush TrailBrush
        {
            get { return (Brush)GetValue(TrailBrushProperty); }
            set { SetValue(TrailBrushProperty, value); }
        }
        #endregion

        #region Méthodes
        /// <summary>
        /// Lance la montre
        /// </summary>
        public void Start()
        {
            _timer.Stop();
            if(Duration != null)
            {
                _end = DateTime.Now + Duration;

            }
            _timer.Start();
        }

        /// <summary>
        /// Arrête la montre
        /// </summary>
        public void Stop()
        {            
            _timer.Stop();
            _end = null;
        }

        protected override void OnApplyTemplate()
        {
            if (GetTemplateChild(ScalePartName) is Path scalePath)
            {
                scalePath.Stroke = ScaleBrush;
                scalePath.StrokeThickness = StrokeThickness;
                RenderArc(360, scalePath);
            }
            base.OnApplyTemplate();

        }

        private static Point ComputeCartesianCoordinate(double angle, double radius)
        {
            double angleRad = (Math.PI / 180.0) * (angle - 90);

            double x = radius * Math.Cos(angleRad);
            double y = radius * Math.Sin(angleRad);

            return new Point(x, y);
        }

        private static double ConvertTimeToAngle(int time)
        {
            double dtime = Convert.ToDouble(time);
            return (dtime * 6);
        }


        private static void OnMinuteAngleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            // [TODO]
        }

        private static void OnSecondAngleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            CircularClock circle = (CircularClock)sender;

            // trainée des secondes
            if (circle.GetTemplateChild(TrailPartName) is Path trailPath)
            {
                trailPath.Stroke = circle.TrailBrush;
                trailPath.StrokeThickness = circle.StrokeThickness;
                circle.RenderArc(circle.SecondAngle, trailPath);
            }
        }


        private void RenderArc(double angle, Path pathRoot)
        {
            var pg = new PathGeometry();
            var pathFigure = new PathFigure();
            var arcSegment = new ArcSegment();
            pathFigure.Segments.Add(arcSegment);
            pg.Figures.Add(pathFigure);

            Point startPoint = new Point(Radius, 0);
            Point endPoint = ComputeCartesianCoordinate(angle, Radius);
            endPoint.X += Radius;
            endPoint.Y += Radius;

            pathRoot.Width = Radius * 2 + StrokeThickness;
            pathRoot.Height = Radius * 2 + StrokeThickness;
            pathRoot.Margin = new Thickness(StrokeThickness, StrokeThickness, 0, 0);

            bool largeArc = angle > 180.0;

            Size outerArcSize = new Size(Radius, Radius);

            pathFigure.StartPoint = startPoint;

            if (startPoint.X == Math.Round(endPoint.X) && startPoint.Y == Math.Round(endPoint.Y))
            {
                endPoint.X -= 0.01;
            }

            arcSegment.SweepDirection = SweepDirection.Clockwise;
            arcSegment.Point = endPoint;
            arcSegment.Size = outerArcSize;
            arcSegment.IsLargeArc = largeArc;

            pathRoot.Data = pg;
        }

        private void Timer_Tick(object sender, object e)
        {
            UpdateTime();
        }

        private void UpdateTime()
        {
            int seconds = 0;
            int minutes = 0;
            if(_end != null)
            {
                var ts =  (_end.Value - DateTime.Now);
                seconds = ts.Seconds;
                minutes = ts.Minutes;
            }
            else
            {
                seconds = DateTime.Now.Second;
                minutes = DateTime.Now.Minute;
            }
            SecondAngle = ConvertTimeToAngle(seconds);
            MinuteAngle = ConvertTimeToAngle(minutes);
        }
        #endregion
    }
}
