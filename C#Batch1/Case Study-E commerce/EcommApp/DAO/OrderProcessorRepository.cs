using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcommApp.Entity;

namespace EcommApp.DAO
{
    public interface OrderProcessorRepository
    {
        // 1. Create a new product
        bool CreateProduct(Product product);

        // 2. Register a new customer
        bool CreateCustomer(Customer customer);

        // 3. Delete a product by productId
        bool DeleteProduct(int productId);

        // 4. Delete a customer by customerId
        bool DeleteCustomer(int customerId);

        // 5. Add product to cart
        bool AddToCart(Customer customer, Product product, int quantity);

        // 6. Remove product from cart
        bool RemoveFromCart(Customer customer, Product product);

        // 7. Get all products from cart
        List<Product> GetAllFromCart(Customer customer);

        // 8. Place an order (adds to orders and order_items)
        bool PlaceOrder(Customer customer, List<KeyValuePair<Product, int>> productQuantityList, string shippingAddress);

        // 9. Get orders by customerId
        List<string> GetOrdersByCustomer(int customerId);
        Product ViewProductById(int productId);
    }
}
