using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipes;
using System.Threading;

namespace SenNetDataInterpreter.Class_Files
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Threads.");
            
            Thread readThread = new Thread(() => PipeThread.readNode());
            readThread.Start();
            
            Thread writeThread = new Thread(() => PipeThread.write());
            writeThread.Start();

            Thread readAudioThread = new Thread(() => PipeThread.audioReadNode());
            readAudioThread.Start();

            Thread fileThread = new Thread(() => PipeThread.writeFile());
            fileThread.Start();
            
            Thread fileCheck = new Thread(() => PipeThread.checkFile());
            fileCheck.Start();

            Thread dataCheck = new Thread(() => PipeThread.checkData());
            dataCheck.Start();
        }
    }
}