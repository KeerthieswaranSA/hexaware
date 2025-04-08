using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechShop
{
    class Collectionseg
    {
        private List<Products> productsList = new();
        private List<Orders> ordersList = new();
        private List<OrderDetails> orderDetailsList = new List<OrderDetails>();
        private SortedList<int, Inventory> inventoryList = new();
        private List<Payment> paymentRecords = new List<Payment>();
        public void AddProduct(Products product)
        {
            if (productsList.Any(p => p.ProductName.Equals(product.ProductName, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidDataException("Duplicate product name detected.");

            productsList.Add(product);
            Console.WriteLine($"Product {product.ProductName} added successfully.");
        }
        public void UpdateProduct(int productId, string newName, string newDescription, float newPrice)
        {
            try
            {
                Products productToUpdate = productsList.FirstOrDefault(p => p.ProductID == productId);
                if (productToUpdate == null)
                {
                    throw new InvalidDataException("Product not found.");
                    
                }
                if (newPrice < 0)
                {
                    throw new InvalidDataException("Price cannot be negative.");
                    
                }

                productToUpdate.ProductName = newName;
                productToUpdate.Description = newDescription;
                productToUpdate.Price = newPrice;
                Console.WriteLine($"Product {newName} updated successfully.");
            }
            catch(InvalidDataException ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void RemoveProduct(int productId)
        {
            Products productToRemove = productsList.FirstOrDefault(p => p.ProductID == productId);
            if (productToRemove == null)
                throw new InvalidDataException("Product not found.");
            foreach (var order in ordersList)
            {
                foreach (var detail in orderDetailsList)
                {
                    if (detail.Product.ProductID == productId)
                        throw new InvalidOperationException("Cannot remove product linked to existing orders.");
                }
            }
            productsList.Remove(productToRemove);
            RemoveInventory(productId);
            Console.WriteLine("Product removed successfully.");
        }
        public void UpdateStock(int productId, int quantity)
        {
            if (inventoryList.ContainsKey(productId))
            {
                inventoryList[productId].AddToInventory(quantity);
                Console.WriteLine($"Stock updated for Product ID {productId}, New quantity: {inventoryList[productId].StockInQuantity}");
            }
            else
            {
                Console.WriteLine("Product not found in inventory.");
            }
        }
        public void AddToInventory(int productId, Products product, int quantity)
        {
            if (!inventoryList.ContainsKey(productId))
            {
                inventoryList[productId] = new Inventory(productId, product, quantity, DateTime.Now);
                Console.WriteLine($"Inventory added for Product ID {productId}");
            }
            else
            {
                inventoryList[productId].AddToInventory(quantity);
                Console.WriteLine($"Inventory updated for Product ID {productId}");
            }
        }
        public void RemoveInventory(int productId)
        {
            if (inventoryList.ContainsKey(productId))
            {
                inventoryList.Remove(productId);
                Console.WriteLine($"Inventory for Product ID {productId} removed successfully.");
            }
            else
            {
                Console.WriteLine("Product not found in inventory.");
            }
        }
        public void DisplayInventory()
        {
            Console.WriteLine("Current Inventory:");
            foreach (var inv in inventoryList.Values)
            {
                inv.DisplayInventory();
            }
        }
        public void AddOrder(Orders order)
        {
            ordersList.Add(order);
            Console.WriteLine($"Order {order.OrderID} added successfully.");
        }
        public void UpdateOrderStatus(int orderId, string newStatus)
        {
            try
            {
                var order = ordersList.Find(o => o.OrderID == orderId);
                if (order != null)
                {
                    order.Status = newStatus;
                    Console.WriteLine($"Order {orderId} status updated to: {newStatus}");
                }
                else
                    throw new IncompleteOrderException("Order not found.");
            }
            catch(IncompleteOrderException ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }

        public List<Orders> GetOrdersByDateRange(DateTime startDate, DateTime endDate)
        {
            return ordersList
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .OrderBy(o => o.OrderDate).ToList();
            
        }
        public void DisplayOrderDetailsByDateRange(DateTime startDate, DateTime endDate)
        {
            var filteredOrders = GetOrdersByDateRange(startDate, endDate);

            if (filteredOrders.Count == 0)
            {
                Console.WriteLine("No orders found in the given date range.");
            }
            else
            {
                Console.WriteLine("Orders in the given date range:\n");

                foreach (var order in filteredOrders)
                {
                    Console.WriteLine($"Order ID: {order.OrderID}");
                    Console.WriteLine($"Customer Name: {order.Customer.FirstName}");
                    Console.WriteLine($"Order Date: {order.OrderDate.ToShortDateString()}");
                    Console.WriteLine($"Total Amount: {order.TotalAmount}");
                    Console.WriteLine($"Status: {order.Status}");
                }
            }
        }
        public void SearchProductsByName(string name)
        {
            
            var results = productsList
                .Where(p => p.ProductName.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (results.Count == 0)
            {
                Console.WriteLine("No products found.");
            }
            else
            {
                Console.WriteLine("Products Details");
                foreach (var p in results)
                {
                    Console.WriteLine($"ID: {p.ProductID}, Name: {p.ProductName}, Price: {p.Price}, Category: {p.Category}");
                }
            }
        }

        public void SearchProductsByCategory(string category)
        {
            
            var results = productsList
                .Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (results.Count == 0)
            {
                Console.WriteLine("No products found in this category.");
            }
            else
            {
                Console.WriteLine($"Products in Category '{category}'...");
                foreach (var p in results)
                {
                    Console.WriteLine($"ID: {p.ProductID}, Name: {p.ProductName}, Price: {p.Price}");
                }
            }
        }
        

        public void RecordPayment(Payment payment)
        {
            var order = ordersList.FirstOrDefault(o => o.OrderID == payment.OrderId);
            if (order == null)
            {
                Console.WriteLine("Order not found. Cannot record payment.");
                return;
            }

            paymentRecords.Add(payment);
            Console.WriteLine("Payment recorded successfully.");
        }

        public void UpdatePaymentStatus(int paymentId, string newStatus)
        {
            var payment = paymentRecords.FirstOrDefault(p => p.PaymentId == paymentId);
            if (payment == null)
            {
                Console.WriteLine("Payment not found.");
                return;
            }

            payment.PaymentStatus = newStatus;
            Console.WriteLine($"Payment status updated to: {newStatus}");
        }

        public void DisplayAllPayments()
        {
            Console.WriteLine("Payment Records:");
            foreach (var p in paymentRecords)
            {
                p.DisplayPayment();
            }
        }
    }


}

