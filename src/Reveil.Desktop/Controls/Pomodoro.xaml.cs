using System.Windows;
using System.Windows.Controls;

namespace Reveil.Controls
{
    /// <summary>
    /// Logique d'interaction pour Pomodoro.xaml
    /// </summary>
    public partial class Pomodoro : UserControl
    {
        #region Champs
        public static readonly DependencyProperty AlarmProperty =
            DependencyProperty.Register(
                "Alarm", 
                typeof(bool), 
                typeof(Pomodoro), 
                new PropertyMetadata(false, new PropertyChangedCallback(OnPropertyChanged)));
        #endregion

        #region Constructeurs
        public Pomodoro()
        {
            InitializeComponent();
        }
        #endregion

        #region Propriétés
        /// <summary>
        /// Obtient ou définit si l'alarme est activé
        /// </summary>
        public bool Alarm
        {
            get { return (bool)GetValue(AlarmProperty); }
            set { SetValue(AlarmProperty, value); }
        }
        #endregion

        #region Méthodes
        private static void OnPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            Pomodoro pvPomodoro;
            bool bvaleur;

            pvPomodoro = sender as Pomodoro;
            bvaleur = (bool)args.NewValue;
            if (bvaleur == true)
            {
                pvPomodoro.meAlarm.Play();
            }
            else
            {
                pvPomodoro.meAlarm.Close();
            }

        }
        #endregion
    }
}
