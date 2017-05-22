using System.Configuration;

namespace Reveil.Configuration
{
    public class ConfigurationStore
    {
        #region Champs
        public const string DefaultRingPath = @"pack://siteoforigin:,,,/Resources/alarm.wav";
        public const int DefaultSprint = 25;
        public const int DefaultLongBreak = 15;
        public const int DefaultShortBreak = 5;

        private readonly System.Configuration.Configuration _config;
        #endregion

        #region Constructeurs
        public ConfigurationStore()
        {
            _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            System.Diagnostics.Trace.WriteLine("ConfigManager's constructor is called");
            
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
                bool valeur = false;
                if (TryGet(nameof(DualMode), out string result))
                {

                    valeur = bool.Parse(result);
                }

                return valeur;
            }
            set
            {
                bool valeur = value;
                Save<bool>(nameof(DualMode), valeur);
            }
        }

        /// <summary>
        /// Obtient la durée d'une longue pause.
        /// </summary>
        public int LongBreak
        {
            get
            {
                int valeur;
                valeur = DefaultLongBreak;
                if (TryGet(nameof(LongBreak), out string result))
                {
                    valeur = int.Parse(result);
                }
                return valeur;
            }
            set
            {
                int valeur = value;
                Save<int>(nameof(LongBreak), valeur);
            }
                
        }

        /// <summary>
        /// Obtient ou définit le chemin du son de l'alarme.
        /// </summary>
        public string RingPath
        {
            get
            {
                string valeur;                
                if(!TryGet(nameof(RingPath), out valeur))
                {
                    return DefaultRingPath;
                }
                return valeur;
            }
            set
            {
                string valeur = value;
                Save<string>(nameof(RingPath), valeur);
            }
        }

        /// <summary>
        /// Obtient ou définit la durée d'une courte pause.
        /// </summary>
        public int ShortBreak
        {
            get
            {
                int valeur;
                valeur = DefaultShortBreak;
                if(TryGet(nameof(ShortBreak), out string result))
                {
                    valeur = int.Parse(result);
                }
                return valeur;
            }
            set
            {
                int valeur = value;
                Save<int>(nameof(ShortBreak), valeur);
            }
        }

        /// <summary>
        /// Obtient ou définit la durée d'un sprint.
        /// </summary>
        public int Sprint
        {
            get
            {
                int valeur;
                valeur = DefaultSprint;
                if(TryGet(nameof(Sprint), out string result))
                {
                    valeur = int.Parse(result);
                }
                return valeur;
            }
            set
            {
                int valeur = value;
                Save<int>(nameof(Sprint), valeur);
            }
        }
        #endregion

        #region Méthodes
        private void Save<T>(string name, T value)
        {
            KeyValueConfigurationElement element = _config.AppSettings.Settings[name];

            if (element == null)
            {
                element = new KeyValueConfigurationElement(name, value.ToString());
                _config.AppSettings.Settings.Add(element);
            }
            else
            {
                element.Value = value.ToString();
            }
            _config.Save(ConfigurationSaveMode.Modified);            
        }

        private bool TryGet(string name, out string value)
        {
            value = string.Empty;

            KeyValueConfigurationElement element = _config.AppSettings.Settings[name];
            if (element == null)
            {
                return false;
            }
            value = element.Value;
            return true;

        }
        #endregion
    }
}
