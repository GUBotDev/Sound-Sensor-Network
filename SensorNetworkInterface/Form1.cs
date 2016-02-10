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

namespace SensorNetworkInterface.Class_Files
{
    public partial class Form1 : Form
    {
        public GMapOverlay markersOverlay;
        PointLatLng main = new PointLatLng(42.127331, -80.087581);
        int markersPlaced = 0;

        public Form1()
        {
            InitializeComponent();
            this.Text = "Sensor Network Interface";
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            gMap.MapProvider = GoogleMapProvider.Instance;
            gMap.Manager.Mode = AccessMode.ServerAndCache;
            GMapProvider.WebProxy = null;
            
            //gMap.Position = new PointLatLng(42.127331, -80.087581);
            
            gMap.MinZoom = 1;
            gMap.MaxZoom = 20;
            gMap.Zoom = 18;
            gMap.Position = main;


            if (listenerThread.IsBusy != true)
            {
                // Start the asynchronous operation.
                listenerThread.RunWorkerAsync();
            }
        }

        delegate void addNodeCallback(int nodeNum, double x, double y, GMapOverlay markersOverlay);


        public void addNode(int nodeNum, double x, double y, GMapOverlay markersOverlay)
        {
            /*
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
                //gMap.Position = main;
            }
            */
        }

        //delegate void addMarkerCallback(GMarkerGoogle marker, GMapOverlay marOver, string name);

        public void addMarker(GMarkerGoogle marker, GMapOverlay marOver, string name)
        {
            try
            {
                if (this.gMap.InvokeRequired)
                {
                    //addMarkerCallback a = new addMarkerCallback(addMarker);
                    //this.Invoke(a, new object[] { marker, marOver, name });
                    this.BeginInvoke(new Action<GMarkerGoogle, GMapOverlay, string>(addMarker), marker, marOver, name);
                }
                else
                {
                    markersPlaced++;
                    gMap.Position = marker.Position;
                    gMap.Overlays.Add(marOver);
                    //gMap.Position = main;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void removeMarkerOverlay(GMapOverlay marOver)
        {
            BeginInvoke((MethodInvoker)delegate {
                gMap.Overlays.Remove(marOver);
                //gMap.Position = main;
            });
        }

        private void listenerThread_DoWork(object sender, DoWorkEventArgs e)
        {
            Connection.connThread();
        }
    }
}
