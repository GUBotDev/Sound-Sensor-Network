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
using NAudio.Wave;

namespace SenNetDataInterpreter.Class_Files
{
    static class PipeThread
    {
        private static volatile bool _continue = true;
        private static TcpListener tcpSTest;
        private static TcpListener tcpWNTest;
        private static TcpListener tcpARTest;
        private static TcpListener tcpWTest;
        private static TcpListener tcpFTest;
        public static Dictionary<string, DateTime> messages = new Dictionary<string, DateTime>();
        public static List<Tuple<string, DateTime, bool>> data = new List<Tuple<string, DateTime, bool>>();
        public static Dictionary<string, DateTime> files = new Dictionary<string, DateTime>();
        private const int saveFor = 24;//hours
        private const int saveMessagesFor = 10;//seconds

        public static void readNode()
        {
            Console.WriteLine("Read Thread Started.");
            tcpSTest = new TcpListener(IPAddress.Any, 10001);
            tcpSTest.Start();

            //inputStream = new NamedPipeClientStream("localhost", "senNetData
            while (_continue)
            {
                Socket client = tcpSTest.AcceptSocket();

                Console.WriteLine("Connection to node for reading established.");

                var childSocketThread = new Thread(() =>
                {
                    NetworkStream netStream = new NetworkStream(client);
                    StreamReader streamReader = new StreamReader(netStream);

                    try
                    {
                        while (true)
                        {
                            string line = streamReader.ReadLine();
                            string[] split = line.Split(' ');
                            string name = split[0];

                            Console.WriteLine("Read Node: " + line);

                            PipeParser.parsePipeString(line);
                        }
                    }
                    catch { }

                    client.Close();
                });

                childSocketThread.Start();
            }//end while
        }//end function
        
