using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Reveil.Configuration;
using System;

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
        private TimeSpan? _duree;
        private MainWindow _view;
        #endregion

        #region Constructeurs
        public MainViewModel()
        {

            //les commandes
            LongBreakCommand = new RelayCommand(() => ExecuteCommand(Configuration.LongBreak));
            ShortBreakCommand = new RelayCommand(() => ExecuteCommand(Configuration.ShortBreak));
            SprintCommand = new RelayCommand(() => ExecuteCommand(Configuration.Sprint));
            StopCommand = new RelayCommand(ExecuteStopCommand);
        }
        #endregion

        #region Propriétés
        public ConfigurationStore Configuration => ServiceLocator.Current.GetInstance<ConfigurationStore>();

        /// <summary>
        /// Obtient ou définit la durée.
        /// </summary>
        public TimeSpan? Duration
        {
            get
            {
                return _duree;
            }
            set
            {
                _duree = value;
                RaisePropertyChanged(nameof(Duration));
            }
        }

        /// <summary>
        /// Obtient la commande LongBreak.
        /// </summary>
        public RelayCommand LongBreakCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Retourne ou définit le chemin de la sonnerie
        /// </summary>
        public string RingPath
        {
            get
            {
                return Configuration.RingPath;
            }
        }
        /// <summary>
        /// Obtient la commande ShortBreak.
        /// </summary>
        public RelayCommand ShortBreakCommand
        {
            get;
            private set;
        }
        /// <summary>
        /// Obtient la commande Sprint
        /// </summary>
        public RelayCommand SprintCommand
        {
            get;
            private set;
        }
        /// <summary>
        /// Obtient la commande Stop
        /// </summary>
        public RelayCommand StopCommand
        {
            get;
            private set;
        }

        public MainWindow View => _view;
        #endregion

        #region Méthodes
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        public void Initialize(MainWindow view)
        {
            _view = view;

            if(Configuration.Transparent)
            {
                _view.WindowStyle = System.Windows.WindowStyle.None;
                _view.AllowsTransparency = true;
                _view.ActivateTransparency();
            }
        }

        private void ExecuteCommand(int duration)
        {
            Duration = TimeSpan.FromMinutes(duration);
        }


        /// <summary>
        /// Execute la commande Stop
        /// </summary>
        private void ExecuteStopCommand()
        {
            Duration = null;
        }


        #endregion
    }
}