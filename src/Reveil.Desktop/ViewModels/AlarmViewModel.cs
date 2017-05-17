using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reveil.ViewModels
{
    public class AlarmViewModel : ViewModelBase
    {
        #region Champs
        public const string HourPropertyName = "Hour";
        public const string MinutePropertyName = "Minute";
        public const string NamePropertyName = "Name";

        private string _nom;
        private int _heure, _minute;
        #endregion

        #region Constructeurs
        public AlarmViewModel()
        {
            _heure = DateTime.Now.Hour;
            _minute = DateTime.Now.Minute;
            _nom = "Bonjour";
        }

        #endregion

        #region Propriétés
        /// <summary>
        /// Retourne ou définit l'heure
        /// </summary>
        public int Hour
        {
            get { return _heure; }
            set
            {
                _heure = value;
                RaisePropertyChanged(HourPropertyName);
            }
        }
        /// <summary>
        /// Retourne ou définit la minute
        /// </summary>
        public int Minute
        {
            get { return _minute; }
            set
            {
                _minute = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Retourne ou définit le nom
        /// </summary>
        public string Name
        {
            get { return _nom; }
            set
            {
                _nom = value;
                RaisePropertyChanged(NamePropertyName);
            }
        }

        #endregion

    }
}
