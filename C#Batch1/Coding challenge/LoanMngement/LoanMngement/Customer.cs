using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanMngement
{
    public class Customer
    {
        private int customerId;
        private string name;
        private string emailAddress;
        private string phoneNumber;
        private string address;
        private int creditScore;
        public Customer() { }
        public Customer(int customerId, string name, string emailAddress, string phoneNumber, string address, int creditScore)
        {
            this.customerId = customerId;
            this.name = name;
            this.emailAddress = emailAddress;
            this.phoneNumber = phoneNumber;
            this.address = address;
            this.creditScore = creditScore;
        }

        public int CustomerId { get => customerId; set => customerId = value; }
        public string Name { get => name; set => name = value; }
        public string EmailAddress { get => emailAddress; set => emailAddress = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public string Address { get => address; set => address = value; }
        public int CreditScore { get => creditScore; set => creditScore = value; }

        public void DisplayCustomerInfo()
        {
            Console.WriteLine("=====Customer Details=====");
            Console.WriteLine($"Customer ID: {CustomerId}");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Email: {EmailAddress}");
            Console.WriteLine($"Phone Number: {PhoneNumber}");
            Console.WriteLine($"Address: {Address}");
            Console.WriteLine($"Credit Score: {CreditScore}");
        }
    }
}
