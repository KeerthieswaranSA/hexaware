using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommApp.Entity
{
    public class Cart
    {
        private int cartId;
        private int customerId;
        private int productId;
        private int quantity;
        public Cart() { }
        public Cart(int cartId, int customerId, int productId, int quantity)
        {
            this.cartId = cartId;
            this.customerId = customerId;
            this.productId = productId;
            this.quantity = quantity;
        }
        public int CartId { get => cartId; set => cartId = value; }
        public int CustomerId { get => customerId; set => customerId = value; }
        public int ProductId { get => productId; set => productId = value; }
        public int Quantity { get => quantity; set => quantity = value; }
        public Product Product { get; set; }
    }
}
