using OrderManagementSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Repository
{
    internal interface IOrderManagementRepository
    {
        public User Login(string username);

        public List<Product> GetAllProducts();

        bool AddToCart(User loggedInUser, Product product_obj, int quantity);
        public Dictionary<Product, int> GetAllFromCart(User loggedInUser);

        public bool CreateOrder(User loggedInUser, Dictionary<Product, int> cartitems, string? shippingAddress);

        public bool CreateProduct(Product obj);

        public User CreateUser(User user);

        public List<Order> GetOrdersByUser(User loggedInUser);

        public bool CancelOrder(User user, int order_id);
    }
}
