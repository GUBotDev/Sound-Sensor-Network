using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenNetDataInterpreter.Class_Files
{
    /// <summary>
    /// store 1 min worth of bytes here - if triggered store next 30 secs, next convert to wav and send file via smtp
    /// if triggers during the 30 seconds then increase the span by 30 secs
    /// 
    /// </summary>
    public static class DataContainer
    {
        //create dial list object here
        public static Dictionary<int, Data> nodeData = new Dictionary<int, Data>();

        public static List<float> xData = new List<float>();
        public static List<float> yData = new List<float>();

        public static int eventsCaptured;
    }

    public class Data
    {
        //possible time check to prevent bad data on analysis
        public DateTime updateTime;

        public float positionX;
        public float positionY;

        public Dictionary<DateTime, List<byte>> wavData = new Dictionary<DateTime, List<byte>>();//all six channels according to time of write
    }
}