using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reveil.Core
{
    public static class Constants
    {
        #region Champs
        public const string ConfigFile = "Reveil.exe.config";
        public const string DualScreen = "dual";
        public const string LongBreak = "longBreak";
        public const string ModePomodoro = "pomodoro";
        public const string RingPath = "music";
        public const string ShortBreak = "shortBreak";
        public const string Sprint = "sprint";
        #endregion
    }
    public enum ClockState
    {
        Alarm,
        Clock,
        StopWatch
    }

}
