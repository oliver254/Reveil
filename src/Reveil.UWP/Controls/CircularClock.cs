using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
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
        public static readonly DependencyProperty AngleProperty = 
            DependencyProperty.Register(nameof(Angle), typeof(double), typeof(CircularClock), new PropertyMetadata(120d, new PropertyChangedCallback(OnValueChanged)));

        public static readonly DependencyProperty RadiusProperty = 
            DependencyProperty.Register(nameof(Radius), typeof(int), typeof(CircularClock), new PropertyMetadata(25, new PropertyChangedCallback(OnValueChanged)));

        public static readonly DependencyProperty ScaleBrushProperty =
            DependencyProperty.Register(nameof(ScaleBrush), typeof(Brush), typeof(CircularClock), new PropertyMetadata(new SolidColorBrush(Colors.DarkGray)));


        public static readonly DependencyProperty StrokeThicknessProperty = 
            DependencyProperty.Register("StrokeThickness", typeof(int), typeof(CircularClock), new PropertyMetadata(5));

        public static readonly DependencyProperty TimeSpanProperty =
            DependencyProperty.Register(nameof(TimeSpan), typeof(TimeSpan), typeof(CircularClock), new PropertyMetadata(TimeSpan.Zero, new PropertyChangedCallback(OnValueChanged)));

        public static readonly DependencyProperty TrailBrushProperty =
            DependencyProperty.Register(nameof(TrailBrush), typeof(Brush), typeof(CircularClock), new PropertyMetadata(new SolidColorBrush(Colors.DarkBlue)));

        private const string ContainerPartName = "PART_Container";
        private const string ScalePartName = "PART_Scale";
        private const string TrailPartName = "PART_Trail";
        #endregion

        #region Constructeurs
        public CircularClock()
        {
            this.DefaultStyleKey = typeof(CircularClock);
        }
        #endregion

        #region Propriétés
        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

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

        public int StrokeThickness
        {
            get { return (int)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public TimeSpan TimeSpan
        {
            get { return (TimeSpan)GetValue(TimeSpanProperty); }
            set { SetValue(TimeSpanProperty, value); }
        }

        public Brush TrailBrush
        {
            get { return (Brush)GetValue(TrailBrushProperty); }
            set { SetValue(TrailBrushProperty, value); }
        }
        #endregion

        #region Méthodes
        protected override void OnApplyTemplate()
        {
            OnValueChanged(this);
            base.OnApplyTemplate();

        }

        private static Point ComputeCartesianCoordinate(double angle, double radius)
        {
            double angleRad = (Math.PI / 180.0) * (angle - 90);

            double x = radius * Math.Cos(angleRad);
            double y = radius * Math.Sin(angleRad);

            return new Point(x, y);
        }

        private static void OnValueChanged(DependencyObject sender)
        {
            CircularClock circle = (CircularClock)sender;

            Path scalePath = circle.GetTemplateChild(ScalePartName) as Path;
            if(scalePath != null)
            {
                scalePath.Stroke = circle.ScaleBrush;
                scalePath.StrokeThickness = circle.StrokeThickness;
                circle.RenderArc(360, scalePath);                
            }
            Path trailPath = circle.GetTemplateChild(TrailPartName) as Path;
            if(trailPath != null)
            {
                trailPath.Stroke = circle.TrailBrush;
                trailPath.StrokeThickness = circle.StrokeThickness;                
                circle.RenderArc(circle.Angle, trailPath);
            }
        }

        private static void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            OnValueChanged(sender);
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
        #endregion
    }
}
