using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.IO;

namespace SensorNetworkInterface.Class_Files
{
    static class Connection
    {
        static TcpClient tcpCTest;
        static NetworkStream netStream;
        static StreamReader streReader;
        static bool hasBeenRead = false;

        public static void connThread()
        {
            Thread.Sleep(1000);

            Console.WriteLine("Connection thread started.");

            while (true)
            {
                try
                {
                    tcpCTest = new TcpClient("10.1.24.243", 9998);
                    netStream = tcpCTest.GetStream();
                    streReader = new StreamReader(netStream);

                    Console.WriteLine("Connected to Interpreter.");
                    
                    while (true)
                    {
                        string line = (string)streReader.ReadLine();

                        string[] lineSplit = line.Split(',');

                        if (lineSplit.Length == 2)
                        {
                            if (hasBeenRead)
                            {
                                Console.WriteLine();
                            }

                            Console.WriteLine(line);

                            double x = Convert.ToDouble(lineSplit[0]);
                            double y = Convert.ToDouble(lineSplit[1]);

                            UserInterface.createMarker(x, y, "Test");

                            hasBeenRead = false;
                        }
                        else if (lineSplit[0].Contains("Node"))
                        {
                            int nodeNum = Convert.ToInt32(lineSplit[1]);
                            double x = Convert.ToDouble(lineSplit[2]);
                            double y = Convert.ToDouble(lineSplit[3]);

                            Console.WriteLine(line);

                            UserInterface.addNode(nodeNum, x, y);
                        }
                        else
                        {
                            Console.Write("\r" + line);

                            hasBeenRead = true;
                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ConnThread: " + ex.Message);
                    Thread.Sleep(2500);
                }
            }

            
        }
    }
}
