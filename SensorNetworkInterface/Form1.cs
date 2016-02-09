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
        public GMapOverlay markersOverlay = new GMapOverlay("markers");
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

            PointLatLng point = new PointLatLng(42.127331, -80.083581);
            
            GMarkerGoogle markerNode1 = new GMarkerGoogle(new PointLatLng(42.127131, -80.087581), GMarkerGoogleType.green);
            GMarkerGoogle markerNode2 = new GMarkerGoogle(new PointLatLng(42.127331, -80.087381), GMarkerGoogleType.green);
            GMarkerGoogle markerNode3 = new GMarkerGoogle(new PointLatLng(42.127631, -80.087281), GMarkerGoogleType.green);

            markersOverlay.Markers.Add(markerNode1);
            markersOverlay.Markers.Add(markerNode2);
            markersOverlay.Markers.Add(markerNode3);
            
            markerNode1.ToolTipMode = MarkerTooltipMode.Always;
            markerNode1.ToolTip = new GMapToolTip(markerNode1);
            markerNode1.ToolTipText = "Node 1";

            markerNode2.ToolTipMode = MarkerTooltipMode.Always;
            markerNode2.ToolTip = new GMapToolTip(markerNode2);
            markerNode2.ToolTipText = "Node 2";

            markerNode3.ToolTipMode = MarkerTooltipMode.Always;
            markerNode3.ToolTip = new GMapToolTip(markerNode3);
            markerNode3.ToolTipText = "Node 3";
            

            gMap.Overlays.Add(markersOverlay);

            gMap.Position = main;

        }
        
        public void addMarker(GMarkerGoogle marker, GMapOverlay marOver, string name)
        {
            //MessageBox.Show("Add");
            
            BeginInvoke((MethodInvoker)delegate {

                
                marOver.Markers.Add(marker);
                marker.ToolTipMode = MarkerTooltipMode.Always;
                marker.ToolTip = new GMapToolTip(marker);
                marker.ToolTipText = name;

                gMap.Position = marker.Position;
                gMap.Overlays.Add(marOver);
                //gMap.Position = main;
            });

            //MessageBox.Show("End");
        }

        public void removeMarker(GMarkerGoogle marker)
        {
            gMap.Overlays.Remove(markersOverlay);
        }
        
    }
}
