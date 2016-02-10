using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SensorNetworkInterface.Class_Files
{
    public static class Program
    {
        public static Form1 form1;
       
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //connection = new Thread(Connection.connThread);
            //connection.Start();
            
            form1 = new Form1();
            Application.Run(form1);
        }
    }
}
