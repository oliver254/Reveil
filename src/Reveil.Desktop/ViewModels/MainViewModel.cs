using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Reveil.Core;
using Reveil.Messages;
using System.Windows.Threading;

namespace Reveil.ViewModels
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {

        #region Champs
        public const string RingPathPropertyName = "RingPath";

        private DispatcherTimer _timer;
        private PomodoroViewModel _pomodoroVM;
        #endregion

        #region Constructeurs
        public MainViewModel()
        {
            this.PomodoroVM = new PomodoroViewModel();
            //timer
            _timer = new DispatcherTimer();
            _timer.Tick += new System.EventHandler(_pomodoroVM.OnTick);
            _timer.Interval = new System.TimeSpan(0, 0, 0, 0, 1);
            _timer.Start();
            //les message
            this.MessengerInstance.Register<RingPathMessage>(this, OnRingPath);
            //les commandes
            ResetCommand = new RelayCommand(ExecuteResetCommand);
        }
        #endregion

        #region Propriétés
        /// <summary>
        /// Détermine si la fenêtre est sur le double écran
        /// </summary>
        public bool IsDualMode
        {
            get;
            private set;
        }
        /// <summary>
        /// Retourne le modele de vue PomodoroVM
        /// </summary>
        public PomodoroViewModel PomodoroVM
        {
            get { return _pomodoroVM; }
            private set
            {
                _pomodoroVM = value;
            }
        }
        /// <summary>
        /// Retourne la commande Reset
        /// </summary>
        public RelayCommand ResetCommand
        {
            get;
            private set;
        }
        /// <summary>
        /// Retourne ou définit le chemin de la sonnerie
        /// </summary>
        public string RingPath
        {
            get { return ConfigManager.Instance.GetRingPath(); }
            set
            {
                ConfigManager.Instance.SetRingPath(value);
                RaisePropertyChanged(RingPathPropertyName);
            }
        }
        #endregion

        #region Méthodes
        public override void Cleanup()
        {
            base.Cleanup();
            if (_timer != null)
            {
                _timer.Stop();
                _timer = null;
            }
        }

        private void OnRingPath(RingPathMessage message)
        {
            RingPath = message.Path; 
        }

        /// <summary>
        /// Exécutes la commande Reset
        /// </summary>
        private void ExecuteResetCommand()
        {
            PomodoroVM.Reset();
        }
        #endregion

    }
}