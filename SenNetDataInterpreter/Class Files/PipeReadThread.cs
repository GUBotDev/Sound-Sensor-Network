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
using System.Diagnostics;

namespace SenNetDataInterpreter.Class_Files
{
    static class PipeReadThread
    {
        private static bool _continue = true;
        private static int attempts = 0;
        static TcpListener tcpSTest;
        static TcpListener tcpWTest;
        static Stopwatch watch = new Stopwatch();
        static int freq = 0;
        public static List<string> data = new List<string>();
        
        public static void read()
        {
            Console.WriteLine("Read Thread Started.");
            tcpSTest = new TcpListener(IPAddress.Any, 9999);
            tcpSTest.Start();

            //inputStream = new NamedPipeClientStream("localhost", "senNetData
            while (_continue)
            {
                while (true) // Add your exit flag here
                {
                    Console.WriteLine("Waiting for handler connection...");
                    Socket client = tcpSTest.AcceptSocket();

                    Console.WriteLine("Connection to handler established.");

                    var childSocketThread = new Thread(() =>
                    {
                        NetworkStream reader = new NetworkStream(client);
                        StreamReader streamReader = new StreamReader(reader);

                        try
                        {
                            while (true)
                            {

                                var line = streamReader.ReadLine();
                                //Console.WriteLine(line);

                                freq++;
                                //Console.WriteLine(line);
                                PipeParser.parsePipeString(line);

                                watch.Start();
                                if (watch.ElapsedMilliseconds > 5000)
                                {
                                    watch.Stop();

                                    //Console.WriteLine(freq);

                                    freq = 0;
                                    watch.Reset();
                                }
                            }
                        }
                        catch
                        {

                        }
                        client.Close();
                    });
                    childSocketThread.Start();
                }

                

                //PipeParser.parsePipeString(data);

                /*
                //Console.WriteLine(pipeStreamReader.ReadLine().ToString());
                if (tcpSTest)
                {

                }
                else
                {
                    attempts++;
                    Console.Write("\rTrying connection. Attempt: {0}", attempts);

                    try
                    {
                        tcpCTest.Connect("10.1.24.222", 8001);
                    }
                    catch
                    {
                        Console.Write("\rConnection Failed. Attempt: {0}", attempts);
                        Thread.Sleep(2000);
                    }
                }*/
            }//end while
        }//end function

        public static void write()
        {
            Console.WriteLine("Write Thread Started.");
            tcpWTest = new TcpListener(IPAddress.Any, 9998);
            tcpWTest.Start();

            //inputStream = new NamedPipeClientStream("localhost", "senNetData
            while (_continue)
            {
                while (true) // Add your exit flag here
                {
                    Console.WriteLine("Waiting for interface connection...");
                    Socket client = tcpWTest.AcceptSocket();

                    Console.WriteLine("Connection to interface established. Data streaming initializing...");

                    var childSocketThread = new Thread(() =>
                    {
                        NetworkStream writer = new NetworkStream(client);
                        StreamWriter streamWriter = new StreamWriter(writer);

                        try
                        {
                            //data.Add("Node,1,42.127,-80.087");
                            //data.Add("Node,2,42.1275,-80.0875");
                            //data.Add("42.12743,-80.0879");

                            /*
                            float lat = 42;
                            float lon = -80;

                            data.Add("");
                            */

                            while (true)
                            {
                                //Thread.Sleep(250);

                                if (data.Any())
                                {
                                    //data.Add(lat + "," + lon);

                                    data.Remove(data.First());
                                    //Console.WriteLine(data.First());

                                    streamWriter.WriteLine(data.First());

                                    streamWriter.Flush();
                                }
                                
                                /*
                                lat += 0.0005F;
                                lon -= 0.0005F;
                                */

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        client.Close();
                    });
                    childSocketThread.Start();
                }



                //PipeParser.parsePipeString(data);

                /*
                //Console.WriteLine(pipeStreamReader.ReadLine().ToString());
                if (tcpSTest)
                {

                }
                else
                {
                    attempts++;
                    Console.Write("\rTrying connection. Attempt: {0}", attempts);

                    try
                    {
                        tcpCTest.Connect("10.1.24.222", 8001);
                    }
                    catch
                    {
                        Console.Write("\rConnection Failed. Attempt: {0}", attempts);
                        Thread.Sleep(2000);
                    }
                }*/
            }//end while
        }//end function
    }
}
