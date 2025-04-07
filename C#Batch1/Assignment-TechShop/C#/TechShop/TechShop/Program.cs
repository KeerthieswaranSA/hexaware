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

    internal class Program
    {
        static SqlConnection con = null;
        static SqlCommand cmd;
        static SqlDataReader dr;
        static void Main(string[] args)
        {

            Console.WriteLine("Hello, World!");
            Customers customers= new Customers(100,"ram","sam","sam@123.com","8080809595","sam st,123");
            Orders orders = new Orders(501,customers,DateTime.Now,1000,"Placed");
            Products products =new Products(1,"Laptop","16GB RAM",60000,"Electronic Gadgets");
            OrderDetails orderDetails = new OrderDetails(101,orders,products,5);
            Inventory inventory = new Inventory(1001, products, 1, DateTime.Now);
            customers.GetCustomerDetails();
            orders.GetOrderDetails();
            products.GetProductDetails();
            orderDetails.GetOrderDetailsInfo();
            inventory.ListAllProducts();
            customers.CalculateTotalOrders();
            DatabaseConnectivity.getConnection();
            DatabaseConnectivity databaseConnectivity = new DatabaseConnectivity();
            //databaseConnectivity.RegisterCustomer(); 
            //databaseConnectivity.UpdateProduct();    
            databaseConnectivity.PlaceOrder();
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
