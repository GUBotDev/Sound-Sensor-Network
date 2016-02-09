using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.IO;
using System.Media;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace SenNetDataHandler.Class_Files
{
    /// <summary>
    /// GunShot peak overpressure lasts approximately 0.000125 seconds, requirement to pickup all shots at peak height - 8,000Hz - loses 15dB in 0.0005 seconds
    /// 
    /// Current frequency: 8500 -> 10000Hz
    /// One full measurement: 778 microseconds -> all sensors return and write to serial
    /// Current bitrate: ~150kbps
    /// 
    /// 1 node 2.88 gigabytes per 24 hours - uart not xbee
    /// 
    /// </summary>

    class HandleData
    {
        public static bool checkTable = true;
        static Stopwatch watch = new Stopwatch();
        static int freq = 0;

        public static void analyzeData(int node, byte[] parsedValues, byte[] parsedInfo, DateTime dateTime)
        {
            try
            {
                string data = dateTime.ToString("yyyy-MM-dd_HH-mm-ss.ffff") + " " + parsedInfo[0] + " " + parsedInfo[1] * parsedInfo[2] + " " + parsedInfo[3] * parsedInfo[4] + " " + parsedValues[0] + " " + parsedValues[1] + " " + parsedValues[2] + " " + parsedValues[3] + " " + parsedValues[4] + " " + parsedValues[5];

                if (PipeHandler.dataStore.ContainsKey(parsedInfo[0]) == false)
                {
                    //PipeHandler.dataStore.Add(parsedInfo[0], data);
                    PipeHandler.pipeThread(data);
                    //Console.WriteLine(parsedInfo[0] + " " + data);
                }
                else
                {
                    PipeHandler.dataStore[parsedInfo[0]] = data;
                    PipeHandler.num++;
                }
                
            }
            catch (Exception exception)
            {
                Console.WriteLine("Analyze Data: " + exception.Message);
            }

        }


        /// <param name="data">The data storage object</param>
        /// <param name="node">The nodes port number</param>
        /// <param name="parsedValues">The parsed sound sensor values</param>
        /// <param name="parsedInfo">The information parsed from the sensor </param>
        /// <param name="dateTime">The datetime of the data arrival</param>
        public static void storeDataInternal(ref bool createFile, ref StreamWriter _nodeWriter, int node, byte[] parsedValues, byte[] parsedInfo, DateTime dateTime, ref DateTime newFileDate, int fileCapTime)
        {
            //internal datacontainer will be used to generate audio files or will be deleted - dependent on the time required to analyze the data

            /*
            DataContainer.nodeData[node].updateTime = dateTime;

            DataContainer.nodeData[node].positionX = parsedValues[0] * parsedValues[1];
            DataContainer.nodeData[node].positionY = parsedValues[2] * parsedValues[3];

            //write wav data
            List<byte> tempList = parsedValues.ToList();
            data.wavData.Add(dateTime, tempList);
            */
            

            if (createFile)
            {
                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\test";
                Directory.CreateDirectory(folderPath);
                _nodeWriter = new StreamWriter(folderPath + @"\node" + parsedInfo[0].ToString() + "_" + dateTime.ToString("yyyy-MM-dd_HH-mm-ss") + ".txt");
                createFile = false;
            }

            if (DateTime.Now > newFileDate)
            {
                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\test";
                _nodeWriter.Close();
                _nodeWriter = new StreamWriter(folderPath + @"\node" + parsedInfo[0].ToString() + "_" + dateTime.ToString("yyyy-MM-dd_HH-mm-ss") + ".txt");

                newFileDate = DateTime.Now.AddHours(fileCapTime);
            }

            _nodeWriter.Write(dateTime.ToString("yyyy-MM-dd_HH-mm-ss.ffff") + " " + parsedInfo[0] + " " + parsedInfo[1] * parsedInfo[2] + " " + parsedInfo[3] * parsedInfo[4] + " " + parsedValues[0] + " " + parsedValues[1] + " " + parsedValues[2] + " " + parsedValues[3] + " " + parsedValues[4] + " " + parsedValues[5] + "\n");
        }

        /// <param name="checkTable">Checks the datatables existence, creates if it does not exist</param>
        /// <param name="parsedValues">The sound sensor values</param>
        /// <param name="parsedInfo">The information from the sensor</param>
        /// <param name="dateTime">The datetime of data arrival</param>
        public static void storeDataExternal(byte[] parsedValues, byte[] parsedInfo, DateTime dateTime)
        {
            if (checkTable)
            {
                MySqlCommand cmd1 = new MySqlCommand("CREATE TABLE IF NOT EXISTS node" + parsedInfo[0] + " LIKE sensorTable", mySQLDB.connection);
                cmd1.ExecuteNonQuery();
                checkTable = false;
            }

            MySqlCommand cmd2 = new MySqlCommand("INSERT INTO node" + parsedInfo[0] + "(dateTime, positionX, positionY, sensOne, sensTwo, sensThr, sensFou, sensFiv, sensSix) VALUES(@dateTime, @positionX, @positionY, @sensOne, @sensTwo, @sensThr, @sensFou, @sensFiv, @sensSix)", mySQLDB.connection);
            cmd2.Parameters.AddWithValue("@dateTime", dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd2.Parameters.AddWithValue("@positionX", parsedValues[0] * parsedValues[1]);
            cmd2.Parameters.AddWithValue("@positionY", parsedValues[2] * parsedValues[3]);
            cmd2.Parameters.AddWithValue("@sensOne", parsedValues[0]);
            cmd2.Parameters.AddWithValue("@sensTwo", parsedValues[1]);
            cmd2.Parameters.AddWithValue("@sensThr", parsedValues[2]);
            cmd2.Parameters.AddWithValue("@sensFou", parsedValues[3]);
            cmd2.Parameters.AddWithValue("@sensFiv", parsedValues[4]);
            cmd2.Parameters.AddWithValue("@sensSix", parsedValues[5]);
            cmd2.ExecuteNonQueryAsync();

        }

        public static void storeStringExternal(int node, string input)
        {
            if (checkTable)
            {
                MySqlCommand cmd1 = new MySqlCommand("CREATE TABLE IF NOT EXISTS node" + node + " LIKE sensorTable2", mySQLDB.connection);
                cmd1.ExecuteNonQuery();
                checkTable = false;
            }

            MySqlCommand cmd2 = new MySqlCommand("INSERT INTO node" + node + "(string) VALUES(@string)", mySQLDB.connection);
            cmd2.Parameters.AddWithValue("@string", input);
            cmd2.ExecuteNonQueryAsync();
            
        }
    }
}