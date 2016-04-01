using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET.MapProviders;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using NAudio.Wave;
using System.IO;
using System.Threading;

namespace SensorNetworkInterface.Class_Files
{
    public partial class MainForm : Form
    {
        public GMapOverlay markersOverlay;//default overlay
        PointLatLng main = new PointLatLng(42.127331, -80.087581);//central starting point of map
        int markersPlaced = 0;
        Bitmap bmp;//default bitmap
        System.Media.SoundPlayer player;//default sound player
        bool wasOn = false;
        bool isConnected = false;
        Thread serverMain;
        Thread serverFile;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            gMap.MapProvider = GoogleMapProvider.Instance;
            gMap.Manager.Mode = AccessMode.ServerAndCache;
            GMapProvider.WebProxy = null;

            gMap.MinZoom = 1;
            gMap.MaxZoom = 20;
            gMap.Zoom = 18;
            gMap.Position = main;

            /*
            if (listenerThread.IsBusy == false)
            {
                listenerThread.RunWorkerAsync();
            }

            if (fileReadThread.IsBusy == false)
            {
                fileReadThread.RunWorkerAsync();
            }
            */

            if (!Directory.Exists("/audio"))
            {
                Directory.CreateDirectory("/audio");
            }
            else
            {
                string[] temp = Directory.GetFiles("audio/", "*.wav").Select(fileName => Path.GetFileNameWithoutExtension(fileName)).ToArray();

                foreach (string input in temp)
                {
                    audioList.Items.Add(input);
                }
            }
        }

        /// <summary>
        /// Delegate callback for addNode function
        /// </summary>
        delegate void addNodeCallback(int nodeNum, double x, double y, GMapOverlay markersOverlay);
        
        /// <summary>
        /// Adds a sound sensor node to the map.
        /// </summary>
        /// <param name="nodeNum">The unique node id number.</param>
        /// <param name="x">The x coordinate of the node.</param>
        /// <param name="y">The y coordinate of the node.</param>
        /// <param name="markersOverlay">The marker overlay that the node is added to.</param>
        public void addNode(int nodeNum, double x, double y, GMapOverlay markersOverlay)
        {
            if (this.gMap.InvokeRequired)
            {
                addNodeCallback a = new addNodeCallback(addNode);
                this.Invoke(a, new object[] { nodeNum, x, y, markersOverlay });
            }
            else
            {
                PointLatLng point = new PointLatLng(x, y);

                GMarkerGoogle markerNode = new GMarkerGoogle(point, GMarkerGoogleType.green);

                markersOverlay.Markers.Add(markerNode);

                markerNode.ToolTipMode = MarkerTooltipMode.Always;
                markerNode.ToolTip = new GMapToolTip(markerNode);
                markerNode.ToolTipText = "Node " + nodeNum;

                gMap.Position = point;
                gMap.Overlays.Add(markersOverlay);
            }
        }
        
        /// <summary>
        /// Adds a marker to the map display.
        /// </summary>
        /// <param name="marker">The actual marker that will be added.</param>
        /// <param name="marOver">The map overlay that the marker is added to.</param>
        /// <param name="name">The name of the marker.</param>
        public void addMarker(GMarkerGoogle marker, GMapOverlay marOver, string name)
        {
            try
            {
                if (this.gMap.InvokeRequired)
                {
                    this.BeginInvoke(new Action<GMarkerGoogle, GMapOverlay, string>(addMarker), marker, marOver, name);
                }
                else
                {
                    System.Media.SystemSounds.Beep.Play();

                    markersPlaced++;
                    gMap.Position = marker.Position;
                    gMap.Overlays.Add(marOver);
                }
            }
            catch (Exception ex)
            {
                printConsole(ex.Message);
            }
        }

        /// <summary>
        /// Removes a map overlay.
        /// </summary>
        /// <param name="marOver">The map overlay to be removed.</param>
        public void removeMarkerOverlay(GMapOverlay marOver)
        {
            BeginInvoke((MethodInvoker)delegate
            {
                gMap.Overlays.Remove(marOver);
            });
        }

