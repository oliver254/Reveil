using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Reveil.Uwp.Controls
{
    [TemplatePart(Name = ContainerPartName, Type = typeof(Grid))]
    [TemplatePart(Name = ScalePartName, Type = typeof(Path))]
    [TemplatePart(Name = MinuteTrailPartName, Type = typeof(Path))]
    [TemplatePart(Name = SecondTrailPartName, Type = typeof(Path))]
    public sealed class CircularClock : Control
    {
        #region Champs
        private const string ContainerPartName = "PART_Container";
        private const string ScalePartName = "PART_Scale";
        private const string MinuteTrailPartName = "PART_MinuteTrail";
        private const string SecondTrailPartName = "PART_SecondTrail";

        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register(
                nameof(Duration),
                typeof(TimeSpan?),
                typeof(CircularClock),
                new PropertyMetadata(null));

        public static readonly DependencyProperty MinuteAngleProperty =
            DependencyProperty.Register(
                nameof(MinuteAngle),
                typeof(double),
                typeof(CircularClock),
                new PropertyMetadata(0d, Clock_MinuteAngleChanged));

        public static readonly DependencyProperty MinuteTrailBrushProperty =
            DependencyProperty.Register(
                nameof(MinuteTrailBrush),
                typeof(Brush), typeof(CircularClock),
                new PropertyMetadata(new SolidColorBrush(Colors.DarkBlue)));

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register(
                nameof(Radius),
                typeof(int),
                typeof(CircularClock),
                new PropertyMetadata(50));

        public static readonly DependencyProperty ScaleBrushProperty =
            DependencyProperty.Register(
                nameof(ScaleBrush),
                typeof(Brush),
                typeof(CircularClock),
                new PropertyMetadata(new SolidColorBrush(Colors.DarkGray)));

        public static readonly DependencyProperty SecondAngleProperty =
            DependencyProperty.Register(
                nameof(SecondAngle),
                typeof(double),
                typeof(CircularClock),
                new PropertyMetadata(0d, Clock_SecondAngleChanged));

        public static readonly DependencyProperty SecondTrailBrushProperty =
            DependencyProperty.Register(
                nameof(SecondTrailBrush),
                typeof(Brush),
                typeof(CircularClock),
                new PropertyMetadata(new SolidColorBrush(Colors.LightBlue)));

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(
                nameof(StrokeThickness),
                typeof(int),
                typeof(CircularClock),
                new PropertyMetadata(4));

        private readonly DispatcherTimer _timer;

        private DateTime? _end;
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
        /// Obtient ou définit le pinceau de couleur pour la trainée des minutes.
        /// </summary>
        public Brush MinuteTrailBrush
        {
            get { return (Brush)GetValue(MinuteTrailBrushProperty); }
            set { SetValue(MinuteTrailBrushProperty, value); }
        }

        /// <summary>
        /// Obtient ou définit le rayon.
        /// </summary>
        public int Radius
        {
            get { return (int)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        /// <summary>
        /// Obtient ou définit le pinceau de couleur pour le fond.
        /// </summary>
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

        /// <summary>
        /// Obtient ou définit le pinceau de la couleur pour la trainée des secondes.
        /// </summary>
        public Brush SecondTrailBrush
        {
            get { return (Brush)GetValue(SecondTrailBrushProperty); }
            set { SetValue(SecondTrailBrushProperty, value); }
        }

        /// <summary>
        /// Obtient ou définit la largeur de l'horloge
        /// </summary>
        public int StrokeThickness
        {
            get { return (int)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
        #endregion

        #region Méthodes
        /// <summary>
        /// Lance la montre
        /// </summary>
        public void Start()
        {
            if (Duration != null)
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

        private static void Clock_DurationChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            CircularClock clock = (CircularClock)sender;
            clock.UpdateTime();
        }

        private static void Clock_MinuteAngleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            CircularClock clock = (CircularClock)sender;

            // trainée des minutes
            if (clock.GetTemplateChild(MinuteTrailPartName) is Path trailPath)
            {
                trailPath.Stroke = clock.MinuteTrailBrush;
                trailPath.StrokeThickness = clock.StrokeThickness / 2;

                int size = (int)(trailPath.StrokeThickness / 2);

                int radius = (int)(clock.Radius - size);

                clock.RenderArcForTrail(clock.MinuteAngle, radius, trailPath);
            }
        }

        private static void Clock_SecondAngleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            CircularClock clock = (CircularClock)sender;

            // trainée des secondes
            if (clock.GetTemplateChild(SecondTrailPartName) is Path trailPath)
            {
                trailPath.Stroke = clock.SecondTrailBrush;
                trailPath.StrokeThickness = clock.StrokeThickness / 2;

                int size = (int)(trailPath.StrokeThickness / 2);

                int radius = clock.Radius + size;

                clock.RenderArcForTrail(clock.SecondAngle, radius, trailPath);
            }
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

            double width = Radius * 2 + StrokeThickness;

            pathRoot.Width = width;
            pathRoot.Height = width;
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

        private void RenderArcForTrail(double angle, int radius, Path pathRoot)
        {
            var pg = new PathGeometry();
            var pathFigure = new PathFigure();
            var arcSegment = new ArcSegment();
            pathFigure.Segments.Add(arcSegment);
            pg.Figures.Add(pathFigure);

            Point startPoint = new Point(radius, 0);
            Point endPoint = ComputeCartesianCoordinate(angle, radius);
            endPoint.X += radius;
            endPoint.Y += radius;

            double width = radius * 2 + StrokeThickness;

            pathRoot.Width = width;
            pathRoot.Height = width;
            pathRoot.Margin = new Thickness(StrokeThickness, StrokeThickness, 0, 0);

            bool largeArc = angle > 180.0;

            Size outerArcSize = new Size(radius, radius);

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
            int seconds;
            int minutes;

            if (_end != null)
            {
                var ts = (_end.Value - DateTime.Now);
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
