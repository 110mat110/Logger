using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Logger
{
    public class GlobalLogger
    {
        private MQTTLogger MQTTLogger = null;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static GlobalLogger() {
        }

        private GlobalLogger() {
        }

        public static GlobalLogger Instance { get; } = new GlobalLogger() {
            UseMQTT = false,
        };
        public static string FileAdress {
            set {
                Instance.fileName = value;
            }
            get {
                return DateTime.Now.ToString("yyyyMMdd") + "_" + Instance.fileName;
            }
        }
        private string fileName = "log.txt";
        private Queue<ILog> messageQuery = new Queue<ILog>();
        private object locker = new object();
        private bool isLoopWorking = false;

        public string MQTTName { private get; set; }
        public string MQTTPassword { private get; set; }
        public string MQTTBroker { private get; set; }

        public bool UseMQTT { private get; set; }


        public static void SetMQTT(string name, string password, string broker) {
            if (Instance.MQTTLogger != null) {
                Instance.MQTTLogger.Unsubscribe();
                Instance.MQTTLogger = null;
            }

            Instance.MQTTName = name;
            Instance.MQTTPassword = password;
            Instance.MQTTBroker = broker;

            Instance.ConnectToMQTT();
        }

        private void ConnectToMQTT() {
            Instance.MQTTLogger = new MQTTLogger(MQTTName, MQTTPassword, MQTTBroker);
            UseMQTT = true;
        }

        public static void AddInputToQueue(ILog message) {
            lock (Instance.locker) {
                Instance.messageQuery.Enqueue(message);
            }

            Instance.LoopAsync();
        }

        private void HandleMQTT() {
            if (Instance.MQTTLogger == null) {
                Instance.ConnectToMQTT();
            }
            Instance.MQTTLogger.Reconnect();
        }

        private void LoopAsync() {
            if (isLoopWorking) return;

            isLoopWorking = true;

            while (messageQuery.Count > 0) {
                ILog action = null;
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
                    if (action.Urgency >= Action.Notification && Instance.UseMQTT) {
                        Instance.HandleMQTT();
                        Instance.MQTTLogger.Publish(action.ToString());
                    }
                }
            }
            isLoopWorking = false;
        }
    }
}
