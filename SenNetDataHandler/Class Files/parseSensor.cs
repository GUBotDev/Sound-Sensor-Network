using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SenNetDataHandler.Class_Files
{
    class ParseSensor
    {
        //List<List<Int16>> iterationList = new List<List<Int16>>();
        int loop = 0;
        const int threshold = 32;
        byte[] parsedInfo = new byte[5];
        byte[] parsedValues = new byte[6];
        DateTime currentDate;
        static int testThr = 0;

        public DateTime getDateTime()
        {
            return currentDate;
        }

        public void parSenVals(byte[] buffer, DecisionHandler decHand, int node)
        {
            foreach (byte element in buffer)
            {
                if (element == 255)
                {
                    loop = 1;
                }
                else
                {
                    switch (loop)
                    {
                        case 1:
                            parsedInfo[0] = element;
                            break;
                        case 2:
                            parsedInfo[1] = element;
                            break;
                        case 3:
                            parsedInfo[2] = element;
                            break;
                        case 4:
                            parsedInfo[3] = element;
                            break;
                        case 5:
                            parsedInfo[4] = element;
                            break;
                        case 6:
                            parsedValues[0] = element;
                            break;
                        case 7:
                            parsedValues[1] = element;
                            break;
                        case 8:
                            parsedValues[2] = element;
                            break;
                        case 9:
                            parsedValues[3] = element;
                            break;
                        case 10:
                            parsedValues[4] = element;
                            break;
                        case 11:
                            parsedValues[5] = element;
                            break;
                    }

                    if (loop == 11)
                    {
                        currentDate = DateTime.Now;
                        decHand.interpretInput(parsedValues, parsedInfo, node);

                        float test = determineAngle(parsedValues[0], parsedValues[1], parsedValues[2], parsedValues[3], parsedValues[4], parsedValues[5]);

                        testThr++;
                        if (testThr > 75)
                        {
                            //Console.WriteLine((int)test);
                            testThr = 0;
                        }
                    }
                    else
                    {
                        loop++;
                    }


                }//end else
            }//end foreach
        }//end parse function

        //from arduino code
        private static float determineAngle(int senOne, int senTwo, int senThree, int senFour, int senFive, int senSix)
        {
            float[] sensors = new float[6];

            sensors[0] = senOne;// Math.Abs(senOne - 127);
            sensors[1] = senTwo;// Math.Abs(senTwo - 127);
            sensors[2] = senThree;// Math.Abs(senThree - 127);
            sensors[3] = senFour;// Math.Abs(senFour - 127);
            sensors[4] = senFive;// Math.Abs(senFive - 127);
            sensors[5] = senSix;// Math.Abs(senSix - 127);

            float highVal = 0;
            int highSen = 0;
            int secHighSen = 0;

            for (int i = 0; i < 6; i++)
            {
                if (sensors[i] > highVal)
                {
                    highVal = sensors[i];
                    highSen = i;
                }
            }

            highVal = 0;

            for (int i = 0; i < 6; i++)
            {
                if (i != highSen && sensors[i] > highVal)
                {
                    highVal = sensors[i];
                    secHighSen = i;
                }
            }

            const float pi = 3.14159F;
            float degree = highSen * 60;
            float arcDegree = secHighSen * 60;
            float sq3 = Convert.ToSingle(Math.Sqrt(3));

            if (highSen == 0 && secHighSen == 5)
            {
                degree = 360 - ((sensors[secHighSen] / sensors[highSen]) * 30);
                arcDegree = Convert.ToSingle(arcDegree + ((180 / pi) * Math.Atan((sq3 * sensors[highSen]) / ((2 * sensors[secHighSen]) + sensors[highSen]))));
            }
            else if (highSen == 5 && secHighSen == 0)
            {
                degree = 360 - ((sensors[secHighSen] / sensors[highSen]) * 30);
                arcDegree = Convert.ToSingle(360 - ((180 / pi) * Math.Atan((sq3 * sensors[highSen]) / ((2 * sensors[secHighSen]) + sensors[highSen]))));
            }
            else if (highSen < secHighSen)
            {
                degree = degree + ((sensors[secHighSen] / sensors[highSen]) * 30);
                arcDegree = Convert.ToSingle(arcDegree - ((180 / pi) * Math.Atan((sq3 * sensors[highSen]) / ((2 * sensors[secHighSen]) + sensors[highSen]))));
            }
            else
            {
                degree = degree - ((sensors[secHighSen] / sensors[highSen]) * 30);
                arcDegree = Convert.ToSingle(arcDegree + ((180 / pi) * Math.Atan((sq3 * sensors[highSen]) / ((2 * sensors[secHighSen]) + sensors[highSen]))));
            }

            /*
            if (sensors[secHighSen] / sensors[highSen] < 0.5f)
            {
                Console.WriteLine(sensors[secHighSen] / sensors[highSen]);
            }

            //Console.WriteLine(sensors[secHighSen] / sensors[highSen]);
            
            if (sensors[highSen] >= 50)
            {
                Console.WriteLine(sensors[secHighSen] / sensors[highSen] * 30);
            }
            
            Serial.println(highSen);
            Serial.println(sensors[highSen]);
            Serial.println(secHighSen);
            Serial.println(sensors[secHighSen]);
            Serial.println(degree);
            Serial.println(arcDegree);

            //Console.WriteLine(degree + " " + (sensors[secHighSen] / sensors[highSen]) + " " + sensors[secHighSen] + " " + sensors[highSen]);
            */

            return arcDegree;
        }
    }
}
