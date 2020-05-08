using Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args) {
            GlobalLogger.SetMQTT("****", "****", "io.adafruit.com");
            int i = 0;
            while (true) {
                Console.ReadLine();
                GlobalLogger.AddInputToQueue(new Log("Test no: " + i.ToString(), LogType.Other, Logger.Action.Notification));
                i++;
            }


        }

        static async void DoThing() {

        }
    }
}
