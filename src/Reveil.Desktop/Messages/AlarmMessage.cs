using System;

namespace Reveil.Messages
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
