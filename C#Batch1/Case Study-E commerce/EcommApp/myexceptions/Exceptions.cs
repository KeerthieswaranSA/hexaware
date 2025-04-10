using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommApp.myexceptions
{
    public class Exceptions
    {
        public class CustomerNotFoundException : Exception
        {
            public CustomerNotFoundException(string message) : base(message) { }
        }
        public class ProductNotFoundException : Exception
        {
            public ProductNotFoundException(string message) : base(message) { }
        }
        public class OrderNotFoundException : Exception
        {
            public OrderNotFoundException(string message) : base(message) { }
        }
    }
}