        /// <summary>
        /// Adds wave file to list box.
        /// </summary>
        /// <param name="name">Name of wave file without file extension.</param>
        public void addWavToListBox(string name)
        {
            BeginInvoke((MethodInvoker)delegate
            {
                audioList.Items.Add(name);
            });
        }

        /// <summary>
        /// Adds wave file to viewer for visualization.
        /// </summary>
        /// <param name="data">Wave file data.</param>
        /// <param name="samplesPerPixel">Samples allocated to each vertical column of pixels.</param>
        public void addWavToViewer(byte[] data, int samplesPerPixel)
        {
            BeginInvoke((MethodInvoker)delegate
            {
                waveViewer1.SamplesPerPixel = samplesPerPixel;

                WaveFormat waveFormat = new WaveFormat(44100, 16, 1);
                Stream memStream = new MemoryStream(data);
                WaveStream waveStream = new WaveStreamDer(memStream, waveFormat);

                waveViewer1.WaveStream = waveStream;
            });
        }

        /// <summary>
        /// Adds a bitmap image to the spectrogram display.
        /// </summary>
        /// <param name="_bmp">Bitmap image.</param>
        public void addBmpSpectrogram(Bitmap _bmp)
        {
            try
            {
                bmp = _bmp;

                if (spectrogram.InvokeRequired)
                {
                    spectrogram.Invoke(new MethodInvoker(
                        delegate ()
                        {
                            spectrogram.ImageLocation = "test.png";
                        }
                    ));
                }
                else
                {
                    spectrogram.ImageLocation = "test.png";
                }
            }
            catch (Exception ex)
            {
                printConsole("BMP: " + ex.Message);
            }
        }

        /// <summary>
        /// Requires invocation, returns x, y size of wave viewer.
        /// </summary>
        public void getWaveViewerSize(out int x, out int y)
        {
            x = waveViewer1.Width;
            y = waveViewer1.Height;
        }

        /// <summary>
        /// Requires invocation, returns x, y size of spectrogram.
        /// </summary>
        public void getSpectrogramSize(out int x, out int y)
        {
            x = spectrogram.Width;
            y = spectrogram.Height;
        }

        private void MainForm_ResizeEnd(Object sender, EventArgs e)
        {
            if (wasOn)
            {
                var thread = new Thread(
                () => {
                     MessageBox.Show("Resizing audio waveforms, please wait... ");
                });

                thread.Start();

                try
                {
                    Visualize.displayWave("audio/" + audioList.SelectedItem.ToString() + ".wav");
                }
                catch
                {
                    audioList.SelectedIndex = 0;

                    Visualize.displayWave("audio/" + audioList.SelectedItem.ToString() + ".wav");
                }

                thread.Abort();
            }
        }

        /// <summary>
        /// Displays audio file in spectrogram and wave viewer.
        /// </summary>
        private void displayFile_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Displaying, please wait.");

            var thread = new Thread(
            () => {
                 MessageBox.Show("Displaying audio, please wait...");
            });

            thread.Start();

            try
            {
                Visualize.displayWave("audio/" + audioList.SelectedItem.ToString() + ".wav");
            }
            catch
            {
                audioList.SelectedIndex = 0;

                Visualize.displayWave("audio/" + audioList.SelectedItem.ToString() + ".wav");
            }
            
