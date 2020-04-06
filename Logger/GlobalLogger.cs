using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Logger
{
    public class GlobalLogger
    {
        private MQTTLogger MQTTLogger = new MQTTLogger();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static GlobalLogger() {
        }

        private GlobalLogger() {
        }

        public static GlobalLogger Instance { get; } = new GlobalLogger();
        public static string FileAdress {
            set {
                Instance.fileName = value;
            }
            get {
                return DateTime.Now.ToString("yyyyMMdd") + "_" + Instance.fileName;
            }
        }
        private string fileName = "log.txt";
        private Queue<Log> messageQuery = new Queue<Log>();
        private object locker = new object();
        private bool isLoopWorking = false;

        public static void AddInputToQueue(Log message) {
            lock (Instance.locker) {
                Instance.messageQuery.Enqueue(message);
            }

            Instance.LoopAsync();
        }

        private void LoopAsync() {
            if (isLoopWorking) return;

            isLoopWorking = true;

            while (messageQuery.Count > 0) {
                Log action = null;
                lock (locker) {
                    action = messageQuery.Dequeue();
                }
                if (action != null) {
                        if (action.Urgency >= Action.Debug)
                            Debug.WriteLine(action.ToString());
                        if (action.Urgency >= Action.Log)
                            File.AppendAllText(FileAdress, action.ToString() + '\n');
                        //if (action.Urgency >= Action.CloudLog) 
                        //Todo
                        if (action.Urgency >= Action.Notification) {
                            MQTTLogger.Publish(action.ToString());
                        }
                }
            }
            isLoopWorking = false;
        }
    }
}
