using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;


namespace SenNetDataHandler.Class_Files
{
    public static class mySQLDB
    {
        public static MySqlConnection connection;
        private static bool isConnected = false;

        /// <summary>
        /// modify storage technique
        /// 
        /// Current max frequency is around 931.2Hz for one rotation - 1/10 what it should be (using memory storage)
        /// This gives a sampling rate of 155.2Hz and a maximum audible of frequency of 77.6Hz - not useable
        /// 
        /// -only check if the table exists once, if the data from the node is incorrect then it shouldn't get stored
        /// -changed query execution method to asynchronous without wait
        /// 
        /// send data via tasks? -> with sync method
        /// 
        /// </summary>
        /// <param name="_server">The address of the server</param>
        /// <param name="_database">The name of the database</param>
        /// <param name="_userId">The user id</param>
        /// <param name="_userPass">The user password</param>
        public static void connect(string _server, string _database, string _userId, string _userPass)
        {
            string connStr = "SERVER=" + _server + ";" + "DATABASE=" + _database + ";" + "UID=" + _userId + ";" + "PASSWORD=" + _userPass + ";";

            connection = new MySqlConnection(connStr);

            try
            {
                connection.OpenAsync();
                Console.WriteLine("Connected to database at: " + _server);
                isConnected = true;
            }
            catch (MySqlException exception)
            {
                Console.WriteLine("Cannot establish connection to:" + _server);
                Console.WriteLine("Error: " + exception);
                disconnect();
            }

        }

        public static void disconnect()
        {
            if (isConnected)
            {
                connection.CloseAsync();
                Console.WriteLine("Closed database connection.");
                isConnected = false;
            }
        }
    }
}