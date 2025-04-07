using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechShop
{
    public class OrderDetails
    {
        private int orderDetailID;
        private Orders order;
        private Products product;
        private int quantity;
        private float discount;
        public int OrderDetailID { get => orderDetailID; set => orderDetailID = value; }
        public Orders Order { get => order; set => order = value; }
        public Products Product { get => product; set => product = value; }
        public int Quantity
        {
            get => quantity;
            set
            {
                if (value <= 0)     
                    throw new InvalidDataException("Quantity must be greater than 0.");
                quantity = value;
            }
        }
        public float Discount
        {
            get => discount;
            set
            {
                if (value < 0 || value > 1) 
                    throw new InvalidDataException("Discount must be between 0 and 1.");
                discount = value;
            }
        }
        public OrderDetails(int orderDetailID, Orders orders, Products products, int quantity)
        {
            OrderDetailID = orderDetailID;
            Order = orders;
            Product = products;
            Quantity = quantity;
        }

        public float CalculateSubtotal() 
        {
            if (Product == null) 
                throw new IncompleteOrderException("Product reference is missing.");
            float data = Product.Price * Quantity * (1-Discount);
            Console.WriteLine($"Total amount for the order is: {data}");
            return data;
        }
        public void GetOrderDetailsInfo() 
        {
            Console.WriteLine("======Order Details Info======");
            Console.WriteLine($"Product: {Product.ProductName}");
            Console.WriteLine($"Quantity: {Quantity}");
            Console.WriteLine($"Discount: {Discount * 100}%");
            Console.WriteLine($"Subtotal: {CalculateSubtotal()}");
        }
        public void UpdateQuantity() 
        {
            int newquantity = int.Parse(Console.ReadLine());
            Quantity = newquantity;
            Console.WriteLine("Quantity Updated...");
        }
        public void AddDiscount()   
        {
            float newdiscount= float.Parse(Console.ReadLine());
            Discount = newdiscount;
        }
    }
}
    