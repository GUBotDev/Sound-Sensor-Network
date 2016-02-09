using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SenNetDataHandler.Class_Files
{
    public class DecisionHandler
    {
        ParseSensor parSens = new ParseSensor();
        StreamWriter nodeWriter;
        bool checkTable = true;
        bool createFile = true;
        static int fileCapTime = 24;// hours
        DateTime newFileDate = DateTime.Now.AddHours(fileCapTime);

        public void readInput(byte[] message, int node, DecisionHandler decHand)
        {
            parSens.parSenVals(message, decHand, node);
        }

        public void interpretInput(byte[] parsedValues, byte[] parsedInfo, int node)
        {
            //string info = parSens.getDateTime().ToString("yyyy-MM-dd_HH:mm:ss") + " " + parsedInfo[0] * parsedInfo[1] + " " + parsedInfo[2] * parsedInfo[3] + " " + parsedValues[0] + " " + parsedValues[1] + " " + parsedValues[2] + " " + parsedValues[3] + " " + parsedValues[4] + " " + parsedValues[5];

            //HandleData.analyzeData(node, parsedValues, parsedInfo, parSens.getDateTime());
            
            //////////////////////////////////////////////////////////////////////////////////////////////////////
            HandleData.storeDataInternal(ref createFile, ref nodeWriter, node, parsedValues, parsedInfo, parSens.getDateTime(), ref newFileDate, fileCapTime);

            //HandleData.storeStringExternal(parsedInfo[0], info);


            //HandleData.storeDataExternal(ref checkTable, parsedValues, parsedInfo, parSens.getDateTime());
            //Console.WriteLine("Stored to Database");

        }

        private bool determineEvent(byte[] parsedValues)
        {
            bool temp = false;
            
            foreach (byte element in parsedValues)
            {
                //assuming 128 is zero dB
                if (element >= 128 + 64) //128 = 0 -> determine digital dB relation to real dB -> determine threshold
                {
                    temp = true;
                }
                else if (element <= 128 - 64)
                {
                    temp = true;
                }
            }

            return temp;
        }
        
    }
}
