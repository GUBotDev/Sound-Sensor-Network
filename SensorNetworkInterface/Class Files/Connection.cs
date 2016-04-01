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
        public static volatile bool _continue = true;

        public static TcpClient tcpRead;
        static NetworkStream netRead;
        static StreamReader streRead;

        public static TcpClient tcpFile;
        static NetworkStream netFile;
        static StreamReader streFile;
        static StreamWriter streFileWriter;

        static bool hasBeenRead = false;

        static string ip = Program.mainForm.returnIP();

        public static void connThread()
        {
            Program.mainForm.printConsole("Marker thread waiting 2.5 seconds.");

            Thread.Sleep(1000);

            Program.mainForm.printConsole("Marker thread started.");
            
            while (_continue)
            {
                try
                {
                    tcpRead = new TcpClient(ip, 9998);
                    netRead = tcpRead.GetStream();
                    streRead = new StreamReader(netRead);

                    Program.mainForm.printConsole("Connected to Interpreter.");
                    
                    while (_continue)
                    {
                        string line = (string)streRead.ReadLine();
                        string[] lineSplit = line.Split(',');

                        if (lineSplit[0].Contains("Node"))
                        {
                            int nodeNum = Convert.ToInt32(lineSplit[1]);
                            double x = Convert.ToDouble(lineSplit[2]);
                            double y = Convert.ToDouble(lineSplit[3]);

                            Program.mainForm.printConsole("Node: " + line);

                            MapInterface.addNode(nodeNum, x, y);
                        }
                        else if (lineSplit.Length == 2)
                        {
                            if (hasBeenRead)
                            {
                                Program.mainForm.printConsole("");
                            }

                            Program.mainForm.printConsole("Event: " + line);

                            double x = Convert.ToDouble(lineSplit[0]);
                            double y = Convert.ToDouble(lineSplit[1]);

                            MapInterface.createMarker(x, y, "Test");

                            hasBeenRead = false;
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
                    Program.mainForm.printConsole("Main connection disrupted, trying to reconnect...");//"ConnThread: " + ex.Message);
                    Thread.Sleep(5000);
                }
            }
        }


        public static void fileThread()
        {
            bool wasSent = false;

            Program.mainForm.printConsole("File thread waiting 2.5 seconds.");

            Thread.Sleep(2500);

            Program.mainForm.printConsole("File thread started.");

            while (_continue)
            {
                try
                {
                    string allFiles = "";

                    tcpFile = new TcpClient(ip, 9997);
                    netFile = tcpFile.GetStream();
                    streFile = new StreamReader(netFile);
                    streFileWriter = new StreamWriter(netFile);

                    Program.mainForm.printConsole("Connected to Interpreter for File Transfer.");

                    if (!wasSent)
                    {
                        string[] temp = Directory.GetFiles("audio/", "*.wav").Select(fileName => Path.GetFileNameWithoutExtension(fileName)).ToArray();

                        foreach (string input in temp)
                        {
                            allFiles += input + " ";
                        }

                        streFileWriter.WriteLine("Existing_Files");
                        streFileWriter.Flush();

                        streFileWriter.WriteLine(allFiles);
                        streFileWriter.Flush();

                        wasSent = true;
                    }

                    while (_continue)
                    {
                        string line = (string)streFile.ReadLine();
                        string[] lineSplit = line.Split(' ');
                        long bytes;
                        string name;
                        DateTime dt = DateTime.Now;

                        //Program.mainForm.printConsole(line);

                        if (lineSplit[0] == "File:")
                        {
                            name = lineSplit[1];
                            bytes = Convert.ToInt64(lineSplit[2]);

                            streFileWriter.WriteLine("Ready");
                            streFileWriter.Flush();

                            using (var output = File.Create("audio/" + name))
                            {
                                Program.mainForm.printConsole("Server sending audio");

                                var buffer = new byte[bytes];
                                int bytesRead = 0;
                                int bytesLeft = Convert.ToInt32(bytes);

                                while (bytesLeft != 0)
                                {
                                    bytesRead = netFile.Read(buffer, 0, buffer.Length);

                                    //Program.mainForm.printConsole(bytes + " " + bytesLeft);

                                    output.Write(buffer, 0, bytesRead);

                                    bytesLeft -= bytesRead;
                                }

                                Program.mainForm.printConsole("Audio received");
                            }

                            Program.mainForm.Invoke(new Action(() =>
                            {
                                string[] nameS = name.Split('.');

                                Program.mainForm.addWavToListBox(nameS[0]);
                            }
                            ));

                            Program.mainForm.printConsole(name + " added.");
                        
                        }
                    }
                }
                catch (Exception ex)
                {
                    Program.mainForm.printConsole("File connection disrupted, trying to reconnect...");//"FileThread: " + ex.Message);
                    Thread.Sleep(5000);
                }
            }
        }
    }
}
