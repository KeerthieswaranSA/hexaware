using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechShop
{
    public class Products
    {
        private int productID;
        private string productName;
        private string description;
        private float  price;
        private string category;

        public Products(int productid, string productname, string description, float price,string category)
        {
            ProductID = productid;
            ProductName = productname;
            Description = description;
            Price = price;
            Category = category;
        }
        public int ProductID { get => productID; set => productID = value; }
        public string ProductName { get => productName; set => productName = value; }
        public string Description { get => description; set => description = value; }
        public float Price
        {
            get => price;
            set
            {
                if (value < 0)
                    throw new InvalidDataException("Price cannot be negative.");
                price = value;
            }
        }
        

        public string Category
        {
            get => category;
            set => category = value;
        }
        public void GetProductDetails() 
        {
            Console.WriteLine("======Product Details======");
            Console.WriteLine("Product Id: "+ ProductID);
            Console.WriteLine("Product Name: "+ProductName);
            Console.WriteLine("Product Description: "+ Description);
            Console.WriteLine("Product Price: "+Price);
        }
        public void UpdateProductInfo(float newprice,string newdescription) 
        { 
            price = newprice;
            description = newdescription;
            Console.WriteLine("Product Info Updated Successfully...");
        }
        
        public void IsProductInStock(Inventory inventory)
        {
            Console.WriteLine($"Product Name: {ProductName}");
            if (inventory.StockInQuantity > 0)
                Console.WriteLine($"{ProductName} is in stock.");
            else
                Console.WriteLine($"{ProductName} is out of stock.");

        }
       
    }
}
