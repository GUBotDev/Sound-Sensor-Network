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
        static Node[] nodes = new Node[3];

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
                else if (dataSplit[0] == "Event")
                {
                    //request audio for 1 minute interval 30 before 30 after
                    //request sensor data for that time

                    //"Event 1 |-32768 -> 32768| yyyy-MM-dd_HH-mm-ss.ffff"
                    
                    int nodeNum = Convert.ToInt32(dataSplit[1]);
                    int amplitude = Math.Abs(Convert.ToInt32(dataSplit[2]));
                    DateTime dt = DateTime.ParseExact(dataSplit[3], "yyyy-MM-dd_HH-mm-ss.ffff", CultureInfo.InvariantCulture);

                    PipeThread.messages.Add("Request " + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss.ffff") + " " + 60.ToString(), DateTime.Now);
                }
                else if (dataSplit[0] == "Node")
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


                    //create node object once triangulation code can be scaled i.e. assume infinite nodes
                    if (nodeT > 3 || nodeT < 0)
                    {
                        dataIntact = false;
                    }
                    else
                    {
                        nodes[nodeT].Date = dateT;
                        nodes[nodeT].NodeNumber = nodeT;
                        nodes[nodeT].X = xT;
                        nodes[nodeT].Y = yT;
                        nodes[nodeT].SenOne = senOneT;
                        nodes[nodeT].SenTwo = senTwoT;
                        nodes[nodeT].SenThr = senThreeT;
                        nodes[nodeT].SenFou = senFourT;
                        nodes[nodeT].SenFiv = senFiveT;
                        nodes[nodeT].SenSix = senSixT;

                        nodes[nodeT].Iterator++;

                        if (nodes[nodeT].Activated == false)
                        {
                            double[] position = new double[2];
                            string tempString = "Node," + nodeT + "," + position[0] + "," + position[1];
                            Tuple<string, DateTime, bool> tempData = Tuple.Create(tempString, DateTime.Now, false);
                            position = Calculation.convToLatLon(xT, yT);

                            PipeThread.data.Add(tempData);

                            //Console.WriteLine(tempString);

                            nodes[nodeT].Activated = true;
                        }
                    }

                    foreach (Node node in nodes)
                    {
                        if (node.Iterator > 50)
                        {
                            Console.WriteLine("Stagnated Data, check sensor connectivity");
                        }
                    }
                    

                    if (dataIntact && nodes.Any(o => o.Activated == false))
                    {
                        Calculation.triangulatePosition(nodes);

                        foreach (Node node in nodes)
                        {
                            node.Iterator = 0;
                        }
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