using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommApp.Util
{
    public static class PropertyUtil
    {
        public static string GetPropertyString(string filePath)
        {
            string hostname = "", dbname = "";
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    if (line.StartsWith("hostname="))
                        hostname = line.Substring("hostname=".Length);
                    else if (line.StartsWith("dbname="))
                        dbname = line.Substring("dbname=".Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading property file ");
            }
            return $"Data Source={hostname};Initial Catalog={dbname};Integrated Security=True;TrustServerCertificate=True;";
        }
    }
}
