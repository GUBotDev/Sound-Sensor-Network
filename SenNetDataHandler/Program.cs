using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using System.IO;
using System.Media;

namespace SenNetDataHandler.Class_Files
{
    class Program
    {
        public static string[] ports;
        public static int baudRate = 115200;
        public static Thread connectThread;
        //public static Thread pipeThread = new Thread(() => PipeHandler.pipeThread());

        // information removed for GitHub upload
        static string server = "";
        static string database = "sensornetwork";
        static string userId = "";
        static string userPass = "";

        static void Main(string[] args)
        {
            Console.WriteLine("Start");
            //mySQLDB.connect(server, database, userId, userPass);

            bool initNoPorts = false;
            ports = SerialPort.GetPortNames();
            

            if (ports.Length == 0)
            {
                Console.WriteLine("No ports available");
                initNoPorts = true;
            }
            else
            {
                //connect();
                connectThread = new Thread(() => DetectPortThread.connect());
                connectThread.Start();
                //pipeThread.Start();
            }

            while (ports.Length == 0)
            {
                ports = SerialPort.GetPortNames();
            }

            if (initNoPorts == true)
            {
                //connect();
                connectThread = new Thread(() => DetectPortThread.connect());
                connectThread.Start();
                //pipeThread.Start();
            }

            Console.ReadLine();
        }

        
    }
}