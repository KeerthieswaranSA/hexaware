using System.Numerics;
using Microsoft.Data.SqlClient;

namespace TechShop
{
    public class InvalidDataException : Exception
    {
        public InvalidDataException(string message) : base(message) { }
    }
    public class InsufficientStockException : Exception
    {
        public InsufficientStockException(string message) : base(message) { }
    }
    public class IncompleteOrderException : Exception
    {
        public IncompleteOrderException(string message) : base(message) { }
    }
    public class SqlException : Exception
    {
        public SqlException(string message) : base(message) { }
    }

    public class IOException : Exception
    {
        public IOException(string message) : base(message) { }
    }
    public class PaymentFailedException : Exception
    {
        public PaymentFailedException(string message) : base(message) { }
    }
    internal class Program
    { 
        static SqlConnection con = null;
        static SqlCommand cmd;
        static SqlDataReader dr;
        static void Main(string[] args)
        {

            Customers customers= new Customers(100,"Raj", "Kumar", "raj@abc.com","8080809595","ram st,123");
            Orders orders = new Orders(501,customers,DateTime.Now,1000,"Placed");
            Products products =new Products(1,"Laptop","16GB RAM",60000,"Electronic Gadgets");
            OrderDetails orderDetails = new OrderDetails(101,orders,products,5);
            Inventory inventory = new Inventory(1001, products, 1, DateTime.Now);
            //inventory.RemoveFromInventory(1000);
            //customers.GetCustomerDetails();
            //orders.GetOrderDetails();
            //products.GetProductDetails();
            //orderDetails.GetOrderDetailsInfo();
            //inventory.ListAllProducts();
            //customers.CalculateTotalOrders();
            //----------------------Collections--------------------------
            //Collectionseg collection = new Collectionseg();
            ////Add products
            //Console.WriteLine("=====Adding Product to the List======");
            //Products p1 = new Products(1, "Smartphone", "Android phone", 15000, "Electronics");
            //Products p2 = new Products(2, "Laptop", "Gaming", 55000, "Electronics");
            //Products p3 = new Products(3, "Pendrive", "4Gb", 2500, "Accessories");
            //Products p4 = new Products(4, "External SSD", "1TB", 3000, "Accessories");
            //collection.AddProduct(p1);
            //collection.AddProduct(p2);
            //collection.AddProduct(p3);
            //collection.AddProduct(p4);
            ////Update Product
            //Console.WriteLine("\n=====Update Product Details======");
            //collection.UpdateProduct(5, "Gaming Keyboard", "RGB Keyboard", 2999);

            ////Add to Inventory
            //Console.WriteLine("\n=====Adding Product to the Inventory List======");
            //collection.AddToInventory(p1.ProductID, p1, 10);
            //collection.AddToInventory(p2.ProductID, p2, 5);
            //collection.AddToInventory(p3.ProductID, p3, 15);
            //collection.AddToInventory(p4.ProductID, p4, 7);

            //// Display Inventory
            //Console.WriteLine("\n=====Display Product Data======");
            //collection.DisplayInventory();

            ////Remove Product
            //Console.WriteLine("\n=====Remove Product Data======");
            //collection.RemoveProduct(4);

            ////Update Stock
            //Console.WriteLine("\n=====Updating Stocks======");
            //collection.UpdateStock(p2.ProductID, 10);
            //collection.DisplayInventory();

            ////Search Product by Name
            //Console.WriteLine("\n=====searched Product by name======");
            //collection.SearchProductsByName("phone");

            ////Search Product by Category
            //Console.WriteLine("\n=====searched Product by Category======");
            //collection.SearchProductsByCategory("Accessories");

            ////Add Order
            //Console.WriteLine("\n=====Order Placement======");
            //Customers cust1 = new Customers(2, "Albert", "John", "john@abc.com", "9568741230", "New York");
            //Orders order1 = new Orders(1001, cust1, new DateTime(2025, 12, 1), 55000, "Pending");
            //collection.AddOrder(order1);

            ////Update Order Status
            //Console.WriteLine("\n=====Update Order Status======");
            //collection.UpdateOrderStatus(1001, "Shipped");

            ////Get Orders by Date Range
            //Console.WriteLine("\n=====Order within Dates======");
            //collection.DisplayOrderDetailsByDateRange(new DateTime(2023,5,5), new DateTime(2026,10,12));


            ////Record Payment
            //Console.WriteLine("\n=====Payment Processing======");
            //Payment payment1 = new Payment(201, order1.OrderID, 17999, "UPI", "Pending");
            //collection.RecordPayment(payment1);

            ////Display All Payments
            //Console.WriteLine("\n=====Payment Records======");
            //collection.DisplayAllPayments();

            ////Update Payment Status
            //Console.WriteLine("\n=====Payment Status======");
            //collection.UpdatePaymentStatus(201, "Completed");
            //collection.DisplayAllPayments();

            //collection.AddProduct(new Products(1, "Laptop", "Gaming laptop", 50000, "Electronics"));
            //collection.AddProduct(new Products(2, "Smartphone", "Latest model", 9999, "Electronics"));
            //// 2. Update a Product
            //collection.UpdateProduct(1, "Gaming Laptop", "High-end gaming laptop", 60000);

            //// 3. Search Products by Name
            //collection.SearchProductsByName("Laptop");

            //// 4. Search Products by Category
            //collection.SearchProductsByCategory("Electronics");
            //collection.UpdateStock(1, 10);  // Add 10 units of Laptop
            //collection.UpdateStock(2, 20);  // Add 20 units of Smartphone

            //// Display the inventory
            // collection.DisplayInventory();
            //Console.WriteLine("============================================================================");
             DatabaseConnectivity.getConnection();
            DatabaseConnectivity databaseConnectivity = new DatabaseConnectivity();
            //databaseConnectivity.RegisterCustomer();
            databaseConnectivity.UpdateProduct();
            //databaseConnectivity.PlaceOrder();
            //databaseConnectivity.TrackOrderStatus();
            //databaseConnectivity.ManageInventory();
            //databaseConnectivity.GenerateSalesReport();
            //databaseConnectivity.UpdateCustomerAccount();
            //databaseConnectivity.ProcessPayment();
            //databaseConnectivity.SearchProduct(); 
            Console.Read();

        }
        
    }
}
