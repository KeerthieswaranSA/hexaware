using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechShop
{
    public class Orders
    {
        private int orderID;
        private Customers customer;
        private DateTime orderDate;
        private float totalAmount;
        private string status;
        public int OrderID { get => orderID; set => orderID = value; }
        public Customers Customer { get => customer; set => customer = value; }
        public DateTime OrderDate { get => orderDate; set => orderDate = value; }
        public float TotalAmount { get => totalAmount; private set => totalAmount = value; }
        public string Status { get => status; set => status = value; }

        private List<OrderDetails> orderDetailsList = new List<OrderDetails>();
        public Orders(int orderID, Customers customers, DateTime orderDate, float totalAmount,string status)
        {
            OrderID = orderID;
            Customer = customers;
            OrderDate = orderDate;
            TotalAmount = totalAmount;
            Status = status;
           
        }
      
        public void CalculateTotalAmount()
        {
            double total = 0;

            foreach (OrderDetails detail in orderDetailsList)
            {
                double price = detail.Product.Price;
                int quantity = detail.Quantity;
                double discount = detail.Discount;

                double subtotal = price * quantity;

                if (discount > 0)
                {
                    subtotal -= subtotal * (discount / 100);
                }

                total += subtotal;
            }

            Console.WriteLine("The total amount for the order is: " + total);
        }
        public void GetOrderDetails() 
        {
            Console.WriteLine("======Order Details======");
            Console.WriteLine("Order Id :" + OrderID);
            Console.WriteLine($"Customer Name: {Customer.FirstName} {Customer.LastName}");
            Console.WriteLine($"Order Date: {OrderDate}");
            Console.WriteLine($"TotalAmount:{TotalAmount}");
            Console.WriteLine($"Order Status:{Status}");
            
        }
        public void UpdateOrderStatus() 
        {
            Console.WriteLine("Enter the new Status of the Order:");
            string newStatus = Console.ReadLine();
            Status = newStatus;
        }
        public void CancelOrder() 
        {
            Status = "Canceled";
            Console.WriteLine($"Order {OrderID} has been canceled.");
        }
    }
}
