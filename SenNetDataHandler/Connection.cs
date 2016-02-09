using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;

namespace SenNetDataHandler.Class_Files
{
    class Connection
    {
        SerialPort _serialPort;
        ConsoleRead readClass;
        Thread readThread;
        public bool portDisc = false;

        public void connect(string port, Connection connection)
        {
            try
            {
                _serialPort = new SerialPort();

                _serialPort.PortName = port;
                _serialPort.BaudRate = Program.baudRate;
                _serialPort.DataBits = 8;
                _serialPort.Parity = Parity.None;
                _serialPort.StopBits = StopBits.One;

                _serialPort.ReadBufferSize = 8;

                _serialPort.Open();
                portDisc = false;
            }
            catch // (Exception exception)
            {
                //if port is physically disconnected this will repeat due to stale data
                //Console.WriteLine("Connect: " + exception.Message);
                disconnect(true);
                portDisc = true;
            }
        }

        public void disconnect(bool isInitConn)
        {
            try
            {
                _serialPort.Close();
                _serialPort.Dispose();

                if (isInitConn == false)
                {
                    readClass._continue = false;
                    readThread.Join();
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine("Disconnect: " + exception.Message);
            }
        }

        public void reset(int node, string port, Connection connection)
        {
            try
            {
                Console.WriteLine("Automatic reset in progress.");
                _serialPort.Close();
                Console.WriteLine("Disconnected from port " + port + ".");

                for (int i = 0; i < 100; i++)
                {
                    Thread.Sleep(120);
                    Console.Write("\rReset progress: {0}%", i + 1);
                }

                Console.Write("\rReset progress: {0}%", 100);

                Console.WriteLine("\nReconnecting to " + port + ".");

                _serialPort.Open();

                readClass = new ConsoleRead();
                readClass._continue = true;
                readThread = new Thread(() => readClass.Read(_serialPort, node, port, connection));

                readThread.Start();
                Console.WriteLine("Port " + port + " connected as node " + node + ".");

                Console.WriteLine("Connection to port " + port + " successfully reset.");

            }
            catch (Exception exception)
            {
                Console.WriteLine("Reset: " + exception.Message);
                disconnect(false);
            }
        }
    }

    //detect ports, add to list, connect
    static class DetectPortThread
    {
        static bool _continue = true;
        public static Dictionary<int, Connection> connContainer = new Dictionary<int, Connection>();
        public static List<string> portList = new List<string>();

        public static void connect()
        {
            while (_continue)
            {
                try
                {
                    List<string> portDiff = new List<string>(SerialPort.GetPortNames().ToList().Except(portList));

                    foreach (string port in portDiff)
                    {
                        //CO 3
                        string[] portSplit = port.Split('M');
                        int portNumber = int.Parse(portSplit[1]);

                        Connection connection = new Connection();

                        connection.connect(port, connection);

                        if (connection.portDisc == false)
                        {
                            connContainer.Add(portNumber, connection);
                            connection.reset(portNumber, port, connection);
                            portList.Add(port);
                        }
                        /*
                        else
                        {
                            Console.Write("\rPort not connected");
                        }
                        */
                    }

                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }//end while

        }

    }
}
