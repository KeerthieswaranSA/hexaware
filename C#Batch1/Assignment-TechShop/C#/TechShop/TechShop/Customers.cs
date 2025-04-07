using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
namespace TechShop
{ 
    public class Customers
    {

        protected int customerID;
        protected string firstName;
        protected string lastName;
        protected string email;
        protected string phone;
        protected string address;
        public int CustomerID { get => customerID; set => customerID = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string Email
        {
            get => email;
            set
            {
                if (!value.Contains("@")) 
                    throw new InvalidDataException("Invalid email format.");
                email = value;
            }
        }
        public string Phone { get => phone; set => phone = value; }
        public string Address { get => address; set => address = value; }
        public List<Orders> Orders { get; set; } = new();
        public Customers(int id, string fName, string lName, string email, string phone, string address)
        {
            CustomerID = id;
            FirstName = fName;
            LastName = lName;
            Email = email;
            Phone = phone;
            Address = address;
        }
        public void CalculateTotalOrders() 
        {
            Console.WriteLine($"Customer {FirstName} {LastName} has placed {Orders.Count} orders.");
        }
        public void GetCustomerDetails() 
        {
            Console.WriteLine("======Customer Details======");
            Console.WriteLine("Customer Id: "+ CustomerID);
            Console.WriteLine($"Name: {FirstName} {LastName}");
            Console.WriteLine($"Email: {Email}");
            Console.WriteLine($"Phone Number: {Phone}");
            Console.WriteLine($"Address: {Address}");
        }
        public void UpdateCustomerInfo(string newEmail,string newPhone,string newAddress)
        {
            Email = newEmail;
            Phone = newPhone;
            Address = newAddress;
            Console.WriteLine("Customer info updated Sucessfully...");
        }
    }
}
