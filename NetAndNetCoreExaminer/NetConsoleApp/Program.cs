//using CoreClassLibrary;

using System;
using System.Data.SqlClient;

namespace NetConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string serverName = @"DESKTOP-212301O\SRISQLDEV2019";

            string connectionString = $"Server= {serverName}; Database= SteelSections; Integrated Security=True;";

            SqlConnection sqlConnection = new SqlConnection(connectionString);

            sqlConnection.Open();


            SqlCommand sqlCommand;

            string query = "SELECT * FROM Countries";

            sqlCommand = new SqlCommand(query, sqlConnection);

            var results = sqlCommand.ExecuteScalar();

            Console.WriteLine(results.ToString());

            Console.ReadLine();

            sqlConnection.Close();

            //Class cls = new Class();

            //Console.WriteLine($"Result: {cls.GetFactor(0.3)}");

        }
    }
}
