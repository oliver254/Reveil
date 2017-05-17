using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reveil.Core
{
    public class ConfigManager
    {
        #region Champs
        public const string DefaultRingPath = @"Resources\\alarm.wav";
        public const int DefaultLongBreak = 15;
        public const int DefaultShortBreak = 5;

        protected static readonly object _syncRoot = new Object();
        private static volatile ConfigManager _instance = null;
        private readonly Configuration _config;
        #endregion

        #region Constructeurs
        public ConfigManager()
        {
            _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            System.Diagnostics.Trace.WriteLine("ConfigManager's constructor is called");
        }
        ~ConfigManager()
        {
            System.Diagnostics.Trace.WriteLine("ConfigManager's destructor is called.");
        }
        #endregion

        #region Propriétés
        /// <summary>
        /// Retourne l'instance du service de la configuration
        /// </summary>
        public static ConfigManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new ConfigManager();
                        }
                    }
                }
                return _instance;
            }
        }
        /// <summary>
        /// Retourne la configuration
        /// </summary>
        public Configuration Config
        {
            get { return _config; }
        }

        #endregion

        #region Méthodes
        /// <summary>
        /// Retourne le deuxième écran
        /// </summary>
        /// <returns></returns>
        public bool GetDualScreen()
        {
            bool bresult;

            bresult = false;
            var propriete = _config.AppSettings.Settings[Constants.DualScreen];
            if (propriete != null &&
                bool.TryParse(propriete.Value, out bresult) == false)
            {
                bresult = false;
            }
            return bresult;
        }

        /// <summary>
        /// Retourne la durée du long break
        /// </summary>
        /// <returns></returns>
        public int GetLongBreak()
        {
            int longBreak;

            longBreak = DefaultLongBreak;
            var propriete = _config.AppSettings.Settings[Constants.LongBreak];
            if (propriete != null &&
                int.TryParse(propriete.Value, out longBreak) == false)
            {
                longBreak = DefaultLongBreak;
            }
            return longBreak;
        }
        /// <summary>
        /// Retourne le chemin musique
        /// </summary>
        /// <returns></returns>
        public string GetRingPath()
        {
            string svaleur;

            svaleur = DefaultRingPath;
            var propriete = _config.AppSettings.Settings[Constants.RingPath];
            if (propriete != null &&
                string.IsNullOrWhiteSpace(propriete.Value) == false &&
                File.Exists(propriete.Value) == true)
            {
                svaleur = propriete.Value;
            }
            return svaleur;
        }
        /// <summary>
        /// Retourne le mode Pomodoro
        /// </summary>
        /// <returns></returns>
        public bool GetPomodoroMode()
        {
            bool bresult;

            bresult = false;
            var propriete = _config.AppSettings.Settings[Constants.ModePomodoro];
            if (propriete != null)
            {
                bool.TryParse(propriete.Value, out bresult);
            }
            return bresult;

        }

        /// <summary>
        /// Retourne la durée d'un short break
        /// </summary>
        /// <returns></returns>
        public int GetShortBreak()
        {
            int iresult;

            iresult = DefaultShortBreak;
            var propriete = _config.AppSettings.Settings[Constants.ShortBreak];
            if (propriete != null &&
                int.TryParse(propriete.Value, out iresult) == false)
            {
                iresult = DefaultShortBreak;
            }
            return iresult;
        }


        /// <summary>
        /// Retourne la durée d'un sprint défini par la configuration
        /// </summary>
        /// <returns></returns>
        public int GetSprint()
        {
            int iresult;

            iresult = 25;
            var propriete = _config.AppSettings.Settings[Constants.Sprint];
            if (propriete != null)
            {
                int.TryParse(propriete.Value, out iresult);
            }
            return iresult;
        }


        /// <summary>
        /// Définit le mode du Dual Screen
        /// </summary>
        /// <param name="valeur"></param>
        public void SetDualScreen(bool valeur)
        {
            string svaleur;


            var propriete = _config.AppSettings.Settings[Constants.DualScreen];
            svaleur = valeur.ToString();
            if (propriete == null)
            {
                _config.AppSettings.Settings.Add(new KeyValueConfigurationElement(Constants.DualScreen, svaleur));
            }
            else
            {
                _config.AppSettings.Settings[Constants.DualScreen].Value = valeur.ToString();
            }
            this.Save();
        }

        /// <summary>
        /// Définit le chemin d'accès de la sonnerie
        /// </summary>
        /// <param name="valeur"></param>
        public void SetRingPath(string valeur)
        {
            var propriete = _config.AppSettings.Settings[Constants.RingPath];
            if (propriete == null)
            {
                _config.AppSettings.Settings.Add(new KeyValueConfigurationElement(Constants.RingPath, valeur));
            }
            else
            {
                _config.AppSettings.Settings[Constants.RingPath].Value = valeur;
            }
            this.Save();
        }
        /// <summary>
        /// Définit la durée du Long Break
        /// </summary>
        /// <param name="duree"></param>
        public void SetLongBreak(int duree)
        {
            string svaleur;

            svaleur = duree.ToString();
            var propriete = _config.AppSettings.Settings[Constants.LongBreak];
            if (propriete == null)
            {
                _config.AppSettings.Settings.Add(
                    new KeyValueConfigurationElement(Constants.LongBreak, svaleur));
            }
            else
            {
                _config.AppSettings.Settings[Constants.LongBreak].Value = svaleur;
            }
            this.Save();
        }
        /// <summary>
        /// Définit si l'application est en mode Pomodoro et l'enregistre dans la configuration
        /// </summary>
        /// <param name="valeur"></param>
        public void SetPomodoroMode(bool valeur)
        {
            string svaleur;

            svaleur = valeur.ToString();
            var propriete = _config.AppSettings.Settings[Constants.ModePomodoro];
            if (propriete == null)
            {
                _config.AppSettings.Settings.Add(
                    new KeyValueConfigurationElement(Constants.ModePomodoro, svaleur));
            }
            else
            {
                _config.AppSettings.Settings[Constants.ModePomodoro].Value = svaleur;
            }
            this.Save();
        }

        /// <summary>
        /// Définit la durée du Short Break
        /// </summary>
        /// <param name="duree"></param>
        public void SetShortBreak(int duree)
        {
            string svaleur;

            svaleur = duree.ToString();
            var propriete = _config.AppSettings.Settings[Constants.ShortBreak];
            if (propriete == null)
            {
                _config.AppSettings.Settings.Add(
                    new KeyValueConfigurationElement(Constants.ShortBreak, svaleur));
            }
            else
            {
                _config.AppSettings.Settings[Constants.ShortBreak].Value = svaleur.ToString();
            }
            this.Save();
        }

        /// <summary>
        /// Définit la durée d'un sprint et la stocke dans la configuration
        /// </summary>
        /// <param name="duree"></param>
        public void SetSprint(int duree)
        {
            _config.AppSettings.Settings[Constants.Sprint].Value = duree.ToString();
            this.Save();
        }
        /// <summary>
        /// Enregistre la configuration
        /// </summary>
        private void Save()
        {
            _config.Save(ConfigurationSaveMode.Modified);
        }
        #endregion
    }
}
