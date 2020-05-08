using System;
using System.Drawing;

namespace Logger
{

    public class Log : ILog
    {
        public Log() { }
        public Log(string Text, LogType type, Action urgency) {
            Type = type;
            Message = Text;
            Created = DateTime.Now;
            Urgency = urgency;
        }
        public Log(Exception ex) {
            Type = LogType.Error;
            Message = ex.Message;
            Created = DateTime.Now;
        }
    }
}
