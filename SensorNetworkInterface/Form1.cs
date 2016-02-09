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
        }

        public void addNode(int nodeNum, double x, double y)
        {
            markersOverlay = new GMapOverlay("markers");

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

        public void addMarker(GMarkerGoogle marker, GMapOverlay marOver, string name)
        {
            //MessageBox.Show("Add");
            
            BeginInvoke((MethodInvoker)delegate {

                gMap.Position = marker.Position;
                gMap.Overlays.Add(marOver);
                //gMap.Position = main;
            });

            //MessageBox.Show("End");
        }

        public void removeMarkerOverlay(GMapOverlay marOver)
        {
            BeginInvoke((MethodInvoker)delegate {
                gMap.Overlays.Remove(marOver);
                //gMap.Position = main;
            });
        }
        
    }
}
