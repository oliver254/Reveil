using GalaSoft.MvvmLight;
using Reveil.Configuration;
using Reveil.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reveil.ViewModels
{
    public class ConfigurationViewModel : ViewModelBase
    {
        #region Champs
        private readonly MainViewModel _parentVM;
        private readonly ConfigurationStore _configuration;
        #endregion

        #region Constructeurs
        public ConfigurationViewModel(MainViewModel parentVM, ConfigurationStore configuration)
        {
            _parentVM = parentVM;
            _configuration = configuration;

            MessengerInstance.Register<RingPathMessage>(this, Configuration_RingPathReceived);
        }
        #endregion

        #region Propriétés
        /// <summary>
        /// Obtient ou définit si le mode double écran est actif.
        /// </summary>
        public bool DualMode
        {
            get
            {
                return _configuration.DualMode;
            }
            set
            {
                _configuration.DualMode = value;
                RaisePropertyChanged(nameof(DualMode));
            }
        }

        /// <summary>
        /// Obtient ou définit la durée d'une longue durée.
        /// </summary>
        public int LongBreak
        {
            get
            {
                return _configuration.LongBreak;
            }
            set
            {
                _configuration.LongBreak = value;
                RaisePropertyChanged(nameof(LongBreak));
            }
        }

        /// <summary>
        /// Obtient ou définit le chemin du son de l'alarme.
        /// </summary>
        public string RingPath
        {
            get
            {
                return _configuration.RingPath;
            }
            set
            {
                _configuration.RingPath = value;
                RaisePropertyChanged(nameof(RingPath));
            }
        }

        /// <summary>
        /// Obitent ou définit la durée d'une courte pause.
        /// </summary>
        public int ShortBreak
        {
            get
            {
                return _configuration.ShortBreak;
            }
            set
            {
                _configuration.ShortBreak = value;
                RaisePropertyChanged(nameof(ShortBreak));
            }

        }

        /// <summary>
        /// Obtient ou définit la durée d'un sprint.
        /// </summary>
        public int Sprint
        {
            get
            {
                return _configuration.Sprint;
            }
            set
            {
                _configuration.Sprint = value;
                RaisePropertyChanged(nameof(Sprint));
            }
        }
        #endregion

        #region Méthodes
        private void Configuration_RingPathReceived(RingPathMessage message)
        {
            RingPath = message.Path;
        }
        #endregion
    }
}
