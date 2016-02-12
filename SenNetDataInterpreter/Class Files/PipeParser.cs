using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace SenNetDataInterpreter.Class_Files
{
    static class PipeParser
    {
        static bool sen1 = false;
        static bool sen2 = false;
        static bool sen3 = false;
        static int senI1 = 0;
        static int senI2 = 0;
        static int senI3 = 0;
        /*
        static List<DateTime> date = new List<DateTime>();
        static List<Int32> node = new List<Int32>();
        static List<Int32> x = new List<Int32>();
        static List<Int32> y = new List<Int32>();
        static List<Int32> senOne = new List<Int32>();
        static List<Int32> senTwo = new List<Int32>();
        static List<Int32> senThree = new List<Int32>();
        static List<Int32> senFour = new List<Int32>();
        static List<Int32> senFive = new List<Int32>();
        static List<Int32> senSix = new List<Int32>();
        */

        static DateTime[] date = new DateTime[3];
        static Int32[] node = new Int32[3];
        static Int32[] x = new Int32[3];
        static Int32[] y = new Int32[3];
        static Int32[] senOne = new Int32[3];
        static Int32[] senTwo = new Int32[3];
        static Int32[] senThree = new Int32[3];
        static Int32[] senFour = new Int32[3];
        static Int32[] senFive = new Int32[3];
        static Int32[] senSix = new Int32[3];

        public static bool node1 = false;
        public static bool node2 = false;
        public static bool node3 = false;

        //split string first via ':', then split via space
        public static void parsePipeString(string _readLine)
        {
            bool dataIntact = true;
            string[] dataSplit = _readLine.Split(' ');

            try
            {
                if (dataSplit.Length != 10)
                {
                    //Console.WriteLine("Error.");
                }
                else
                {
                    DateTime dateT;
                    int nodeT, xT, yT, senOneT, senTwoT, senThreeT, senFourT, senFiveT, senSixT;

                    dateT = new DateTime();
                    nodeT = -1;
                    senOneT = -1;
                    senTwoT = -1;
                    senThreeT = -1;
                    senFourT = -1;
                    senFiveT = -1;
                    senSixT = -1;

                    DateTime.TryParseExact(dataSplit[0], "yyyy-MM-dd_HH-mm-ss.ffff", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateT);
                    Int32.TryParse(dataSplit[1], out nodeT);
                    Int32.TryParse(dataSplit[2], out xT);
                    Int32.TryParse(dataSplit[3], out yT);
                    Int32.TryParse(dataSplit[4], out senOneT);
                    Int32.TryParse(dataSplit[5], out senTwoT);
                    Int32.TryParse(dataSplit[6], out senThreeT);
                    Int32.TryParse(dataSplit[7], out senFourT);
                    Int32.TryParse(dataSplit[8], out senFiveT);
                    Int32.TryParse(dataSplit[9], out senSixT);

                    //Console.WriteLine(dataSplit[0] + " " + dataSplit[1] + " " + dataSplit[2] + " " + dataSplit[3] + " " + dataSplit[4]);
                    //Console.WriteLine(dateT + "  " + nodeT + " " + xT + " " + yT + " " + senOneT + " " + senTwoT + " " + senThreeT + " " + senFourT + " " + senFiveT + " " + senSixT);

                    if (dateT == new DateTime()) { dataIntact = false; };
                    if (nodeT == -1) { dataIntact = false; };
                    if (xT == 0) { dataIntact = false; };
                    if (yT == 0) { dataIntact = false; };
                    if (senOneT == -1) { dataIntact = false; };
                    if (senTwoT == -1) { dataIntact = false; };
                    if (senThreeT == -1) { dataIntact = false; };
                    if (senFourT == -1) { dataIntact = false; };
                    if (senFiveT == -1) { dataIntact = false; };
                    if (senSixT == -1) { dataIntact = false; };

                    if (nodeT == 0)
                    {
                        date[0] = dateT;
                        node[0] = nodeT;
                        x[0] = xT;
                        y[0] = yT;
                        senOne[0] = senOneT;
                        senTwo[0] = senTwoT;
                        senThree[0] = senThreeT;
                        senFour[0] = senFourT;
                        senFive[0] = senFiveT;
                        senSix[0] = senSixT;

                        sen1 = true;
                        senI1++;

                        if (node1 == false)
                        {
                            PipeReadThread.data.Add("Node," + nodeT + "," + xT + "," + yT);
                            node1 = true;
                        }
                    }
                    else if (nodeT == 1)
                    {
                        date[1] = dateT;
                        node[1] = nodeT;
                        x[1] = xT;
                        y[1] = yT;
                        senOne[1] = senOneT;
                        senTwo[1] = senTwoT;
                        senThree[1] = senThreeT;
                        senFour[1] = senFourT;
                        senFive[1] = senFiveT;
                        senSix[1] = senSixT;

                        sen2 = true;
                        senI2++;

                        if (node2 == false)
                        {
                            PipeReadThread.data.Add("Node," + nodeT + "," + xT + "," + yT);
                            node2 = true;
                        }
                    }
                    else if (nodeT == 2)
                    {
                        date[2] = dateT;
                        node[2] = nodeT;
                        x[2] = xT;
                        y[2] = yT;
                        senOne[2] = senOneT;
                        senTwo[2] = senTwoT;
                        senThree[2] = senThreeT;
                        senFour[2] = senFourT;
                        senFive[2] = senFiveT;
                        senSix[2] = senSixT;

                        sen3 = true;
                        senI3++;

                        if (node3 == false)
                        {
                            PipeReadThread.data.Add("Node," + nodeT + "," + xT + "," + yT);
                            node3 = true;
                        }
                    }
                    else
                    {
                        dataIntact = false;
                    }

                    if (senI1 > 50 || senI2 > 50 || senI3 > 50)
                    {
                        //Console.WriteLine("Stagnated Data, check sensor connectivity");
                    }

                    if (dataIntact && sen1 && sen2 && sen3)
                    {
                        Calculation.triangulatePosition(date.ToList(), node.ToList(), x.ToList(), y.ToList(), senOne.ToList(), senTwo.ToList(), senThree.ToList(), senFour.ToList(), senFive.ToList(), senSix.ToList());

                        senI1 = 0;
                        senI2 = 0;
                        senI3 = 0;
                    }
                    else
                    {
                        //PipeReadThread.data.Add("Data not intact.");
                    }
                    //read backwards, take only one value,kill rest
                    /*
                    if (senI1 > 0 && senI2 > 0 && senI3 > 0)
                    {
                        if (senI1 > 1)
                        {
                            int pres = 0;
                            for (int i = node.ToArray().Length; i > 0; i--)
                            {
                                if (node[i - 1] == 0)
                                {
                                    if (pres == 0)
                                    {
                                        pres++;//increment only
                                    }
                                    else
                                    {
                                        date.RemoveAt(i);
                                        node.RemoveAt(i);
                                        x.RemoveAt(i);
                                        y.RemoveAt(i);
                                        senOne.RemoveAt(i);
                                        senTwo.RemoveAt(i);
                                        senThree.RemoveAt(i);
                                        senFour.RemoveAt(i);
                                        senFive.RemoveAt(i);
                                        senSix.RemoveAt(i);
                                    }
                                }
                            }
                        }

                        if (senI2 > 1)
                        {
                            int pres = 0;
                            for (int i = node.ToArray().Length; i > 0; i--)
                            {
                                if (node[i - 1] == 1)
                                {
                                    if (pres == 0)
                                    {
                                        pres++;//increment only
                                    }
                                    else
                                    {
                                        date.RemoveAt(i);
                                        node.RemoveAt(i);
                                        x.RemoveAt(i);
                                        y.RemoveAt(i);
                                        senOne.RemoveAt(i);
                                        senTwo.RemoveAt(i);
                                        senThree.RemoveAt(i);
                                        senFour.RemoveAt(i);
                                        senFive.RemoveAt(i);
                                        senSix.RemoveAt(i);
                                    }
                                }
                            }
                        }

                        if (senI3 > 1)
                        {
                            int pres = 0;
                            for (int i = node.ToArray().Length; i > 0; i--)
                            {
                                if (node[i - 1] == 2)
                                {
                                    if (pres == 0)
                                    {
                                        pres++;//increment only
                                    }
                                    else
                                    {
                                        date.RemoveAt(i);
                                        node.RemoveAt(i);
                                        x.RemoveAt(i);
                                        y.RemoveAt(i);
                                        senOne.RemoveAt(i);
                                        senTwo.RemoveAt(i);
                                        senThree.RemoveAt(i);
                                        senFour.RemoveAt(i);
                                        senFive.RemoveAt(i);
                                        senSix.RemoveAt(i);
                                    }
                                }
                            }
                        }

                        Console.WriteLine(node.ToArray().Length);

                        Calculation.triangulatePosition(date, node, x, y, senOne, senTwo, senThree, senFour, senFive, senSix);

                        date.Clear();
                        node.Clear();
                        x.Clear();
                        y.Clear();
                        senOne.Clear();
                        senTwo.Clear();
                        senThree.Clear();
                        senFour.Clear();
                        senFive.Clear();
                        senSix.Clear();

                        sen1 = false;
                        sen2 = false;
                        sen3 = false;

                        senI1 = 0;
                        senI2 = 0;
                        senI3 = 0;
                    }
                    */
                }
            }
            catch// (Exception exception)
            {
                //Console.WriteLine("PipeParser: " + exception);
            }

        }
    }
}