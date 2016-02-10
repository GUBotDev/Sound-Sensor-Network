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
        static List<GMapOverlay> overlays = new List<GMapOverlay>();
        static GMapOverlay markersOverlay;
        static int markersPlaced = 0;
        
        public static void createMarker(double x, double y, string name)
        {
            try
            {
                markersOverlay = new GMapOverlay("markers");

                GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(x, y), GMarkerGoogleType.red_dot);

                marker.ToolTipMode = MarkerTooltipMode.Always;
                marker.ToolTip = new GMapToolTip(marker);
                marker.ToolTipText = (x + ", " + y).ToString();
                
                if (markersPlaced >= 6)
                {
                    //markers.ElementAt(1).IsVisible = true;
                    //markers.ElementAt(2).IsVisible = false;
                    //markers.ElementAt(1).IsVisible = false;
                    //markers.ElementAt(0).IsVisible = false;
                    //markers.Last().Dispose();
                    overlays.ToList().First().Markers.Remove(markers.ToList().First());
                    markers.Remove(markers.ToList().First());
                    overlays.Remove(overlays.ToList().First());

                    //Console.WriteLine("Should remove marker");
                }
                else
                {
                    markersPlaced++;
                }

                markersOverlay.Markers.Add(marker);
                markers.Add(marker);
                overlays.Add(markersOverlay);

                Program.form1.gMap.Invoke(new Action(() =>
                    {
                        //Program.form1.addMarker(marker, markersOverlay, name);

                        Program.form1.gMap.Overlays.Add(markersOverlay);
                        Program.form1.gMap.Position = marker.Position;
                        //gMap.Position = main;
                    }
                ));

            }
            catch (Exception ex)
            {
                Console.WriteLine("Create Marker: " + ex.Message);
            }
        }
        
        public static void addNode(int nodeNum, double x, double y)
        {
            try
            {
                Program.form1.addNode(nodeNum, x, y, markersOverlay);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Add Node: " + ex.Message);
            }
        }
    }
}
