using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipes;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace SenNetDataHandler.Class_Files
{
    static class PipeHandler
    {
        static TcpClient tcpCTest = new TcpClient("10.1.24.243", 9999);
        static NetworkStream netStream = tcpCTest.GetStream();
        static StreamWriter streWriter = new StreamWriter(netStream);
        public static Dictionary<int, string> dataStore = new Dictionary<int, string>();
        public static int num = 0;
        private static bool _continue = true;
        public static string dataString;

        public static void pipeThread(string sendString)
        {
            try
            {
                streWriter.WriteLine(sendString);
                streWriter.Flush();

                //while (_continue)
                //{
                //string sendString = "1";
                /*
                for (int i = 0; i < dataStore.Keys.Count; i++)
                {
                    sendString += dataStore.Values.ElementAt(i);

                    if (i != dataStore.Keys.Count - 1)
                    {
                        sendString += ":";
                    }
                }*/


                //Console.WriteLine(dataStore.Values.First());
                //dataStore.Remove(dataStore.First().Key);

                //streWriter.WriteLine(sendString.ToString());
                //streWriter.WriteLine(dataString);
                //streWriter.Flush();

                //Console.WriteLine(sendString);
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                tcpCTest = new TcpClient("10.1.24.243", 9999);
                netStream = tcpCTest.GetStream();
                streWriter = new StreamWriter(netStream);
            }


        }// end while

    }// end function


}