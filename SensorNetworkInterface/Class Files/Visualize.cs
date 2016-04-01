using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using NAudio.Wave;
using System.IO;
using System.Windows.Forms;

namespace SensorNetworkInterface.Class_Files
{
    static class Visualize
    {
        public static void displayWave(string address)
        {
            try {
                int x = 0, y = 0, xWave = 0, yWave = 0;
                int samples = 0;
                int samplesPerPixel = 0;

                Program.mainForm.Invoke(new Action(() =>
                {
                    Program.mainForm.getWaveViewerSize(out xWave, out yWave);
                    Program.mainForm.getSpectrogramSize(out x, out y);
                }
                ));

                Bitmap bmp = new Bitmap(x, y);
                List<double> ratioList = new List<double>();

                using (WaveFileReader reader = new WaveFileReader(address))
                {
                    if (reader.WaveFormat.BitsPerSample == 16)
                    {
                        byte[] buffer = new byte[reader.Length];
                        int read = reader.Read(buffer, 0, buffer.Length);
                        short[] sampleBuffer = new short[read / 2];
                        Buffer.BlockCopy(buffer, 0, sampleBuffer, 0, read);

                        double tempD = 0.0;

                        foreach (short k in sampleBuffer)
                        {
                            tempD = Math.Abs((double)k / 32768.0);

                            ratioList.Add(tempD);

                            samples++;
                        }

                        samplesPerPixel = samples / x;

                        double tempYOff = 44100 / y;
                        int initSamp = 0;

                        for (int xPos = 0; xPos < x; xPos++)
                        {
                            List<double> xSamples = new List<double>();

                            for (int i = initSamp; i < samplesPerPixel + initSamp; i++)
                            {
                                xSamples.Add(ratioList.ElementAt(i));
                            }

                            //iterate through all pixels
                            for (int yPos = 0; yPos < y; yPos++)
                            {
                                byte tempB = (byte)(xSamples.ElementAt(yPos) * y);

                                bmp.SetPixel(xPos, yPos, getColor(tempB));//set pixel at y height
                            }

                            initSamp += samplesPerPixel;
                        }
                        
                        Program.mainForm.Invoke(new Action(() =>
                        {
                            samplesPerPixel = samples / xWave;

                            Program.mainForm.addWavToViewer(buffer, samplesPerPixel);
                        }
                        ));

                        Program.mainForm.Invoke(new Action(() =>
                        {
                            Program.mainForm.addBmpSpectrogram(bmp);
                        }
                        ));

                        Program.mainForm.printConsole("Sample displayed");
                    }
                    else
                    {
                        Program.mainForm.printConsole("Only works with 16 bit audio");
                    }
                }

                bmp.Save("test.png", ImageFormat.Png);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Visualize: " + ex);
            }
        }

        private static Color getColor(byte intensity)
        {
            intensity = (byte)(Math.Abs(intensity - 128) * 2);

            double r = 0, g = 0, b = 0;
            double intensityMod = Math.Pow(intensity / 255.0, 3);
            //double intensityMod = (intensity / 255.0);

            r = intensity;
            g = (intensity / 2);
            b = 255 - intensity;

            r = r * intensityMod;
            g = g * intensityMod;
            b = b * intensityMod;

            //Program.mainForm.printConsole(intensity + " " + (intensity / 255.0) + " " + b);

            if (r > 255) { r = 255; }
            if (g > 255) { g = 255; }
            if (b > 255) { b = 255; }

            if (r < 0) { r = 0; }
            if (g < 0) { g = 0; }
            if (b < 0) { b = 0; }

            byte clip = (intensityMod * 255.0) - 20 > 0 ? (byte)((1 - intensityMod) * 255.0) : (byte)0;

            Color color = Color.FromArgb(255, (int)r, (int)g, (int)b);

            return color;
        }

        public static void convertToWave(byte[] waveData)
        {
            short[] sampleBuffer = new short[waveData.Length / 2];
            Buffer.BlockCopy(waveData, 0, sampleBuffer, 0, waveData.Length);

            DateTime dt = DateTime.Now;
            string file = dt.ToString("hh:mm:ss:ffff");//Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".wav");
            WaveFormat waveFormat = new WaveFormat(44100, 16, 1);
            Stream memStream = new MemoryStream(waveData);
            WaveStream waveStream = new WaveStreamDer(memStream, waveFormat);

            Program.mainForm.Invoke(new Action(() =>
            {
                Program.mainForm.addWavToListBox(file);
            }
            ));

            file += ".wav";

            WaveFileWriter.CreateWaveFile(file, waveStream);
        }
    }

    /// <summary>
    /// Used to convert 16 bit data to wave files
    /// </summary>
    public class WaveStreamDer : WaveStream
    {
        private Stream sourceStream;
        private WaveFormat waveFormat;

        public WaveStreamDer(Stream sourceStream, WaveFormat waveFormat)
        {
            this.sourceStream = sourceStream;
            this.waveFormat = waveFormat;
        }

        public override WaveFormat WaveFormat
        {
            get { return this.waveFormat; }
        }

        public override long Length
        {
            get { return this.sourceStream.Length; }
        }

        public override long Position
        {
            get
            {
                return this.sourceStream.Position;
            }
            set
            {
                this.sourceStream.Position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return sourceStream.Read(buffer, offset, count);
        }
    }
}
