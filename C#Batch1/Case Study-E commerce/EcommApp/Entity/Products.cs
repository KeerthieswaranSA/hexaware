using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommApp.Entity
{
    public class Product
    {
        private int productId;
        private string name;
        private decimal price;
        private string description;
        private int stockQuantity;
        public Product() { }
        public Product(int productId, string name, decimal price, string description, int stockQuantity)
        {
            this.productId = productId;
            this.name = name;
            this.price = price;
            this.description = description;
            this.stockQuantity = stockQuantity;
        }
        public int ProductId { get => productId; set => productId = value; }
        public string Name { get => name; set => name = value; }
        public decimal Price { get => price; set => price = value; }
        public string Description { get => description; set => description = value; }
        public int StockQuantity { get => stockQuantity; set => stockQuantity = value; }
    }
}
