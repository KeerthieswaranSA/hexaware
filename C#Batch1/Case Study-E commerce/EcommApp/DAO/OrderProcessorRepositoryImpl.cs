using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcommApp.Entity;
using EcommApp.Util;
using Microsoft.Data.SqlClient;
using static EcommApp.myexceptions.Exceptions;

namespace EcommApp.DAO
{
    public class OrderProcessorRepositoryImpl : OrderProcessorRepository
    {
        // 1. Create a new product
        public bool CreateProduct(Product product)
        {
            try
            {
                using (SqlConnection connection = DBConnection.GetConnection())
                {
                    
                    string query = "INSERT INTO products (name, price, description, stockQuantity) VALUES (@name, @price, @description, @stockQuantity)";
                    SqlCommand cmd = new SqlCommand(query, connection);        
                    cmd.Parameters.AddWithValue("@name", product.Name);
                    cmd.Parameters.AddWithValue("@price", product.Price);
                    cmd.Parameters.AddWithValue("@description", product.Description);
                    cmd.Parameters.AddWithValue("@stockQuantity", product.StockQuantity);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine("Product created successfully.");
                    return rowsAffected > 0;
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("SQL Error: " + e.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }


        // 2. Register a new customer
        public bool CreateCustomer(Customer customer)
        {
            try
            {
                using (SqlConnection connection = DBConnection.GetConnection())
                {
                    Console.WriteLine("Welcome for Customer Registration...");
                    string query = "INSERT INTO customers (name, email, password) VALUES (@name, @email, @password)";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@name", customer.Name);
                    cmd.Parameters.AddWithValue("@email", customer.Email);
                    cmd.Parameters.AddWithValue("@password", customer.Password);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine("Customer registered successfully.");
                    return rowsAffected > 0;
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("SQL Error: " + e.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        // 3. Delete a product by productId
        public bool DeleteProduct(int productId)
        {
            try
            {
                using (SqlConnection connection = DBConnection.GetConnection())
                {
                    string query = "DELETE FROM products WHERE product_id = @productId";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@productId", productId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new ProductNotFoundException("Product ID " + productId + " does not exist.");
                        return false;
                    }
                    Console.WriteLine("Product deleted successfully.");
                    return true;
                }
            }
            catch (ProductNotFoundException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        // 4. Delete a customer by customerId
        public bool DeleteCustomer(int customerId)
        {
            try
            {
                using (SqlConnection connection = DBConnection.GetConnection())
                {
                    string query = "DELETE FROM customers WHERE customer_id = @customerId";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@customerId", customerId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new CustomerNotFoundException("Customer ID " + customerId + " does not exist.");
                    }

                    return true;
                }
            }
            catch (CustomerNotFoundException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        // 5. Add product to cart
        public bool AddToCart(Customer customer, Product product, int quantity)
        {
            try
            {
                using (SqlConnection connection = DBConnection.GetConnection())
                {
                    string checkProductQuery = "SELECT stockQuantity FROM products WHERE product_id = @productId";
                    SqlCommand checkCmd = new SqlCommand(checkProductQuery, connection);
                    checkCmd.Parameters.AddWithValue("@productId", product.ProductId);

                    object result = checkCmd.ExecuteScalar();

                    if (result == null)
                    {
                        throw new ProductNotFoundException("Product ID " + product.ProductId + " does not exist.");
                    }

                    int stock = Convert.ToInt32(result);
                    if (stock < quantity)
                    {
                        Console.WriteLine("Not enough stock available.");
                        return false;
                    }

                    string insertQuery = @"INSERT INTO cart (customer_id, product_id, quantity)
                                   VALUES (@customerId, @productId, @quantity)";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, connection);
                    insertCmd.Parameters.AddWithValue("@customerId", customer.CustomerId);
                    insertCmd.Parameters.AddWithValue("@productId", product.ProductId);
                    insertCmd.Parameters.AddWithValue("@quantity", quantity);

                    int rowsInserted = insertCmd.ExecuteNonQuery();
                    return rowsInserted > 0;
                }
            }
            catch (ProductNotFoundException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        // 6. Remove product from cart
        public bool RemoveFromCart(Customer customer, Product product)
        {
            try
            {
                using (SqlConnection connection = DBConnection.GetConnection())
                {
                    string query = @"DELETE FROM cart 
                             WHERE customer_id = @customerId AND product_id = @productId";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@customerId", customer.CustomerId);
                    command.Parameters.AddWithValue("@productId", product.ProductId);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error: " + ex.Message);
                return false;
            }
        }


        // 7. Get all products from cart
        public List<Product> GetAllFromCart(Customer customer)
        {
            List<Product> productList = new List<Product>();

            try
            {
                using (SqlConnection connection = DBConnection.GetConnection())
                {
                    string query = @"SELECT p.product_id, p.name, p.price, p.description, p.stockQuantity, c.quantity 
                             FROM cart c 
                             JOIN products p ON c.product_id = p.product_id 
                             WHERE c.customer_id = @customerId";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@customerId", customer.CustomerId);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Product p = new Product
                        {
                            ProductId = Convert.ToInt32(reader["product_id"]),
                            Name = reader["name"].ToString(),
                            Price = Convert.ToDecimal(reader["price"]),
                            Description = reader["description"].ToString(),
                            StockQuantity = Convert.ToInt32(reader["stockQuantity"]),
                            
                        };
                        Cart c = new Cart { Quantity = Convert.ToInt32(reader["quantity"])};
                        productList.Add(p);

                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
            }
            return productList;
        }


        // 8. Place an order (adds to orders and order_items)
        public bool PlaceOrder(Customer customer, List<KeyValuePair<Product, int>> productQuantityList, string shippingAddress)
        {
            bool isPlaced = false;

            try
            {
                using (SqlConnection connection = DBConnection.GetConnection())
                {
                    

                    try
                    {
                        decimal totalPrice = 0;
                        foreach (var entry in productQuantityList)
                        {
                            totalPrice += entry.Key.Price * entry.Value;
                        }

                        string insertOrder = @"INSERT INTO orders (customer_id, order_date, total_price, shipping_address)
                                       VALUES (@customerId, @orderDate, @totalPrice, @shippingAddress);
                                       SELECT SCOPE_IDENTITY();";

                        SqlCommand cmdOrder = new SqlCommand(insertOrder, connection);
                        cmdOrder.Parameters.AddWithValue("@customerId", customer.CustomerId);
                        cmdOrder.Parameters.AddWithValue("@orderDate", DateTime.Now);
                        cmdOrder.Parameters.AddWithValue("@totalPrice", totalPrice);
                        cmdOrder.Parameters.AddWithValue("@shippingAddress", shippingAddress);

                       int orderId = Convert.ToInt32(cmdOrder.ExecuteScalar());

                        foreach (var entry in productQuantityList)
                        {
                            string insertItem = @"INSERT INTO order_items (order_id, product_id, quantity)
                                          VALUES (@orderId, @productId, @quantity)";
                            SqlCommand cmdItem = new SqlCommand(insertItem, connection);
                            cmdItem.Parameters.AddWithValue("@orderId", orderId);
                            cmdItem.Parameters.AddWithValue("@productId", entry.Key.ProductId);
                            cmdItem.Parameters.AddWithValue("@quantity", entry.Value);
                            cmdItem.ExecuteNonQuery();

                            string updateStock = @"UPDATE products SET stockQuantity = stockQuantity - @quantity WHERE product_id = @productId";
                            SqlCommand cmdStock = new SqlCommand(updateStock, connection);
                            cmdStock.Parameters.AddWithValue("@quantity", entry.Value);
                            cmdStock.Parameters.AddWithValue("@productId", entry.Key.ProductId);
                            cmdStock.ExecuteNonQuery();
                        }
                        Console.WriteLine("Order placed successfully.");
                        isPlaced = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Order failed: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection error: " + ex.Message);
            }
            return  isPlaced;
        }


        // 9. Get orders by customerId
        public List<string> GetOrdersByCustomer(int customerId)
        {
            List<string> orderDetails = new List<string>();

            try
            {
                using (SqlConnection connection = DBConnection.GetConnection())
                {
                    string queryOrders = @"SELECT o.order_id, o.order_date, o.total_price, o.shipping_address,
                                          oi.product_id, p.name AS product_name, oi.quantity
                                   FROM orders o
                                   JOIN order_items oi ON o.order_id = oi.order_id
                                   JOIN products p ON oi.product_id = p.product_id
                                   WHERE o.customer_id = @customerId
                                   ORDER BY o.order_date DESC";

                    SqlCommand cmd = new SqlCommand(queryOrders, connection);
                    cmd.Parameters.AddWithValue("@customerId", customerId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        int currentOrderId = -1;
                        string currentOrder = "";

                        while (reader.Read())
                        {
                            int orderId = reader.GetInt32(reader.GetOrdinal("order_id"));

                            if (orderId != currentOrderId)
                            {
                                if (!string.IsNullOrEmpty(currentOrder))
                                {
                                    orderDetails.Add(currentOrder);
                                }

                                currentOrderId = orderId;
                                currentOrder = $"Order ID: {orderId}, Date: {reader["order_date"]}, Total: {reader["total_price"]}, Address: {reader["shipping_address"]}\nProducts:";
                            }

                            currentOrder += $"\n  - {reader["product_name"]} x {reader["quantity"]}";
                        }

                        if (!string.IsNullOrEmpty(currentOrder))
                        {
                            orderDetails.Add(currentOrder);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving orders: " + ex.Message);
            }

            return orderDetails;
        }
        // 10.didplay product via id
        public Product ViewProductById(int productId)
        {
            Product product = null;

            try
            {
                using (SqlConnection connection = DBConnection.GetConnection())
                {
                    string query = "SELECT * FROM products WHERE product_id = @productId";

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@productId", productId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        product = new Product
                        (
                            Convert.ToInt32(reader["product_id"]),
                            reader["name"].ToString(),
                            Convert.ToDecimal(reader["price"]),
                            reader["description"].ToString(),
                            Convert.ToInt32(reader["stock_quantity"])
                        );
                    }
                    else
                    {
                        throw new ProductNotFoundException($"Product not found with id: {productId}");
                    }

                    reader.Close();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
            }

            return product;
        }

    }
}