        public static void writeNode()
        {
            Console.WriteLine("Write Thread Started.");
            tcpWNTest = new TcpListener(IPAddress.Any, 10000);
            tcpWNTest.Start();
            
            while (_continue)
            {
                Socket client = tcpWNTest.AcceptSocket();

                Console.WriteLine("Connection to node for writing established.");

                var childSocketThread = new Thread(() =>
                {
                    NetworkStream netStream = new NetworkStream(client);
                    StreamWriter streamWriter = new StreamWriter(netStream);

                    List<string> sentMessages = new List<string>();

                    try
                    {
                        /*
                        messages.Add("Echo 5", DateTime.Now);
                        messages.Add("Echo 1", DateTime.Now);
                        messages.Add("Echo 2", DateTime.Now);
                        messages.Add("Echo 3", DateTime.Now);
                        messages.Add(("Request " + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss.ffff")), DateTime.Now);
                        messages.Add("Echo 4", DateTime.Now);
                        */
                        //Console.WriteLine(messages.ToArray().Length);

                        while (true)
                        {
                            
                            List<string> tempAllMessages;
                            
                            tempAllMessages = messages.Keys.ToList().Except(sentMessages).ToList();

                            if (tempAllMessages.Any())
                            {
                                streamWriter.WriteLine(tempAllMessages.ToList().First());
                                sentMessages.Add(tempAllMessages.ToList().First());

                                Console.WriteLine("Sent: " + tempAllMessages.ToList().First());

                                streamWriter.Flush();
                            }

                            if (messages.Any() && messages.ToList().First().Value.AddSeconds(saveMessagesFor) < DateTime.Now)
                            {
                                Console.WriteLine("Removed: " + messages.ToList().First().Key);

                                messages.Remove(messages.ToList().First().Key);
                            }

                            Thread.Sleep(10);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    client.Close();
                });

                childSocketThread.Start();
            }//end while
        }//end function

/// ///////////////////////////////////////////////////////
        public static void audioReadNode()
        {
            Console.WriteLine("Audio Read Thread Started.");
            tcpARTest = new TcpListener(IPAddress.Any, 9999);
            tcpARTest.Start();
            
            while (_continue)
            {
                Socket client = tcpARTest.AcceptSocket();

                Console.WriteLine("Connection to node for reading audio established.");

                var childSocketThread = new Thread(() =>
                {
                    NetworkStream netStream = new NetworkStream(client);
                    StreamReader streamReader = new StreamReader(netStream);
                    string line;
                    string[] split;
                    string name;
                    int bytes;
                    byte[] audioBytes;

                    List<string> sentMessages = new List<string>();

                    try
                    {
                        while (true)
                        {
                            //bytestream? convert (1, 44100) to wave, save, add to dictionary to send to interfaces
                            //send the stream of the sensor that triggered an event most recently

                            //transfer, save, add to files dictionary - rest is automatic


                            //files.Add(key, DateTime.Now);
                            line = streamReader.ReadLine();
                            split = line.Split(' ');

                            if (split[0] == "Audio")
                            {
                                name = split[1];
                                bytes = Convert.ToInt32(split[2]);
                                audioBytes = new byte[bytes];

                                netStream.Read(audioBytes, 0, bytes);

                                short[] sampleBufShort = new short[bytes / 2];
                                byte a, b;

                                for (int i = 0; i < bytes; i += 2)
                                {
                                    a = audioBytes[i];
                                    b = audioBytes[i + 1];

                                    sampleBufShort[i / 2] = returnShort(a, b);
                                }
                                
                                byte[] byteArray = new byte[bytes];
                                Buffer.BlockCopy(sampleBufShort, 0, byteArray, 0, byteArray.Length);

                                DateTime dt = DateTime.Now;
                                //string file = dt.ToString("hh:mm:ss:ffff");//Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".wav");
                                WaveFormat waveFormat = new WaveFormat(44100, 16, 1);
                                Stream memStream = new MemoryStream(byteArray);
                                WaveStream waveStream = new WaveStreamDer(memStream, waveFormat);

                                //var desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                                string file = "audio/" + name + ".wav";

                                WaveFileWriter.CreateWaveFile(file, waveStream);

                                Console.WriteLine("Audio downloaded");
                            }

                            Thread.Sleep(1);
                        }
                    }
                    catch { }

                    client.Close();
                });

                childSocketThread.Start();
            }//end while
        }//end function

        public static void write()
        {
            Console.WriteLine("Write Thread Started.");
            tcpWTest = new TcpListener(IPAddress.Any, 9998);
            tcpWTest.Start();
            
            while (_continue)
            {
                Socket client = tcpWTest.AcceptSocket();

                Console.WriteLine("Connection to interface established. Data streaming initializing...");

                var childSocketThread = new Thread(() =>
                {
                    NetworkStream writer = new NetworkStream(client);
                    StreamWriter streamWriter = new StreamWriter(writer);

                    List<Tuple<string, DateTime, bool>> dataSent = new List<Tuple<string, DateTime, bool>>();

                    try
                    {
                        Tuple<string, DateTime, bool> temp;

                        temp = Tuple.Create("Node,1,42.127,-80.087", DateTime.Now, true);

                        data.Add(temp);

                        temp = Tuple.Create("Node,2,42.1275,-80.0875", DateTime.Now, true);

                        data.Add(temp);

                        temp = Tuple.Create("Node,3,42.127,-80.0878", DateTime.Now, true);

                        data.Add(temp);

                        temp = Tuple.Create("42.12743,-80.0879", DateTime.Now, false);

                        data.Add(temp);

                        temp = Tuple.Create("42.12843,-80.0849", DateTime.Now, false);

                        data.Add(temp);

                        temp = Tuple.Create("42.127943,-80.0869", DateTime.Now, false);

                        data.Add(temp);

                        while (true)
                        {
                            List<Tuple<string, DateTime, bool>> tempAllData = new List<Tuple<string, DateTime, bool>>();

                            tempAllData = data.Except(dataSent).ToList();

                            if (tempAllData.ToList().Any())
                            {
                                streamWriter.WriteLine(tempAllData.ToList().First().Item1);
                                dataSent.Add(tempAllData.ToList().First());

                                streamWriter.Flush();
                            }

                            Thread.Sleep(1);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Write: " + ex.Message);
                    }

                    client.Close();
                });

                childSocketThread.Start();
            }//end while
        }//end function

        public static void writeFile()
        {
            Console.WriteLine("Write File Thread Started.");

            tcpFTest = new TcpListener(IPAddress.Any, 9997);
            tcpFTest.Start();

            if (!files.Any() && !files.ContainsKey("dressCodesS"))
            {
                files.Add("dressCodesS", DateTime.Now);
            }

            while (_continue)
            {
                Socket client = tcpFTest.AcceptSocket();

                Console.WriteLine("Connection to interface file client established. File streaming initializing...");

                var childSocketThread = new Thread(() =>
                {
                    NetworkStream writer = new NetworkStream(client);
                    StreamWriter streamWriter = new StreamWriter(writer);
                    StreamReader streamReader = new StreamReader(writer);

                    List<string> filesSent = new List<string>();

                    try
                    {
                        Thread.Sleep(2000);

                        string input = streamReader.ReadLine();
                        string[] split;

                        if (input == "Existing_Files")
                        {
                            input = streamReader.ReadLine();
                            split = input.Split(' ');

                            filesSent.AddRange(split.ToList());
                        }

                        while (true)
                        {
                            List<string> tempAllFiles;
                            string file;
                            string firstFile;

                            tempAllFiles = files.Keys.ToList().Except(filesSent).ToList();

                            if (tempAllFiles.ToList().Any())
                            {
                                firstFile = tempAllFiles.First();
                                file = "audio/" + firstFile + ".wav";

                                //existing files   <-

                                //file name, bytes ->
                                //ready for send   <-
                                //send audio       ->

                                long fileLength = new FileInfo(file).Length;
                                streamWriter.WriteLine("File: " + firstFile + ".wav " + fileLength);
                                streamWriter.Flush();

                                Console.WriteLine("File: " + firstFile + ".wav " + fileLength);

                                string line = (string)streamReader.ReadLine();

                                Console.WriteLine(line);

                                if (line == "Ready")
                                {
                                    Console.WriteLine("Sending {0}, {1} bytes to the client.", file, fileLength);

                                    client.SendFile(file);

                                    filesSent.Add(firstFile);
                                }
                            }

                            Thread.Sleep(1);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("FileThread: " + ex.Message);

                        files.Remove("dressCodesS");
                    }

                    client.Close();
                });

                childSocketThread.Start();
            }
        }

        public static void checkFile()
        {
            Console.WriteLine("Check File Thread Started.");

            while (_continue)
            {
                try
                {
                    if (files.Any() && files.First().Value.AddHours(saveFor) < DateTime.Now)
                    {
                        if (File.Exists("audio/" + files.First().Key + ".wav"))
                        {
                            File.Delete("audio/" + files.First().Key + ".wav");
                        }

                        files.Remove(files.First().Key);
                    }
                }
                catch { }

                Thread.Sleep(10);
            }
        }

        public static void checkData()
        {
            Console.WriteLine("Check Data Thread Started.");

            while (_continue)
            {
                try
                {
                    if (data.Any() && data.First().Item2.AddHours(saveFor) < DateTime.Now)
                    {
                        data.Remove(data.First());
                    }
                }
                catch { }

                Thread.Sleep(10);
            }
        }

        private static short returnShort(byte a, byte b)
        {
            string inputA = Convert.ToString(a, 2).PadLeft(8, '0');
            string inputB = Convert.ToString(b, 2).PadLeft(8, '0');

            string shortString = inputA + inputB;

            short output = Convert.ToInt16(shortString, 2);

            return output;
        }
    }

    /// <summary>
    /// Used to convert 16 bit data to wave files
    /// </summary>
    public class WaveStreamDer : WaveStream
    {
        private Stream sourceStream;
        private WaveFormat waveFormat;

        public WaveStreamDer(Stream sourceStream, WaveFormat waveFormat)
        {
            this.sourceStream = sourceStream;
            this.waveFormat = waveFormat;
        }

        public override WaveFormat WaveFormat
        {
            get { return this.waveFormat; }
        }

        public override long Length
        {
            get { return this.sourceStream.Length; }
        }

        public override long Position
        {
            get
            {
                return this.sourceStream.Position;
            }
            set
            {
                this.sourceStream.Position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return sourceStream.Read(buffer, offset, count);
        }
    }
}
