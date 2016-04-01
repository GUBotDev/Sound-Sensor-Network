using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenNetDataInterpreter.Class_Files
{
    /// <summary>
    /// store 1 min worth of bytes here - if triggered store next 30 secs, next convert to wav and send file via tcp
    /// if triggers during the 30 seconds then increase the span by 30 secs
    /// 
    /// </summary>
    public static class DataContainer
    {
        //create dial list object here
        public static Dictionary<int, Data> nodeData = new Dictionary<int, Data>();

        public static List<float> xData = new List<float>();
        public static List<float> yData = new List<float>();


        public static List<int> node01 = new List<int>();
        public static List<int> node02 = new List<int>();
        public static List<int> node03 = new List<int>();
        public static List<int> node04 = new List<int>();
        public static List<int> node05 = new List<int>();
        public static List<int> node06 = new List<int>();

        public static List<int> node11 = new List<int>();
        public static List<int> node12 = new List<int>();
        public static List<int> node13 = new List<int>();
        public static List<int> node14 = new List<int>();
        public static List<int> node15 = new List<int>();
        public static List<int> node16 = new List<int>();

        public static List<int> node21 = new List<int>();
        public static List<int> node22 = new List<int>();
        public static List<int> node23 = new List<int>();
        public static List<int> node24 = new List<int>();
        public static List<int> node25 = new List<int>();
        public static List<int> node26 = new List<int>();

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

    public class Node
    {
        private DateTime date;
        private int iterator = 0;
        private int node = 0;
        private int x = 0;
        private int y = 0;
        private int senOne = 0;
        private int senTwo = 0;
        private int senThr = 0;
        private int senFou = 0;
        private int senFiv = 0;
        private int senSix = 0;
        private bool isActivated = false;

        public DateTime Date
        {
            get { return this.date; }
            set { this.date = value; }
        }
        public int Iterator
        {
            get { return this.iterator; }
            set { this.iterator = value; }
        }
        public int NodeNumber
        {
            get { return this.node; }
            set { this.node = value; }
        }
        public int X
        {
            get { return this.x; }
            set { this.x = value; }
        }
        public int Y
        {
            get { return this.y; }
            set { this.y = value; }
        }
        public int SenOne
        {
            get { return this.senOne; }
            set { this.senOne = value; }
        }
        public int SenTwo
        {
            get { return this.senTwo; }
            set { this.senTwo = value; }
        }
        public int SenThr
        {
            get { return this.senThr; }
            set { this.senThr = value; }
        }
        public int SenFou
        {
            get { return this.senFou; }
            set { this.senFou = value; }
        }
        public int SenFiv
        {
            get { return this.senFiv; }
            set { this.senFiv = value; }
        }
        public int SenSix
        {
            get { return this.senSix; }
            set { this.senSix = value; }
        }
        public bool Activated
        {
            get { return this.isActivated; }
            set { this.isActivated = value; }
        }

    }
}