using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;

namespace SenNetDataHandler.Class_Files
{
    class ConsoleRead
    {
        public bool _continue = true;
        DecisionHandler decHand = new DecisionHandler();
        ParseSensor parsSens = new ParseSensor();
        Stopwatch watch = new Stopwatch();
        int freq = 0;

        public void Read(SerialPort _serialPort, int commNum, string portName, Connection connection)
        {
            while (_continue)
            {
                try
                {
                    int bytes = _serialPort.BytesToRead;

                    //Console.WriteLine(bytes);
                    
                    if (bytes > 0)
                    {
                        watch.Start();
                        byte[] buffer = new byte[bytes];
                        _serialPort.Read(buffer, 0, bytes);

                        decHand.readInput(buffer, commNum, decHand);

                        freq += buffer.Length;

                        if (watch.ElapsedMilliseconds > 5000)
                        {
                            watch.Stop();
                            Console.WriteLine("Sensor Node " + commNum + " Data Frequency (Bytes per Second): " + (freq / watch.Elapsed.Seconds));// 2 = freq/watch / 12 * 6

                            freq = 0;
                            watch.Reset();
                        }
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Read: " + exception.Message);
                    Console.WriteLine(portName + " disconnected :" + commNum);
                    DetectPortThread.connContainer.Remove(commNum);
                    DetectPortThread.portList.Remove(portName);

                    connection.disconnect(false);
                }
            }//end while
        }//end void

    }
}
