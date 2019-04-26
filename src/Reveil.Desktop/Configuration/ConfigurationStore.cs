using System.Configuration;

using NLog;

using Reveil.Properties;

namespace Reveil.Configuration
{
    public class ConfigurationStore
    {
        #region Champs
        public const int DefaultLongBreak = 15;
        public const string DefaultRingPath = @"pack://siteoforigin:,,,/Resources/alarm.wav";
        public const int DefaultShortBreak = 5;
        public const int DefaultSprint = 25;
        #endregion

        #region Constructeurs
        public ConfigurationStore()
        {
            
        }
        #endregion

        #region Propriétés
        /// <summary>
        /// Obtient ou définit si la fenêtre possède des bordures.
        /// </summary>
        public bool Border
        {
            get
            {
                return Settings.Default.Border;
            }
            set
            {
                Settings.Default.Border = value;
                Save();
            }
        }
        /// <summary>
        /// Obtient la durée d'une longue pause.
        /// </summary>
        public int LongBreak
        {
            get
            {
                return Settings.Default.LongBreak;
            }
            set
            {
                Settings.Default.LongBreak = value;
                Save();
            }
        }

        /// <summary>
        /// Obtient ou définit le chemin du son de l'alarme.
        /// </summary>
        public string RingPath
        {
            get
            {
                return Settings.Default.RingPath;
            }
            set
            {
                Settings.Default.RingPath = value;
                Save();
            }
        }
        /// <summary>
        /// Obtient ou définit la durée d'une courte pause.
        /// </summary>
        public int ShortBreak
        {
            get
            {
                return Settings.Default.ShortBreak;
            }
            set
            {
                Settings.Default.ShortBreak = value;
                Save();
            }
        }
        /// <summary>
        /// Obtient ou définit la durée d'un sprint.
        /// </summary>
        public int Sprint
        {
            get
            {
                return Settings.Default.Sprint;
            }
            set
            {
                Settings.Default.Sprint = value;
                Save();
            }
        }
        /// <summary>
        /// Détermine si le mode transparent est actif.
        /// </summary>
        public bool Transparent
        {
            get
            {
                return Settings.Default.Transparent;
            }
            set
            {
                Settings.Default.Transparent = value;
                Save();
            }
        }
        #endregion

        #region Méthodes
        private void Save()
        {
            Settings.Default.Save();
        }
        #endregion
    }
}
