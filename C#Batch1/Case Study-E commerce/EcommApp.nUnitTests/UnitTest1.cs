using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using EcommApp.Entity;
using EcommApp.DAO;
using EcommApp.myexceptions;
using System.Collections.Generic;
using static EcommApp.myexceptions.Exceptions;

namespace EcommApp.Tests
{
    public class Tests
    {
        private OrderProcessorRepository service;

        [SetUp]
        public void Setup()
        {
            service = new OrderProcessorRepositoryImpl(); 
        }


        [Test]
        public void TestCreateProduct()
        {
            Product product = new Product(0, "TestProduct", 1000, "Test Description", 100);
            bool result = service.CreateProduct(product);
            ClassicAssert.IsTrue(result);
        }


        [Test]
        public void TestAddProductToCart()
        {
            Customer customer = new Customer(1, "Alice", "alice@abc.com", "alice123"); 
            Product product = new Product(101, "Smartphone", 10000, "atest Android phone", 50);    
            int quantity = 2;

            bool result = service.AddToCart(customer, product, quantity);
            ClassicAssert.IsTrue(result);
        }

        [Test]
        public void TestPlaceOrder()
        {
            Customer customer = new Customer(1, "Alice", "alice@abc.com", "alice123 ");
            Product product = new Product(3, "Tablet", 30000, "Electronics", 8);
            service.AddToCart(customer, product, 1);
            var productQuantityList = new List<KeyValuePair<Product, int>> { new KeyValuePair<Product, int>(product, 1) };
            string shippingAddress = "456 Green Street";

            bool result = service.PlaceOrder(customer, productQuantityList, shippingAddress);

            ClassicAssert.IsTrue(result);
        }

        [Test]
        public void TestOrderNotFoundException()
        {
            int fakeCustomerId = 99999; 

            Assert.Throws<OrderNotFoundException>(() =>
            {
                var orders = service.GetOrdersByCustomer(fakeCustomerId);
            });
        }

    }
}
