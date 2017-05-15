using Reveil.Core.Infrastructure;
using Reveil.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Reveil.UWP.Services
{
    public class SettingsManager : ISettingsManager
    {

        #region Champs
        private readonly ApplicationDataContainer _container = null;
        #endregion

        #region Constructeurs
        public SettingsManager()
        {
            _container = ApplicationData.Current.LocalSettings.CreateContainer(
                Constants.ClockSettings, 
                ApplicationDataCreateDisposition.Always);
        }
        #endregion

        #region Méthodes      

        public Settings Get()
        {
            Settings settings = new Settings()
            {
                Sprint = Constants.SprintDefault,
                LongBreak = Constants.LongBreakDefault,
                ShortBreak = Constants.ShortBreakDefault
            };

            if(_container.Values[Constants.Sprint] is int sprintValue)
            {
                settings.Sprint = sprintValue;
            }
            if(_container.Values[Constants.LongBreak] is int longBreakValue)
            {
                settings.LongBreak = longBreakValue;
            }
            if(_container.Values[Constants.ShortBreak] is int shortBreakValue)
            {
                settings.ShortBreak = shortBreakValue;
            }

            return settings;
        }

        public void Save(Settings settings)
        {
            _container.Values[Constants.Sprint] = settings.Sprint;
            _container.Values[Constants.LongBreak] = settings.LongBreak;
            _container.Values[Constants.ShortBreak] = settings.ShortBreak;
        }
        #endregion
    }
}
