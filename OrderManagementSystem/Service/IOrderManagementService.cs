using OrderManagementSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Service
{
    internal interface IOrderManagementService
    {
        public User Login();
        public void CreateOrder();

        public void CancelOrder();

        public void CreateProduct();

        public User CreateUser();

        public void GetAllProducts();

        public void GetOrdersByUser();

        public void AddToCart();

    }
}
