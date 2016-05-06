using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorNetworkInterface.Class_Files
{
    //Thanks paul
    class Fourier
    {
        static int numOfTimeEntries = 7; //Number of Time Values
        static int numOfFreqEntries = 7; //Number of Frequency Values Under Consideration
        static double mathy = 2 * Math.PI / numOfFreqEntries;

        public static double[] fourier(double[][] entries, int timeNum, int freqNum)
        {
            double[] amplitude = new double[2];

            amplitude[0] = 0;
            amplitude[1] = 0;

            for (int i = 0; i < numOfTimeEntries; i++)
            {
                amplitude[0] += (entries[i][0] * Math.Cos(mathy * freqNum * i) + entries[i][1] * Math.Sin(mathy * freqNum * i)) * window(i);
                amplitude[1] += (entries[i][1] * Math.Cos(mathy * freqNum * i) - entries[i][0] * Math.Sin(mathy * freqNum * i)) * window(i);
            }

            return amplitude;
        }

        public static double inverseFourier(double[][] amplitude, int timeNum, bool isReal)
        {
            double entries = 0;

            for (int i = 0; i < numOfFreqEntries; i++)
            {
                if (isReal)
                {
                    entries += (amplitude[i][0] * Math.Cos(mathy * timeNum * i) - amplitude[i][1] * Math.Sin(mathy * timeNum * i));
                }
                else
                {
                    entries += (amplitude[i][1] * Math.Cos(mathy * timeNum * i) + amplitude[i][0] * Math.Sin(mathy * timeNum * i));
                }
                //entries /= (window(timeNum - i) * 1f * numOfFreqEntries);
            }

            return entries;
        }

        public static double spectrogram(double[] amplitude)
        {
            double strength = Math.Sqrt(amplitude[0] * amplitude[0] + amplitude[1] * amplitude[1]);

            //Can Check to See if "strength" is Less Than Some Cutoff Value... If So, Set Equal to Zero

            return strength;
        }

        public static double window(int num)
        {
            //Used to Localize Signal
            double signal = (.5) * (1 - Math.Cos(2 * Math.PI * num / (numOfTimeEntries - 1)));

            if (signal == 0)
            {
                return .0000001;
            }

            return signal;
        }

        public static double[][] getPixels(short[] data, int x, int y)
        {
            numOfTimeEntries = x;
            numOfFreqEntries = y;

            double[][] entries = new double[data.Length + 1][]; //Entries of Signal
            double[][][] amplitude = new double[numOfTimeEntries][][]; //Signal Intensity Stuff
            double[][] intensity = new double[numOfTimeEntries][];
            double[][] endSignal = new double[data.Length + 1][];
            
            for (int i = 0; i < entries.Length; i++)
            {
                entries[i] = new double[2];
            }

            for (int i = 0; i < amplitude.Length; i++)
            {
                amplitude[i] = new double[numOfFreqEntries][];

                for (int j = 0; j < amplitude[i].Length; j++)
                {
                    amplitude[i][j] = new double[2];
                }
            }

            for (int i = 0; i < intensity.Length; i++)
            {
                intensity[i] = new double[numOfFreqEntries];
            }

            for (int i = 0; i < entries.Length; i++)
            {
                endSignal[i] = new double[2];
            }

            Console.WriteLine("inited " + entries.Length + " " + data.Length + " " + endSignal.Length);
            
            for (int i = 0; i < data.Length; i++)
            {
                entries[i][0] = Math.Abs(data[i] / 32768.0) * Math.Abs(data[i] / 32768.0); //Frequency Amplitude of Entry Signal
                entries[i][1] = Math.Abs(data[i] / 32768.0); //Time Step of Entry Signal
                endSignal[i][0] = 0;
                endSignal[i][1] = 0;
            }

            /*
            for (int i = 0; i < numOfTimeEntries; i++) {
                entries[i][0] = i * i; //Frequency Amplitude of Entry Signal
                entries[i][1] = i; //Time Step of Entry Signal
                endSignal[i][0] = 0;
                endSignal[i][1] = 0;
            }
            */

            for (int i = 0; i < numOfTimeEntries; i++)
            {
                for (int j = 0; j < numOfFreqEntries; j++)
                {
                    amplitude[i][j] = fourier(entries, i, j);
                    intensity[i][j] = spectrogram(amplitude[i][j]);
                    /*if(intensity[i][j] < 0) {
                        amplitude[i][j] = new double[]{0,0};
                    }*/
                    //System.out.println(i + " " + j + " " + intensity[i][j] + "\n");
                }
                //endSignal[i][0] += inverseFourier(amplitude[i], i, true) / (window(i) * numOfFreqEntries);
                //endSignal[i][1] += inverseFourier(amplitude[i], i, false) / (window(i) * numOfFreqEntries);
                //System.out.println(i + " " + endSignal[i][0] + " " + endSignal[i][1] + "\n\n");
                Console.WriteLine(i);
            }

            return intensity;
        }
    }
}