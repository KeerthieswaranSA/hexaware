using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
namespace EcommApp.Util
{
    public class DBConnection
    {
        private static SqlConnection connection = null;

        public static SqlConnection GetConnection()
        {
            try
            {
                string path = "D:\\Hexaware\\Foundation training\\C#Batch1\\Case Study-E commerce\\EcommApp\\EcommApp\\Util\\property.txt";
                string connStr = PropertyUtil.GetPropertyString(path);
                connection = new SqlConnection(connStr);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Database connection failed: " + ex.Message);
                return null;
            }
        }
    }
}
