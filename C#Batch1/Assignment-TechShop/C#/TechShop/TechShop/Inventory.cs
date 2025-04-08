using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechShop
{
    public class Inventory
    {
        private int inventoryID;
        private Products product;
        private int stockInQuantity;
        private DateTime lastStockUpdate;
        public int InventoryID { get => inventoryID; set => inventoryID = value; }
        public Products Product { get => product; set => product = value; }
        public int StockInQuantity { get => stockInQuantity; set => stockInQuantity = value; }
        public DateTime LastStockUpdate { get => lastStockUpdate; set => lastStockUpdate = value; }

        public Inventory(int inventoryID, Products products, int stockInQuantity, DateTime lastStockUpdate)
        {
            InventoryID = inventoryID;
            Product = products;
            StockInQuantity = stockInQuantity;
            LastStockUpdate = lastStockUpdate;
        }

        public Products GetProduct() 
        { 
            return Product;
        }
        public int GetQuantityInStock()
        {
            Console.WriteLine($"Quantity in Stock Product{Product.ProductName} is {StockInQuantity}");
            return StockInQuantity;
        }
        public void AddToInventory(int quantity) 
        {
            StockInQuantity += quantity;
        }
        public void RemoveFromInventory(int quantity) 
        {
            try
            {
                Console.WriteLine("Reducing Quantity From Inventory");
                if (quantity > StockInQuantity)
                    throw new InsufficientStockException("Insufficient stock to fulfill the request.");
                StockInQuantity -= quantity;
            }
            catch(InsufficientStockException ex)
            {
                Console.WriteLine(ex.Message );
            }
        }
        public void UpdateStockQuantity(int newQuantity)
        {
            StockInQuantity = newQuantity;
        }
        public bool IsProductAvailable(int quantityToCheck) 
        {
            return StockInQuantity >= quantityToCheck;
        }
        public void DisplayInventory()
        {
            Console.WriteLine($"Product ID: {Product.ProductID}, Name: {Product.ProductName}, Quantity: {StockInQuantity}");
        }

        public float GetInventoryValue() 
        {
            return Product.Price * StockInQuantity;
        }
        public void ListLowStockProducts(int threshold) 
        {
            if (StockInQuantity < threshold)
                Console.WriteLine($"{Product.ProductName} is below threshold with {StockInQuantity} in stock.");
        }
        public void ListOutOfStockProducts() 
        {
            if (StockInQuantity == 0)
            {
                Console.WriteLine($"Out of Stock: {Product.ProductName}");
            }
        }
        public void ListAllProducts() 
        {
            {
                Console.WriteLine($"Product: {Product.ProductName}, Stock: {StockInQuantity}, Price: {Product.Price}");
            }
        }
    }
}
