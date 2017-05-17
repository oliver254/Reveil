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

namespace Reveil.Controls
{
    /// <summary>
    /// Logique d'interaction pour ReveilClock.xaml
    /// </summary>
    public partial class ReveilClock : UserControl
    {
        #region Champs
        // Using a DependencyProperty as the backing store for Angle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register("Angle", typeof(double), typeof(ReveilClock), new PropertyMetadata(120d, new PropertyChangedCallback(OnPropertyChanged)));
        // Using a DependencyProperty as the backing store for SegmentColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinuteColorProperty =
            DependencyProperty.Register("MinuteColor", typeof(Brush), typeof(ReveilClock), new PropertyMetadata(new SolidColorBrush(Colors.Orange)));
        // Using a DependencyProperty as the backing store for Radius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(int), typeof(ReveilClock), new PropertyMetadata(100, new PropertyChangedCallback(OnPropertyChanged)));
        // Using a DependencyProperty as the backing store for SegmentColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SecondColorProperty =
            DependencyProperty.Register("SecondColor", typeof(Brush), typeof(ReveilClock), new PropertyMetadata(new SolidColorBrush(Colors.OrangeRed)));

        // Using a DependencyProperty as the backing store for StrokeThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(int), typeof(ReveilClock), new PropertyMetadata(30, new PropertyChangedCallback(OnRenderBackground)));
        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(DateTime), typeof(ReveilClock), new PropertyMetadata(new PropertyChangedCallback(OnPropertyChanged)));
        #endregion

        #region Constructeurs
        public ReveilClock()
        {
            InitializeComponent();
            RenderArc();

        }
        #endregion

        #region Propriétés
        /// <summary>
        /// Retourne ou définit l'angle
        /// </summary>
        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
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
        /// <summary>
        /// Retourne ou définit le temps
        /// </summary>
        public DateTime Time
        {
            get { return (DateTime)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }
        #endregion

        #region Méthodes
        public void RenderArc()
        {
            double angle;
            int dr, itemp;
            Point startPoint, endPoint;
            bool largeArc;

            //seconds
            angle = ((double)this.Time.Second) + ((double)this.Time.Millisecond / 1000);
            angle = angle * 6;
            startPoint = new Point(Radius, 0);
            endPoint = ComputeCartesianCoordinate(angle, Radius);
            endPoint.X += Radius;
            endPoint.Y += Radius;

            itemp = Radius * 2 + StrokeThickness;
            pathSeconds.Width = itemp;
            pathSeconds.Height = itemp;
            pathSeconds.Margin = new Thickness(StrokeThickness, StrokeThickness, 0, 0);

            largeArc = angle > 180.0;

            Size outerArcSize = new Size(Radius, Radius);
            pfSeconds.StartPoint = startPoint;
            if (startPoint.X == Math.Round(endPoint.X) && startPoint.Y == Math.Round(endPoint.Y))
                endPoint.X -= 0.01;

            arcSeconds.Point = endPoint;
            arcSeconds.Size = outerArcSize;
            arcSeconds.IsLargeArc = largeArc;

            //minutes
            pathMinutes.Width = itemp;
            pathMinutes.Height = itemp;
            pathMinutes.Margin = new Thickness(StrokeThickness, StrokeThickness, 0, 0);

            dr = this.Radius - this.StrokeThickness;
            angle = (this.Time.Minute) * 6;
            startPoint = new Point(Radius, this.StrokeThickness);
            endPoint = ComputeCartesianCoordinate(angle, dr);
            endPoint.X += Radius;
            endPoint.Y += Radius;

            largeArc = angle > 180.0;

            outerArcSize = new Size(dr, dr);
            pfMinutes.StartPoint = startPoint;

            if (startPoint.X == Math.Round(endPoint.X) && startPoint.Y == Math.Round(endPoint.Y))
                endPoint.X -= 0.01;

            arcMinutes.Point = endPoint;
            arcMinutes.Size = outerArcSize;
            arcMinutes.IsLargeArc = largeArc;

        }
        private static void OnPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ReveilClock circle = sender as ReveilClock;
            circle.RenderArc();
        }
        private static void OnRenderBackground(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            double dtemp;
            int ivaleur;
            ReveilClock reveil;

            reveil = sender as ReveilClock;
            ivaleur = (int)args.NewValue;
            dtemp = (2 * reveil.Radius) + ivaleur;
            reveil.centerEllipse.Width = dtemp;
            reveil.centerEllipse.Height = dtemp;

            dtemp = (2 * reveil.Radius) - (3 * ivaleur);
            reveil.centerEllipse1.Width = dtemp;
            reveil.centerEllipse1.Height = dtemp;
        }
        private Point ComputeCartesianCoordinate(double angle, double radius)
        {
            // convert to radians
            double angleRad = (Math.PI / 180.0) * (angle - 90);

            double x = radius * Math.Cos(angleRad);
            double y = radius * Math.Sin(angleRad);

            return new Point(x, y);
        }
        #endregion

    }
}
