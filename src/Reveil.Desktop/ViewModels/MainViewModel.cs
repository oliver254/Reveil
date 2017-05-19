using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Reveil.Configuration;
using Reveil.Core;
using Reveil.Messages;
using System;
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

        private readonly ConfigurationStore _configuration;

        private TimeSpan? _duree;   
        #endregion

        #region Constructeurs
        public MainViewModel(ConfigurationStore configuration)
        {
            _configuration = configuration;


            //les commandes
            SprintCommand = new RelayCommand(() => ExecuteCommand(_configuration.Sprint));
            LongBreakCommand = new RelayCommand(() => ExecuteCommand(_configuration.LongBreak));
            ShortBreakCommand = new RelayCommand(() => ExecuteCommand(_configuration.ShortBreak));
        }
        #endregion

        #region Propri�t�s
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

        public RelayCommand LongBreakCommand
        {
            get;
            private set;
        }
 
        /// <summary>
        /// Retourne ou d�finit le chemin de la sonnerie
        /// </summary>
        public string RingPath
        {
            get { return _configuration.RingPath; }

        }


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

        #endregion

        #region M�thodes      
        private void ExecuteCommand(int duration)
        {
            Duration = TimeSpan.FromMinutes(duration);

        }
        #endregion

    }
}