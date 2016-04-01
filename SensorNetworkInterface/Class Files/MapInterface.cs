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
    static class MapInterface
    {
        static Dictionary<GMarkerGoogle, DateTime> markers = new Dictionary<GMarkerGoogle, DateTime>();
        static List<GMapOverlay> overlays = new List<GMapOverlay>();
        static GMapOverlay markersOverlay;
        static int markersPlaced = 0;
        static int saveTimeFor = 30;
        
        public static void createMarker(double x, double y, string name)
        {
            try
            {
                markersOverlay = new GMapOverlay("markers");

                GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(x, y), GMarkerGoogleType.red_dot);
                DateTime dt = DateTime.Now;

                marker.ToolTipMode = MarkerTooltipMode.Always;
                marker.ToolTip = new GMapToolTip(marker);
                marker.ToolTipText = (x + ", " + y + ", " + dt.ToString("hh:mm:ss.ffff"));
                
                if (markers.Any() && markers.First().Value.AddMinutes(saveTimeFor) < DateTime.Now)//markersPlaced >= 6)
                {
                    //markers.ElementAt(1).IsVisible = true;
                    //markers.ElementAt(2).IsVisible = false;
                    //markers.ElementAt(1).IsVisible = false;
                    //markers.ElementAt(0).IsVisible = false;
                    //markers.Last().Dispose();
                    overlays.ToList().First().Markers.Remove(markers.First().Key);
                    markers.Remove(markers.First().Key);
                    overlays.Remove(overlays.ToList().First());
                }
                else
                {
                    markersPlaced++;
                }

                markersOverlay.Markers.Add(marker);
                markers.Add(marker, dt);
                overlays.Add(markersOverlay);

                Program.mainForm.gMap.Invoke(new Action(() =>
                    {
                        Program.mainForm.gMap.Overlays.Add(markersOverlay);
                        Program.mainForm.gMap.Position = marker.Position;
                        //gMap.Position = main;
                    }
                ));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create Marker: " + ex);
            }
        }
        
        public static void addNode(int nodeNum, double x, double y)
        {
            try
            {
                markersOverlay = new GMapOverlay("markers");

                PointLatLng point = new PointLatLng(x, y);

                GMarkerGoogle markerNode = new GMarkerGoogle(point, GMarkerGoogleType.green);

                markersOverlay.Markers.Add(markerNode);

                markerNode.ToolTipMode = MarkerTooltipMode.Always;
                markerNode.ToolTip = new GMapToolTip(markerNode);
                markerNode.ToolTipText = "Node " + nodeNum;
                
                Program.mainForm.gMap.Invoke(new Action(() =>
                {
                    Program.mainForm.gMap.Overlays.Add(markersOverlay);
                    Program.mainForm.gMap.Position = point;
                    //gMap.Position = main;

                    //Program.mainForm.addMarker(marker, markersOverlay, name);

                    //Program.mainForm.addNode(nodeNum, x, y, markersOverlay);

                    //gMap.Position = main;
                }
                ));

            }
            catch (Exception ex)
            {
                Console.WriteLine("Add Node: " + ex.Message);
            }
        }

    }
}
