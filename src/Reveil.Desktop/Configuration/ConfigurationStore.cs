using System.Configuration;

namespace Reveil.Configuration
{
    public class ConfigurationStore
    {
        #region Champs
        public const int DefaultLongBreak = 15;
        public const string DefaultRingPath = @"pack://siteoforigin:,,,/Resources/alarm.wav";
        public const int DefaultShortBreak = 5;
        public const int DefaultSprint = 25;
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
        /// Obtient la durée d'une longue pause.
        /// </summary>
        public int LongBreak
        {
            get
            {
                int valeur;
                if (!TryGet(nameof(LongBreak), out valeur))
                {
                    return DefaultLongBreak;
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
        /// Détermine si le mode minimisé est actif.
        /// </summary>
        public bool Minimize
        {
            get
            {
                bool valeur = false;
                if (!TryGet(nameof(Minimize), out bool value))
                {
                    return false;
                }
                return valeur;
            }
            set
            {
                bool valeur = value;
                Save<bool>(nameof(Minimize), valeur);
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
                if (!TryGet(nameof(RingPath), out valeur))
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
                if (!TryGet(nameof(ShortBreak), out valeur))
                {
                    return DefaultShortBreak;
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
                if (!TryGet(nameof(Sprint), out valeur))
                {
                    return DefaultSprint;
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

        private bool TryGet(string name, out bool value)
        {
            value = false;

            KeyValueConfigurationElement element = _config.AppSettings.Settings[name];
            if (element == null)
            {
                return false;
            }
            return bool.TryParse(element.Value, out value);
        }

        private bool TryGet(string name, out int value)
        {
            value = 0;

            KeyValueConfigurationElement element = _config.AppSettings.Settings[name];
            if (element == null)
            {
                return false;
            }
            return int.TryParse(element.Value, out value);
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