            thread.Abort();
            wasOn = true;
        }

        /// <summary>
        /// Plays the wave file.
        /// </summary>
        private void playFile_Click(object sender, EventArgs e)
        {
            printConsole("Playing");

            try
            {
                player = new System.Media.SoundPlayer("audio/" + audioList.SelectedItem.ToString() + ".wav");
                player.Play();
            }
            catch
            {
                audioList.SelectedIndex = 0;

                player = new System.Media.SoundPlayer("audio/" + audioList.SelectedItem.ToString() + ".wav");
                player.Play();
            }
        }

        /// <summary>
        /// Stops the playback of the wave file.
        /// </summary>
        private void stopFile_Click(object sender, EventArgs e)
        {
            printConsole("Stopping");

            player.Stop();
        }
 
        /// <summary>
        /// Adds wave file to list box.
        /// </summary>
        /// <param name="name">Name of wave file without file extension.</param>
        public void printConsole(string input)
        {
            BeginInvoke((MethodInvoker)delegate
            {
                consoleRTBox.AppendText(input + "\n");
                consoleRTBox.ScrollToCaret();
            });
        }

        private void flagButton_Click(object sender, EventArgs e)
        {
            audioList.SelectedItem.ToString();

            //send email
            //save file, name is date + coordinates
            //get longer audio file from server?
            //
        }

        public string returnIP()
        {
            if (ipAddress.InvokeRequired)
            {
                return (string)ipAddress.Invoke(new Func<string>(returnIP));
            }
            else
            {
                return ipAddress.Text;
            }
        }

        private void setIPButton_Click(object sender, EventArgs e)
        {
            //restart connection
            try
            {
                if (isConnected)
                {
                    var messageThread = new Thread(
                    () => {
                        MessageBox.Show("Restarting server connections, please wait...");
                    });

                    var restartingThread = new Thread(
                    () => {
                        try
                        {
                            printConsole("Closing connections");

                            Connection._continue = false;

                            Connection.tcpFile.GetStream().Close();
                            Connection.tcpRead.GetStream().Close();
                            Connection.tcpFile.Close();
                            Connection.tcpRead.Close();


                            printConsole("Aborting threads");

                            serverMain.Join();
                            serverFile.Join();

                            printConsole("Restarting threads");

                            serverMain = new Thread(() => Connection.connThread());
                            serverFile = new Thread(() => Connection.fileThread());

                            Connection._continue = true;

                            serverMain.Start();
                            serverFile.Start();

                            printConsole("Threads should start");
                        }
                        catch { }
                    });

                    messageThread.Start();
                    restartingThread.Start();

                    restartingThread.Join();
                    messageThread.Abort();
                }
                else
                {
                    //ip is returned every time the the threads are restarted
                }

            }
            catch (Exception ex) { printConsole("Close Button: " + ex.Message); }

        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //stop connection
            try
            {
                if (isConnected)
                {
                    var messageThread = new Thread(
                    () => {
                        MessageBox.Show("Closing server connections, please wait...");
                    });

                    var closingThread = new Thread(
                    () => {
                        try
                        {
                            printConsole("Closing connections");

                            Connection._continue = false;

                            Connection.tcpFile.GetStream().Close();
                            Connection.tcpRead.GetStream().Close();
                            Connection.tcpFile.Close();
                            Connection.tcpRead.Close();

                            printConsole("Aborting threads");

                            serverMain.Join();
                            serverFile.Join();
                        }
                        catch { }
                    });

                    messageThread.Start();
                    closingThread.Start();

                    closingThread.Join();
                    messageThread.Abort();

                    isConnected = false;
                }
            }
            catch { }
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            serverMain = new Thread(() => Connection.connThread());
            serverFile = new Thread(() => Connection.fileThread());

            serverMain.Start();
            serverFile.Start();

            isConnected = true;
        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {
            //stop connection
            try
            {
                if (isConnected)
                {
                    var messageThread = new Thread(
                    () => {
                        MessageBox.Show("Closing server connections, please wait...");
                    });

                    var closingThread = new Thread(
                    () => {
                        try
                        {
                            printConsole("Closing connections");

                            Connection._continue = false;

                            Connection.tcpFile.GetStream().Close();
                            Connection.tcpRead.GetStream().Close();
                            Connection.tcpFile.Close();
                            Connection.tcpRead.Close();

                            printConsole("Aborting threads");

                            serverMain.Join();
                            serverFile.Join();
                        }
                        catch { }
                    });

                    messageThread.Start();
                    closingThread.Start();

                    closingThread.Join();
                    messageThread.Abort();

                    isConnected = false;
                }
            }
            catch { }
        }
    }
}
