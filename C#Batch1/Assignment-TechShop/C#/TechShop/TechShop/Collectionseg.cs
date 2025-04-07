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
        public void AddProduct(Products product)
        {
            if (productsList.Any(p => p.ProductName.Equals(product.ProductName, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidDataException("Duplicate product name detected.");

            productsList.Add(product);
            Console.WriteLine("Product added successfully.");
        }
        public void UpdateProduct(int productId, string newName, string newDescription, float newPrice)
        {
            Products productToUpdate = productsList.FirstOrDefault(p => p.ProductID == productId);

            if (productToUpdate == null)
                throw new InvalidDataException("Product not found.");

            if (newPrice < 0)
                throw new InvalidDataException("Price cannot be negative.");

            productToUpdate.ProductName = newName;
            productToUpdate.Description = newDescription;
            productToUpdate.Price = newPrice;

            Console.WriteLine("Product updated successfully.");
        }
        public void RemoveProduct(int productId)
        {
            Products productToRemove = productsList.FirstOrDefault(p => p.ProductID == productId);

            if (productToRemove == null)
                throw new InvalidDataException("Product not found.");

            // Check if product exists in any existing orders
            foreach (var order in ordersList)
            {
                foreach (var detail in orderDetailsList)
                {
                    if (detail.Product.ProductID == productId)
                        throw new InvalidOperationException("Cannot remove product linked to existing orders.");
                }
            }

            productsList.Remove(productToRemove);
            Console.WriteLine("Product removed successfully.");
        }


        public void UpdateOrderStatus(int orderId, string newStatus)
        {
            var order = ordersList.Find(o => o.OrderID == orderId);
            if (order != null)
                order.Status = newStatus;
            else
                throw new IncompleteOrderException("Order not found.");
        }

        public void RemoveCancelledOrders()
        {
            ordersList.RemoveAll(o => o.Status.Equals("Cancelled", StringComparison.OrdinalIgnoreCase));
        }
        public List<Orders> GetOrdersSortedByDate(bool ascending = true)
        {
            return ascending
                ? ordersList.OrderBy(o => o.OrderDate).ToList()
                : ordersList.OrderByDescending(o => o.OrderDate).ToList();
        }

        public List<Orders> GetOrdersByDateRange(DateTime startDate, DateTime endDate)
        {
            return ordersList.Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate).ToList();
        }
        public List<Products> SearchProductsByName(string name)
        {
            return productsList.Where(p => p.ProductName.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<Products> SearchProductsByCategory(string category)
        {
            return productsList.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
        }


    }


}

