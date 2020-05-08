using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public enum LogType { Runtime, Recieved, Send, Error, Initialization, Other }
    public enum Action { Debug, Log, CloudLog, Notification }

    public abstract class ILog
    {
        public int id { get; set; }
        public LogType Type { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }
        public Action Urgency { get; set; }

        public string Prefix {
            get {
                return GetPrefix(Type);
            }
        }
        public Color Color {
            get {
                if (Type == LogType.Runtime) return Color.AliceBlue;
                if (Type == LogType.Recieved) return Color.Green;
                if (Type == LogType.Send) return Color.Blue;
                if (Type == LogType.Error) return Color.Red;
                if (Type == LogType.Initialization) return Color.Yellow;
                return Color.Orange;
            }
        }

        public override string ToString() {
            return Prefix + " " + Created.ToString("dd.MM.yyyy HH:mm:ss") + ": " + Message;
        }

        public static string GetPrefix(LogType Type) {
            if (Type == LogType.Runtime) return "RNT";
            if (Type == LogType.Recieved) return "RVD";
            if (Type == LogType.Send) return "SND";
            if (Type == LogType.Error) return "ERR";
            if (Type == LogType.Initialization) return "INT";
            return "OTH";
        }
    }
}
