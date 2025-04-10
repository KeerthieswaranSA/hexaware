using Microsoft.Data.SqlClient;
using EcommApp.Util;
using EcommApp.Entity;
using EcommApp.DAO;
using EcommApp.myexceptions;

namespace EcommApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            OrderProcessorRepository service = new OrderProcessorRepositoryImpl();
            while (true)
            {
                Console.WriteLine("\n===== E-Commerce Application =====");
                Console.WriteLine("1. Register Customer");
                Console.WriteLine("2. Create Product");
                Console.WriteLine("3. Delete Product");
                Console.WriteLine("4. Add to Cart");
                Console.WriteLine("5. View Cart");
                Console.WriteLine("6. Place Order");
                Console.WriteLine("7. View Customer Orders");
                Console.WriteLine("8. View Product By Id");
                Console.WriteLine("9. Exit");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            Console.WriteLine("Enter Name: ");
                            string name = Console.ReadLine();
                            Console.WriteLine("Enter Email: ");
                            string email = Console.ReadLine();
                            Console.WriteLine("Enter Password: ");
                            string password = Console.ReadLine();
                            Customer customer = new Customer(0,name, email, password);
                            service.CreateCustomer(customer);
                            break;

                        case "2":

                            Console.Write("Enter Product Name: ");
                            string pname = Console.ReadLine();
                            Console.Write("Enter Price: ");
                            decimal price = Convert.ToDecimal(Console.ReadLine());
                            Console.Write("Enter Description: ");
                            string description = Console.ReadLine();
                            Console.Write("Enter Stock Quantity: ");
                            int stock = Convert.ToInt32(Console.ReadLine());
                            Product product = new Product(0,pname, price, description, stock);
                            service.CreateProduct(product);
                            break;
                        case "3":
                            Console.Write("Enter Product ID to delete: ");
                            int deleteId = Convert.ToInt32(Console.ReadLine());
                            service.DeleteProduct(deleteId);
                            break;
                        case "4":
                            Console.Write("Enter Customer ID: ");
                            int cId = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Enter Product ID: ");
                            int pId = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Enter Quantity: ");
                            int qty = Convert.ToInt32(Console.ReadLine());
                            service.AddToCart(new Customer { CustomerId = cId }, new Product { ProductId = pId }, qty);
                            Console.WriteLine("Product added to cart.");
                            break;

                        case "5":
                            Console.Write("Enter Customer ID to view cart: ");
                            int customerId = Convert.ToInt32(Console.ReadLine());
                            List<Product> cartItems = service.GetAllFromCart(new Customer { CustomerId = customerId });
                            Console.WriteLine("Cart Items:");
                            foreach (Product p in cartItems)
                            {
                                Console.WriteLine($"Product ID: {p.ProductId}");
                                Console.WriteLine($"Name: {p.Name}");
                                Console.WriteLine($"Price: {p.Price}");
                                Console.WriteLine($"Description: {p.Description}");
                                Console.WriteLine($"Stock Quantity: {p.StockQuantity}");
                                Console.WriteLine("-----------------------------------");
                            }
                            break;
                            case "6":
                                Console.Write("Enter Customer ID: ");
                                int custId = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Enter Shipping Address: ");
                                string address = Console.ReadLine();
                                Console.WriteLine("Enter the Quantity:");
                                int Quantity = Convert.ToInt32(Console.ReadLine());
                                var cart = service.GetAllFromCart(new Customer { CustomerId = custId });
                                var items = new List<KeyValuePair<Product, int>>();
                                foreach (var cartItem in cart)
                                {
                                    items.Add(new KeyValuePair<Product, int>(cartItem, Quantity));
                                }

                                if (items.Count == 0)
                                {
                                    Console.WriteLine("Cart is empty. Cannot place order.");
                                }
                                else
                                {
                                    bool placed = service.PlaceOrder(new Customer { CustomerId = custId }, items, address); 
                                }
                            break;
                        case "7":
                            Console.Write("Enter Customer ID: ");
                            int ocid = int.Parse(Console.ReadLine());
                            var orders = service.GetOrdersByCustomer(ocid);

                            if (orders.Count == 0)
                                Console.WriteLine("No orders found.");
                            else
                            {
                                foreach (string order in orders)
                                {
                                    Console.WriteLine(order);
                                }
                            }
                            break;
                    case "8":
                        Console.WriteLine("enter product id");
                        int productid = int.Parse(Console.ReadLine());
                        service.ViewProductById(productid);
                        break;
                    case "9":
                        Console.WriteLine("Exiting... Thank you!");
                        return;
                        
                    default:
                         Console.WriteLine("Invalid choice. Please select from the menu.");
                         break;
                    }
            }
        }
    }
}
    