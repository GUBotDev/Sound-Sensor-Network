using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET.MapProviders;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System.Windows.Forms;

namespace SensorNetworkInterface.Class_Files
{
    static class UserInterface
    {
        static List<GMapOverlay> markerOverlays = new List<GMapOverlay>();
        static GMapOverlay markersOverlay = new GMapOverlay("markers");
        static int markersPlaced = 0;
        
        public static void createMarker(double x, double y, string name)
        {
            GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(x, y), GMarkerGoogleType.red_dot);

            markersPlaced++;
            
            marker.ToolTipMode = MarkerTooltipMode.Always;
            marker.ToolTip = new GMapToolTip(marker);
            marker.ToolTipText = name;

            markersOverlay.Markers.Add(marker);

            Program.form1.addMarker(marker, markersOverlay, name);

            markerOverlays.Add(markersOverlay);

            if (markersPlaced > 3)
            {
                removeMarkerOverlay(markersOverlay);

                Console.WriteLine("Should remove marker");
                markersPlaced--;
            }
        }

        public static void removeMarkerOverlay(GMapOverlay markerOverlay)
        {
            Program.form1.removeMarkerOverlay(markerOverlay);
        }
        
        public static void addNode(int nodeNum, double x, double y)
        {
            Program.form1.addNode(nodeNum, x, y);
        }
    }
}
