using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommApp.Entity
{
    public class Customer
    {
        private int customerId;
        private string name;
        private string email;
        private string password;
        public Customer() { }
        public Customer(int customerId, string name, string email, string password)
        {
            this.customerId = customerId;
            this.name = name;
            this.email = email;
            this.password = password;
        }
        public int CustomerId { get => customerId; set => customerId = value; }
        public string Name { get => name; set => name = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
    }
}
