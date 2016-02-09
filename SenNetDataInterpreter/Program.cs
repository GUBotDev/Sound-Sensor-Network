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

            Thread readThread = new Thread(() => PipeReadThread.read());
            readThread.Start();
            
            Thread writeThread = new Thread(() => PipeReadThread.write());
            writeThread.Start();
            //Console.ReadLine();
        }
    }
}