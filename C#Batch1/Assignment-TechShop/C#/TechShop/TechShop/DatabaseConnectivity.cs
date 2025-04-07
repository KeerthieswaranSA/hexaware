using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace TechShop
{
        public class DatabaseConnectivity
        {
            static SqlConnection con = null;
            static SqlCommand cmd;
            static SqlDataReader dr;

            public static SqlConnection getConnection()
            {
                try
                {
                    con = new SqlConnection("data source = LAPTOP-53TIM3OI\\SQLEXPRESS;initial catalog = TechShopDB;integrated security = true;TrustServerCertificate=True;");
                    con.Open();
                    return con;
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Database connection error: " + ex.Message);
                    throw; 
                }
            }

            // 1. Customer Registration
            public void RegisterCustomer()
            {
                try
                {
                    Console.WriteLine("Enter the Id:");
                    int CustomerId = int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter the First Name:");
                    string firstname = Console.ReadLine();
                    Console.WriteLine("Enter the Last Name:");
                    string lastname = Console.ReadLine();
                    Console.WriteLine("Enter the Email:");
                    string email = Console.ReadLine();
                    Console.WriteLine("Enter the Phone Number:");
                    string phone = Console.ReadLine();
                
                    con = getConnection();
                         if (!email.Contains("@"))
                            {
                                throw new InvalidDataException("Invalid email format.");
                            }
                        // Check for duplicate email
                        string checkQuery = "SELECT COUNT(*) FROM Customers WHERE Email = @Email";
                        cmd = new SqlCommand(checkQuery, con);
                        cmd.Parameters.AddWithValue("@Email", email);
                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            Console.WriteLine("Error: Email already registered.");
                            return;
                        }

                    // Insert new customer
                    string insertQuery = "INSERT INTO Customers (CustomerId,FirstName,LastName, Email, Phone) VALUES (@CustomerId,@FirstName,@LastName, @Email, @Phone)";
                    cmd = new SqlCommand(insertQuery, con);
                    cmd.Parameters.AddWithValue("@CustomerId", CustomerId);
                    cmd.Parameters.AddWithValue("@FirstName", firstname);
                    cmd.Parameters.AddWithValue("@LastName", lastname);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Phone", phone);
                    
                    int rows = cmd.ExecuteNonQuery();
                    Console.WriteLine(rows > 0 ? "Customer registered successfully." : "Registration failed.");
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Database connection error: " + ex.Message);
                    throw;
                }
              
            }
            // 2. Product Catalog Management
            public void UpdateProduct()
            {
                try
                {

                    con = getConnection();
                    Console.WriteLine("Enter the Id:");
                    int productId = int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter the Product Name:");
                    string name = Console.ReadLine();
                    Console.WriteLine("Enter the Description:");
                    string description = Console.ReadLine();
                    Console.WriteLine("Enter the Price:");
                    int price = int.Parse(Console.ReadLine());
                    // Check if product exists
                    string checkQuery = "SELECT COUNT(*) FROM Products WHERE ProductId = @ProductId";
                    cmd = new SqlCommand(checkQuery, con);
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    int count = (int)cmd.ExecuteScalar();

                    if (count == 0)
                    {
                        Console.WriteLine("Error: Product not found.");
                        return;
                    }

                    // Update product
                    string updateQuery = "UPDATE Products SET ProductName = @Name, Description = @Description, Price = @Price WHERE ProductId = @ProductId";
                    cmd = new SqlCommand(updateQuery, con);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@ProductId", productId);

                    cmd.ExecuteNonQuery();
                    Console.WriteLine( "Product updated successfully.");
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Database connection error: " + ex.Message);
                    throw;
                }    
            }
            //3.Placing Customer Orders
            public void PlaceOrder()
            {

                try
                {
                    con = getConnection();

                    Console.WriteLine("Enter Customer ID: ");
                    int customerId = int.Parse(Console.ReadLine());
                    

                    // Insert Order
                    string insertOrder = "INSERT INTO Orders (CustomerId, OrderDate, Status) OUTPUT INSERTED.OrderId VALUES (@CustomerId, GETDATE(), 'Pending')";
                    cmd = new SqlCommand(insertOrder, con);
                    cmd.Parameters.AddWithValue("@CustomerId", customerId);
                    int orderId = (int)cmd.ExecuteScalar();

                    decimal total = 0;
                    List<(int productId, int quantity)> orderItems = new List<(int, int)>();

                    Console.Write("Enter number of products in the order: ");
                    int count = int.Parse(Console.ReadLine());

                    for (int i = 0; i < count; i++)
                    {
                        Console.Write($"Enter Product ID for item {i + 1}: ");
                        int productId = int.Parse(Console.ReadLine());

                        Console.Write($"Enter Quantity for product {productId}: ");
                        int quantity = int.Parse(Console.ReadLine());

                        if (productId <= 0 || quantity <= 0)
                            throw new InvalidDataException("Invalid product ID or quantity.");

                        orderItems.Add((productId, quantity));
                    }

                    foreach (var item in orderItems)
                    {
                        int productId = item.productId;
                        int quantity = item.quantity;

                        // Get product price
                        string priceQuery = "SELECT Price FROM Products WHERE ProductId = @ProductId";
                        SqlCommand priceCmd = new SqlCommand(priceQuery, con);
                        priceCmd.Parameters.AddWithValue("@ProductId", productId);
                        object result = priceCmd.ExecuteScalar();

                        if (result == null)
                            throw new InvalidDataException($"Product with ID {productId} not found.");

                        decimal price = (decimal)result;

                        // Insert into OrderDetails
                        string detailQuery = "INSERT INTO OrderDetails (OrderId, ProductId, Quantity, Price) VALUES (@OrderId, @ProductId, @Quantity, @Price)";
                        SqlCommand detailCmd = new SqlCommand(detailQuery, con);
                        detailCmd.Parameters.AddWithValue("@OrderId", orderId);
                        detailCmd.Parameters.AddWithValue("@ProductId", productId);
                        detailCmd.Parameters.AddWithValue("@Quantity", quantity);
                        detailCmd.Parameters.AddWithValue("@Price", price);
                        detailCmd.ExecuteNonQuery();

                        // Update inventory
                        string updateInventory = "UPDATE Inventory SET Quantity = Quantity - @Quantity WHERE ProductId = @ProductId";
                        SqlCommand updateCmd = new SqlCommand(updateInventory, con);
                        updateCmd.Parameters.AddWithValue("@Quantity", quantity);
                        updateCmd.Parameters.AddWithValue("@ProductId", productId);
                        updateCmd.ExecuteNonQuery();

                        total += price * quantity;
                    }

                    Console.WriteLine($"Order placed successfully. Order ID: {orderId}, Total Amount: {total}");
                
                }
                catch (IncompleteOrderException ex)
                {
                        Console.WriteLine("Error in Order palcement..." + ex.Message);
                        throw;
                }
            }
            //4.Tracking order status 
            public void TrackOrderStatus()
            {
                try
                {
                    con = getConnection();
                    Console.WriteLine("Enter the Order Id:");
                    int orderId = int.Parse(Console.ReadLine());
                    string query = "SELECT OrderId, OrderDate, Status, TotalAmount FROM Orders WHERE OrderId = @OrderId";
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@OrderId", orderId);

                    dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        Console.WriteLine("=== Order Status ===");
                        Console.WriteLine("Order ID     : " + dr["OrderId"]);
                        Console.WriteLine("Order Date   : " + Convert.ToDateTime(dr["OrderDate"]).ToShortDateString());
                        Console.WriteLine("Status       : " + dr["Status"]);
                        Console.WriteLine("Total Amount : " + dr["TotalAmount"]);
                    }
                    else
                    {
                        Console.WriteLine("Order not found.");
                    }

                    dr.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            //5.Inventory Management
            public void ManageInventory( )
            {
                try
                {
                    con = getConnection();
                    Console.WriteLine("Enter true to insert new Inventory Record else enter False:");
                    bool isNewProduct = bool.Parse(Console.ReadLine());
                    Console.WriteLine("Enter true to discontinue Inventory Record else enter False:");
                    bool discontinue = bool.Parse(Console.ReadLine());
                    if (isNewProduct)
                    {
                    // Insert new inventory record
                        Console.WriteLine("Enter the Id:");
                        int productId = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter the Quantity:");
                        int quantity = int.Parse(Console.ReadLine());
                        string insertQuery = "INSERT INTO Inventory (ProductId, QuantityInStock) VALUES (@ProductId, @Quantity)";
                        cmd = new SqlCommand(insertQuery, con);
                        cmd.Parameters.AddWithValue("@ProductId", productId);
                    
                        cmd.ExecuteNonQuery();

                        Console.WriteLine("New inventory added.");
                    }
                    else if (discontinue)
                    {
                    // Set stock to 0 and mark product as inactive
                        Console.WriteLine("Enter the Id:");
                        int productId = int.Parse(Console.ReadLine());
                        string updateQuery = "UPDATE Inventory SET QuantityInStock = 0 WHERE ProductId = @ProductId";
                        cmd = new SqlCommand(updateQuery, con);
                        cmd.Parameters.AddWithValue("@ProductId", productId);
                    
                        cmd.ExecuteNonQuery();

                        //update product status in Products table
                        string disableProduct = "UPDATE Products SET Description = Description + ' (Discontinued)' WHERE ProductId = @ProductId";
                        SqlCommand cmd2 = new SqlCommand(disableProduct, con);
                        cmd2.Parameters.AddWithValue("@ProductId", productId);
                        cmd2.ExecuteNonQuery();

                        Console.WriteLine("Product marked as discontinued.");
                    }
                    else
                    {
                    // Update stock level
                        Console.WriteLine("Enter the Id:");
                        int productId = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter the Quantity:");
                        int quantity = int.Parse(Console.ReadLine());
                        string updateQuery = "UPDATE Inventory SET QuantityInStock" +
                        " =  @Quantity WHERE ProductId = @ProductId";
                        cmd = new SqlCommand(updateQuery, con);
                        cmd.Parameters.AddWithValue("@ProductId", productId);
                        cmd.Parameters.AddWithValue("@Quantity", quantity);
                    
                        cmd.ExecuteNonQuery();

                        Console.WriteLine("Inventory updated.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error managing inventory: " + ex.Message);
                }
            }
            //6.Sales Report
            public void GenerateSalesReport()
            {
                try
                {
                    con = getConnection();
                    string query = @"
                    SELECT 
                        P.ProductName, 
                        SUM(OD.Quantity) AS UnitsSold,
                        SUM(OD.Quantity * P.Price) AS Revenue
                    FROM OrderDetails OD
                    JOIN Products P ON OD.ProductId = P.ProductId
                    GROUP BY P.ProductName";

                    cmd = new SqlCommand(query, con);
                    dr = cmd.ExecuteReader();

                    Console.WriteLine("===== SALES REPORT =====");
                    Console.WriteLine("Product", "Units Sold", "Revenue");
                    Console.WriteLine("--------------------");

                    while (dr.Read())
                    {
                        string productName = dr["ProductName"].ToString();
                        int unitsSold = Convert.ToInt32(dr["UnitsSold"]);
                        decimal revenue = Convert.ToDecimal(dr["Revenue"]);

                        Console.WriteLine($"{productName} {unitsSold} {revenue}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error generating sales report: " + ex.Message);
                }
            }

            // 7. Customer Account Updates
            public void UpdateCustomerAccount()
            {
                try
                {
                    con = getConnection();
                    Console.WriteLine("Enter the Id:");
                    int customerId = int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter the Email:");
                    string newEmail= Console.ReadLine();
                    Console.WriteLine("Enter the Phone Number:");
                    string newPhone = Console.ReadLine();
                    // Check if new email already exists
                    string checkEmail = "SELECT COUNT(*) FROM Customers WHERE Email = @Email AND CustomerId != @Id";
                    cmd = new SqlCommand(checkEmail, con);
                    cmd.Parameters.AddWithValue("@Email", newEmail);
                    cmd.Parameters.AddWithValue("@Id", customerId);

                    int exists = (int)cmd.ExecuteScalar();
                    if (exists > 0)
                    {
                        Console.WriteLine("Email already in use by another customer.");
                        return;
                    }

                    // Proceed to update
                    string query = "UPDATE Customers SET Email = @Email, Phone = @Phone WHERE CustomerId = @Id";
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Email", newEmail);
                    cmd.Parameters.AddWithValue("@Phone", newPhone);
                    cmd.Parameters.AddWithValue("@Id", customerId);

                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                        Console.WriteLine("Customer account updated successfully.");
                    else
                        Console.WriteLine("Customer not found.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error updating account: " + ex.Message);
                }
            }
            // 8. Payment Processing
            public void ProcessPayment( int amount, string method)
            {
                try
                {
                    con = getConnection();
                    Console.WriteLine("Enter the Id:");
                    int orderId = int.Parse(Console.ReadLine());
                    // Check if the order exists
                    string checkOrder = "SELECT COUNT(*) FROM Orders WHERE OrderId = @OrderId";
                    cmd = new SqlCommand(checkOrder, con);
                    cmd.Parameters.AddWithValue("@OrderId", orderId);
                    int exists = (int)cmd.ExecuteScalar();

                    if (exists == 0)
                    {
                        Console.WriteLine("Order not found.");
                        return;
                    }

                    // Insert payment record
                    string query = "INSERT INTO Payments (OrderId, Amount, PaymentMethod, PaymentDate) " +
                                   "VALUES (@OrderId, @Amount, @Method, @Date)";
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@OrderId", orderId);
                    cmd.Parameters.AddWithValue("@Amount", amount);
                    cmd.Parameters.AddWithValue("@Method", method);
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                        Console.WriteLine("Payment processed successfully.");
                    else
                        Console.WriteLine("Failed to process payment.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error processing payment: " + ex.Message);
                }
            }
            //9.Product search and recommendations
            public void SearchProduct()
            {
                try
                {

                    con = getConnection();
                    Console.WriteLine("Enter the Product Name:");
                    string name = Console.ReadLine();
                    Console.WriteLine("Enter the Category of Product(Electronics Gadgets/Computer Peripherals/Storage & Networking):");
                    string category = Console.ReadLine();
                    string query = "SELECT ProductId, ProductName, Category, Price FROM Products WHERE ProductName LIKE @name OR Category LIKE @category";
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@name", "%" + name + "%");
                    cmd.Parameters.AddWithValue("@category", "%" + category + "%");

                    dr = cmd.ExecuteReader();

                    Console.WriteLine("\n--- Search Results ---");
                    bool found = false;
                    while (dr.Read())
                    {
                        found = true;
                        Console.WriteLine($"ID: {dr["ProductId"]}, Name: {dr["ProductName"]}, Category: {dr["Category"]}, Price: {dr["Price"]}");
                    }

                    if (!found)
                    {
                        Console.WriteLine("No matching products found.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error during search: " + ex.Message);
                }
            }
        }
}
