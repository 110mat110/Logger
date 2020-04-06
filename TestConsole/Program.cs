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
                GlobalLogger.AddInputToQueue(new Log("Test", LogType.Other, Logger.Action.Notification));
        }
    }
}
