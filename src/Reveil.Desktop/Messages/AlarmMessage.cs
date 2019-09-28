using System;

namespace Reveil.Desktop.Messages
{
    public class AlarmMessage
    {
        public AlarmMessage(DateTime alarm)
        {
            Alarm = alarm;
        }


        public DateTime Alarm
        {
            get;
            set;
        }
    }
}
