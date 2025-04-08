using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechShop
{
    public class Payment
    {
            public int PaymentId { get; set; }
            public int OrderId { get; set; }
            public float Amount { get; set; }
            public string PaymentMethod { get; set; }
            public string PaymentStatus { get; set; }

            public Payment(int paymentId, int orderId, float amount, string method, string status)
            {
                PaymentId = paymentId;
                OrderId = orderId;
                Amount = amount;
                PaymentMethod = method;
                PaymentStatus = status;
            }

            public void DisplayPayment()
            {
                Console.WriteLine($"PaymentID: {PaymentId}, OrderID: {OrderId}, Amount: {Amount}, Method: {PaymentMethod}, Status: {PaymentStatus}");
            }
        
    }
}
