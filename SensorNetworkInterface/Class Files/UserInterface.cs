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
        static List<GMarkerGoogle> markers = new List<GMarkerGoogle>();
        static GMapOverlay markersOverlay = new GMapOverlay("markers");
        static int markersPlaced = 0;
        
        public static void createMarker(double x, double y, string name)
        {
            GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(x, y), GMarkerGoogleType.red_dot);

            markers.Add(marker);
            markersPlaced++;

            Program.form1.addMarker(marker, markersOverlay, name);

            if (markersPlaced > 10)
            {
                markersOverlay.Markers.Remove(markers.First());
                removeMarker(markers.First());
            }
        }

        public static void removeMarker(GMarkerGoogle marker)
        {
            Program.form1.removeMarker(marker);
        }
        
    }
}
