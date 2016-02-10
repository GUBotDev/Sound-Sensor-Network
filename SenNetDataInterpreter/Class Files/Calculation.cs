using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenNetDataInterpreter.Class_Files
{
    class Calculation
    {
        static float avgSensVal1;
        static float avgSensVal2;
        static float avgSensVal3;
        static bool wasTripped = false;
        static float baseLat = 42.127331F;
        static float baseLon = -80.087581F;
        static float earthRad = 6378137;
        //the base Lat Lon is point 0, 0 in meters

        public static double[] convToLatLon(double x, double y)
        {
            double xDeg = arcToDeg(x) + baseLat;
            double yDeg = arcToDeg(y) + baseLon;

            double[] position = new double[2];

            position[0] = xDeg;
            position[1] = yDeg;

            return position;
        }

        private static double arcToDeg(double arcLength)
        {
            double degree = (arcLength / earthRad) * (180 / Math.PI);

            return degree;
        }

        public static void triangulatePosition(List<DateTime> date, List<Int32> node, List<Int32> x, List<Int32> y, List<Int32> senOne, List<Int32> senTwo, List<Int32> senThree, List<Int32> senFour, List<Int32> senFive, List<Int32> senSix)
        {
            ////////////////////////
            /*
            TODO:
            -fix node degree accuracy - values were not returning correctly
            -find base voltages of sensors? average values that did not trip the sensor?
            -
            */
            ////////////////////////

            bool isTripped = false;
            int length = node.ToArray().Length;
            float[] nodeDegrees = new float[length];
            float[] highSens = new float[length];
            float[] secSens = new float[length];
            float[] highSensIndex = new float[length];
            float[] secSensIndex = new float[length];
            float[] data;

            if (length == 1)
            {
                try
                {
                    float degree = determineAngle(senOne[0], senTwo[0], senThree[0], senFour[0], senFive[0], senSix[0]);
                    //Console.WriteLine("Data: " + degree);


                    float[] tempArr = { senOne[0], senTwo[0], senThree[0], senFour[0], senFive[0], senSix[0] };

                    highSens[0] = tempArr.Max();//first highest sensor value

                    if (highSens[0] > 1)
                    {
                        Console.WriteLine("Data: " + degree + " sensor val: " + senOne[0] + " " + senTwo[0] + " " + senThree[0] + " " + senFour[0] + " " + senFive[0] + " " + senSix[0]);
                    }
                }
                catch
                {

                }
            }
            

            if (length >= 3)
            {
                //calculate degree of each node
                for (int i = 0; i < length; i++)
                {
                    float[] tempArr = { senOne[i], senTwo[i], senThree[i], senFour[i], senFive[i], senSix[i] };

                    nodeDegrees[i] = determineAngle(senOne[i], senTwo[i], senThree[i], senFour[i], senFive[i], senSix[i]);
                    highSens[i] = tempArr.Max();//first highest sensor value
                    secSens[i] = (from num in tempArr orderby num descending select num).Skip(1).First();//second highest sensor value

                    highSensIndex[i] = Array.IndexOf(tempArr, highSens[i]);
                    secSensIndex[i] = Array.IndexOf(tempArr, secSens[i]);

                    if (highSens[i] > 35)
                    {
                        isTripped = true;
                    }
                    else
                    {
                        float tempVal = 0;

                        foreach (float element in tempArr)
                        {
                            tempVal += element;
                        }

                        switch (i)
                        {
                            case 0:
                                avgSensVal1 = (avgSensVal1 + tempVal / 6) / 2;
                                break;
                            case 1:
                                avgSensVal2 = (avgSensVal2 + tempVal / 6) / 2;
                                break;
                            case 2:
                                avgSensVal3 = (avgSensVal3 + tempVal / 6) / 2;
                                break;
                        }



                    }
                }

                if (isTripped)
                {
                    data = triangulate(x[0], x[1], x[2], y[0], y[1], y[2], highSens[0], secSens[0], highSens[1], secSens[1], highSens[2], secSens[2], highSensIndex[0], secSensIndex[0], highSensIndex[1], secSensIndex[1], highSensIndex[2], secSensIndex[2], avgSensVal1, avgSensVal2, avgSensVal3);

                    double[] position = new double[2];
                    position = convToLatLon(data[0], data[1]);

                    //just for organizing the console a bit better
                    if (wasTripped)
                    {
                        Console.WriteLine();
                        wasTripped = false;
                    }

                    Console.WriteLine(position[0] + "," + position[1]);
                    PipeReadThread.data.Add(position[0] + "," + position[1]);

                }
                else
                {
                    wasTripped = true;

                    Console.Write("\rNo sensor tripped.");
                }

                /*
                data[0] = x_est;
                data[1] = y_est;
                data[2] = I1_SE;
                data[3] = I1_S;
                data[4] = I2_SE;
                data[5] = I2_S;
                data[6] = I3_SE;
                data[7] = I3_S;
                */


                /* old code
                float[] slope = new float[length];

                for (int i = 0; i < length; i++)
                {
                    slope[i] = calcSlope(nodeDegrees[i], x[i], y[i]);
                }

                float[] interceptX = new float[length * length];
                float[] interceptY = new float[length * length];
                int tempLength = 0;

                for (int i = 0; i < length; i++)
                {
                    for (int j = 0; j < length; j++)
                    {
                        if (i != j)
                        {
                            float _x;
                            float _y;

                            findIntercept(slope[i], slope[j], x[i], y[i], x[j], y[j], out _x, out _y);

                            interceptX[tempLength] = _x;
                            interceptY[tempLength] = _y;

                            if (senOne[i] >= 150 || senTwo[i] >= 150 || senThree[i] >= 150 || senFour[i] >= 150 || senFive[i] >= 150 || senSix[i] >= 150)
                            {
                                //Console.WriteLine(i + " " + nodeDegrees[i]);

                            }

                            tempLength++;
                        }
                    }
                }

                float avgX;
                float avgY;

                avgPoints(interceptX, interceptY, out avgX, out avgY);

                for (int i = 0; i < length; i++)
                {
                    if (senOne[i] >= 150 || senTwo[i] >= 150 || senThree[i] >= 150 || senFour[i] >= 150 || senFive[i] >= 150 || senSix[i] >= 150)
                    {
                        Console.WriteLine(avgX + " " + avgY);
                    }
                }
                */
            }
            else
            {
                //Console.Write("\rConnect more nodes to begin.    ");
            }

        }

        /// <summary>
        /// Pauls' method for sound triangulation
        /// </summary>
        /// <param name="x1">First X coord</param>
        /// <param name="x2"></param>
        /// <param name="x3"></param>
        /// <param name="y1">First Y coord</param>
        /// <param name="y2"></param>
        /// <param name="y3"></param>
        /// <param name="s11">First node, High sens</param>
        /// <param name="s12">first node, low sens</param>
        /// <param name="s21"></param>
        /// <param name="s22"></param>
        /// <param name="s31"></param>
        /// <param name="s32"></param>
        /// <param name="index11">which sensor tripped node 1? (highest val)</param>
        /// <param name="index12">second highest val</param>
        /// <param name="index21"></param>
        /// <param name="index22"></param>
        /// <param name="index31"></param>
        /// <param name="index32"></param>
        /// <param name="v1B">background voltage for node 1</param>
        /// <param name="v2B"></param>
        /// <param name="v3B"></param>
        /// <returns>x/y position, and source intensities</returns>
        private static float[] triangulate(float x1, float x2, float x3, float y1, float y2, float y3, float s11, float s12, float s21, float s22, float s31, float s32, float index11, float index12, float index21, float index22, float index31, float index32, float v1B, float v2B, float v3B)
        {
            //Returns Angles from All Three Sensors in Degrees
            float A_1 = angle(s11, s12, index11, index12);
            float A_2 = angle(s21, s22, index11, index22);
            float A_3 = angle(s31, s32, index31, index32);

            //Returns Recorded Total Voltage from All Three Sensors in dB
            float V1_R = magnitude(s11, s12);
            float V2_R = magnitude(s21, s22);
            float V3_R = magnitude(s31, s32);

            //Triangulation
            //Least Squares Mumbo Jumbo
            //Top and Bottom for Each Sensor
            float a_1 = Convert.ToSingle(-Math.Tan(degToRad(90 - A_1)));
            float a_3 = y1 + x1 * a_1;
            float b_1 = Convert.ToSingle(-Math.Tan(degToRad(90 - A_2)));
            float b_3 = y1 + x2 * b_1;
            float xtop_1 = leastSquares(1, a_1, a_3, 1, b_1, b_3);
            float ytop_1 = leastSquares(a_1, 1, a_3, b_1, 1, b_3);
            float bot_1 = leastSquares(a_1, 1, 1, b_1, 1, 1);

            a_1 = Convert.ToSingle(-Math.Tan(degToRad(90 - A_1)));
            a_3 = y1 + x1 * a_1;
            b_1 = Convert.ToSingle(-Math.Tan(degToRad(90 - A_3)));
            b_3 = y3 + x3 * b_1;

            float xtop_2 = leastSquares(1, a_1, a_3, 1, b_1, b_3);
            float ytop_2 = leastSquares(a_1, 1, a_3, b_1, 1, b_3);
            float bot_2 = leastSquares(a_1, 1, 1, b_1, 1, 1);

            a_1 = Convert.ToSingle(-Math.Tan(degToRad(90 - A_2)));
            a_3 = y2 + x2 * a_1;
            b_1 = Convert.ToSingle(-Math.Tan(degToRad(90 - A_3)));
            b_3 = y3 + x3 * b_1;

            float xtop_3 = leastSquares(1, a_1, a_3, 1, b_1, b_3);
            float ytop_3 = leastSquares(a_1, 1, a_3, b_1, 1, b_3);
            float bot_3 = leastSquares(a_1, 1, 1, b_1, 1, 1);

            float x_est = (xtop_1 + xtop_2 + xtop_3) / (bot_1 + bot_2 + bot_3);
            float y_est = (ytop_1 + ytop_2 + ytop_3) / (bot_1 + bot_2 + bot_3);

            //Finds Distance Between Sensor and Sound Source
            float d1 = Convert.ToSingle(Math.Sqrt(Math.Pow((x_est - x1), 2) + Math.Pow((y_est - y1), 2)));
            float d2 = Convert.ToSingle(Math.Sqrt(Math.Pow((x_est - x2), 2) + Math.Pow((y_est - y2), 2)));
            float d3 = Convert.ToSingle(Math.Sqrt(Math.Pow((x_est - x3), 2) + Math.Pow((y_est - y3), 2)));

            //Returns Intensity As Well As Error Bounds
            float I1_S = sourceIntensity(V1_R, 0, d1);
            float I2_S = sourceIntensity(V2_R, 0, d2);
            float I3_S = sourceIntensity(V3_R, 0, d3);
            float I1_SE = sourceIntensity(V1_R, v1B, d1);
            float I2_SE = sourceIntensity(V2_R, v2B, d2);
            float I3_SE = sourceIntensity(V3_R, v3B, d3);
            
            //stores data that will be returned from eq
            float[] data = new float[8];

            data[0] = x_est;
            data[1] = y_est;
            data[2] = I1_SE;
            data[3] = I1_S;
            data[4] = I2_SE;
            data[5] = I2_S;
            data[6] = I3_SE;
            data[7] = I3_S;

            //return "The (x,y)-coordinates of your sound source are ( " + x_est + ", " + y_est + "). Sensor 1 Gives Source Intensity of Between " + I1_SE + " and " + I1_S + " dB, Sensor 2 Between " + I2_SE + " and " + I2_S + "dB , and Sensor 3 Between " + I3_SE + " and " +  I3_S + " dB.";

            return data;
        }

        private static float leastSquares(float a1, float a2, float a3, float b1, float b2, float b3)
        {
            float ls = Convert.ToSingle((a1 * b2 - a2 * b1) * (a1 * b3 - a3 * b1) / ((Math.Pow(a1, 2) + Math.Pow(a2, 2)) * (Math.Pow(b1, 2) + Math.Pow(b2, 2))));

            return ls;
        }

        private static float magnitude(float s1, float s2)
        {
            float magnitude = Convert.ToSingle(Math.Sqrt(Math.Pow(s1, 2) + s1 * s2 + Math.Pow(s2, 2)));

            return magnitude;
        }

        private static float angle(float s1, float s2, float index1, float index2)
        {
            float az;

            if (((index1 == 0) && (index2 == 5)) || ((index1 == 5) && (index2 == 0)))
            {
                az = 60 * index2 - (1 / 5) * (index1 - index2) * (radToDeg(Convert.ToSingle(Math.Atan((Math.Sqrt(3) * s1) / (2 * s2 + s1)))));
            }
            else
            {
                az = 60 * index2 + (index1 - index2) * (radToDeg(Convert.ToSingle(Math.Atan((Math.Sqrt(3) * s1) / (2 * s2 + s1)))));
            }

            return az;
        }

        private static float sourceIntensity(float vR, float vB, float d)
        {
            float sourceInt = 0;

            if (vR >= vB)
            {
                sourceInt = 20 * Convert.ToSingle(Math.Log10(1609.34 * d * vR / (1 - vB / vR)));          //Conversion to Meters from Miles
            }
            else
            {
                Console.WriteLine("Recorded Level Must be Greater than Background Level");
            }

            return sourceInt;
        }
        
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
            int leftHighSen = 0;
            int rightHighSen = 0;

            for (int i = 0; i < 6; i++)
            {
                if (sensors[i] > highVal)
                {
                    highVal = sensors[i];
                    highSen = i;
                }
            }
            
            if (highSen == 0)
            {
                leftHighSen = 5;
                rightHighSen = 1;
            }
            else if (highSen == 5)
            {
                leftHighSen = 4;
                rightHighSen = 0;
            }
            else
            {
                leftHighSen = highSen - 1;
                rightHighSen = highSen + 1;
            }

            float pi = Convert.ToSingle(Math.PI);
            float sq3 = Convert.ToSingle(Math.Sqrt(3));
            
            float s, s1, s2, t, t1, t2, tF;

            s1 = Convert.ToSingle(Math.Sqrt(sensors[highSen] * sensors[highSen] + sensors[highSen] * sensors[leftHighSen] + sensors[leftHighSen] * sensors[leftHighSen]));//modified sensor 1
            s2 = Convert.ToSingle(Math.Sqrt(sensors[highSen] * sensors[highSen] + sensors[highSen] * sensors[rightHighSen] + sensors[rightHighSen] * sensors[rightHighSen]));//modified sensor 2

            t1 = Convert.ToSingle(Math.Atan((sq3 * sensors[leftHighSen]) / (2 * sensors[highSen] + sensors[leftHighSen])));//base angle 1
            t2 = Convert.ToSingle(Math.Atan((sq3 * sensors[rightHighSen]) / (2 * sensors[highSen] + sensors[rightHighSen])));//base angle 2

            t = Convert.ToSingle(Math.Atan((s2 * Math.Sin(t1 + t2)) / s1 + s2 * Math.Cos(t1 + t2)));//final mid-angle
            s = Convert.ToSingle(Math.Sqrt((s1 + s2 * Math.Cos(t1 + t2)) * (s1 + s2 * Math.Cos(t1 + t2)) + (s2 * Math.Sin(t1 + t2)) * (s2 * Math.Sin(t1 + t2))));//final sensors magnitude

            tF = highSen * 60 + (t1 - t) * (180 / pi);//final angle

            return tF;

            /*
            float arcDegree = secHighSen * 60;

            if (highSen == 0)
            {
                // high sens and left
                //arcDegree = Convert.ToSingle(arcDegree + ((180 / pi) * Math.Atan((sq3 * sensors[highSen]) / ((2 * sensors[secHighSen]) + sensors[highSen]))));
                
                //left

            }
            else if (highSen == 5 && secHighSen == 0)
            {
                // high sens and right
                //arcDegree = Convert.ToSingle(360 - ((180 / pi) * Math.Atan((sq3 * sensors[highSen]) / ((2 * sensors[secHighSen]) + sensors[highSen]))));
                
            }
            else if (highSen < secHighSen)
            {
                //arcDegree = Convert.ToSingle(arcDegree - ((180 / pi) * Math.Atan((sq3 * sensors[highSen]) / ((2 * sensors[secHighSen]) + sensors[highSen]))));
            }
            else
            {
                //arcDegree = Convert.ToSingle(arcDegree + ((180 / pi) * Math.Atan((sq3 * sensors[highSen]) / ((2 * sensors[secHighSen]) + sensors[highSen]))));
            }

            return arcDegree;
            */
        }

        private static float degToRad(float angle)
        {
            return Convert.ToSingle(Math.PI * angle / 180);
        }

        private static float radToDeg(float angle)
        {
            return Convert.ToSingle(angle * (180.0 / Math.PI));
        }
        /////////////////////////////////////////
        ///&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
        ///////////////////////////////////////// previous stand-in for Pauls code
        ///&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
        /////////////////////////////////////////

        private static void avgPoints(float[] interceptX, float[] interceptY, out float xAvg, out float yAvg)
        {
            int i = 0;
            int j = 0;
            xAvg = 0;
            yAvg = 0;

            foreach (float element in interceptX)
            {
                xAvg += element;
                i++;
            }

            foreach (float element in interceptY)
            {
                yAvg += element;
                j++;
            }

            i--;
            j--;

            xAvg /= i;
            yAvg /= j;
        }

        private static void findPoints(int _length, float[] degree, float[] x, float[] y)
        {
            for (int i = 0; i <= _length; i++)
            {
                for (int j = 0; j <= _length; j++)
                {
                    if (i != j)
                    {
                        float pointX;
                        float pointY;

                        float slopeO = calcSlope(degree[i], x[i], y[i]);
                        float slopeT = calcSlope(degree[j], x[j], y[j]);

                        findIntercept(slopeO, slopeT, x[i], y[i], x[j], y[j], out pointX, out pointY);

                        DataContainer.xData.Add(pointX);
                        DataContainer.yData.Add(pointY);

                        Console.WriteLine(pointX + " " + pointY);
                    }//end if
                }//end for
            }//end for
        }//end function

        //uses the event angle - 90 degrees is north, calculates the angle from the line created by the origin in the cartesian coordinate system and the node position to the line created by the node and the event position
        private static float calcSlope(float baseAngle, float x, float y)
        {
            float phi;
            float slope;

            //assuming the origin is x=0, y=0
            phi = 4.712F - Convert.ToSingle(Math.Atan(x / y)) + 6.283F - degToRad(baseAngle);
            slope = radToDeg(Convert.ToSingle(Math.Tan(Math.Atan(y / x) - phi)));

            return slope;
        }

        private static void findIntercept(float slopeO, float slopeT, float xO, float yO, float xT, float yT, out float x, out float y)
        {
            //y-y1 = m(x-x1)
            //y=mx - mx1 + y1
            //m1x - m1x1 + y1 = m2x - m2x2 + y2
            //x = (m1x1 - m2x2 - y1 - y2) / (m1+m2)

            x = (slopeO * xO - slopeT * xT - yO + yT) / (slopeO + slopeT);
            y = slopeO * x - slopeO * xO + yO;
        }

        private static float findDistance(float eventX, float eventY, float X, float Y)
        {
            float sideOne = Math.Abs(Y - eventY);
            float sideTwo = Math.Abs(X - eventX);
            float distance = Convert.ToSingle(Math.Sqrt(Math.Pow(sideOne, 2) + Math.Pow(sideTwo, 2)));

            return distance;
        }

        private static float determineSoundPropagation(float distanceTravelled, float initialSPL)
        {
            float actualSPL = Convert.ToSingle(initialSPL * Math.Pow(distanceTravelled, 2));

            return actualSPL;
        }

        //si units, kpa, c, relative humidity
        private static float calcSpeedOfSound(float pressure, float T, float relHum)
        {
            float pressureSaturation = Convert.ToSingle(6.1078 * Math.Pow(10, ((7.5 * T) / (T + 237.3))));
            float partialPressureVapor = relHum * pressureSaturation;
            float partialPressureAir = pressure - partialPressureVapor;
            float univGasConst = 8.314F;
            float molarMassDryAir = 0.028964F;
            float molarMassVapor = 0.018016F;
            float airDensity = (partialPressureAir * molarMassDryAir + partialPressureVapor * molarMassVapor) / (univGasConst * T);
            float molarMass = (univGasConst * T * airDensity) / pressure;
            float gasConst = univGasConst / molarMass;
            float heatCapDryAir = 1.005F;
            float heatCapVapor = gasConst + heatCapDryAir;
            float heatCapacityRadio = heatCapVapor / heatCapDryAir;

            float speedOfSound = Convert.ToSingle(Math.Sqrt(heatCapacityRadio * univGasConst * T));

            return speedOfSound;
        }


        private static float calcDistTrav(float speedOfSound, float windDegree, float windSpeed, float distance)
        {
            float distanceTravelled = Convert.ToSingle(speedOfSound + windSpeed * Math.Cos(degToRad(windDegree)) + windSpeed * Math.Cos(radToDeg(windDegree)));

            return distanceTravelled;
        }

        /*
        public static void calcWindOffset(float speedOfSound, float averageDist, float windSpeed, float windDegree, float x, float y, out float xCorrected, out float yCorrected)
        {
            float time = speedOfSound / distance;
            float windSlope = calcSlope(windDegree, x, y);


        }
        */
    }
}
