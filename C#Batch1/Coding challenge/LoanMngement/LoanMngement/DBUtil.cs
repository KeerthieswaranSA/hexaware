using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace LoanMngement
{
    public class DBUtil
    {
        static SqlConnection conn = null;
        static SqlCommand cmd;
        static SqlDataReader dr;
        public static SqlConnection GetDBConn()
        {
            try
            {
                conn = new SqlConnection("data source =LAPTOP-53TIM3OI\\SQLEXPRESS;initial catalog = Loan_Management;integrated security = true;TrustServerCertificate=True;");
                conn.Open();
                return conn;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database connection failed: " + ex.Message);
                return null;
            }
        }
    }
}