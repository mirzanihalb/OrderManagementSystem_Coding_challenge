using OrderManagementSystem.Model;
using OrderManagementSystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Service
{
    internal class OrderManagementServiceImpl : IOrderManagementService
    {
        User loggedInUser;

        readonly IOrderManagementRepository repository;

        public OrderManagementServiceImpl()
        {
            repository = new OrderManagementRepository();
            loggedInUser = null;
        }
        public User Login()
        {
            try
            {
                Console.Write("Enter the Username");
                string username = Console.ReadLine();
                Console.WriteLine();
                Console.Write("Enter password");
                string password = Console.ReadLine();

                User user_obj = repository.Login(username);
                if (user_obj != null)
                {
                    if (user_obj.Password == password)
                    {
                        loggedInUser = user_obj;
                        Console.WriteLine("Log In Successfull");
                        return user_obj;
                    }
                    else
                    {
                        Console.WriteLine("please check password");
                        return null;
                    }

                }
                else
                {
                    Console.WriteLine("No User Found");
                    return null;
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return null; }

        }
        public void CancelOrder()
        {
            List<Order> orders = repository.GetOrdersByUser(loggedInUser);
            foreach (Order order in orders)
            {
                Console.WriteLine(order);
            }
            Console.WriteLine("Enter the order Id to delelte");
            int orderId = int.Parse(Console.ReadLine());
            bool status = repository.CancelOrder(loggedInUser, orderId);
            if (status)
            {
                Console.WriteLine("Successfully cancelled the order");
            }
            else
            {
                Console.WriteLine("Please try again");
            }
        }

        public void CreateOrder()
        {
            Dictionary<Product, int> cart_items = repository.GetAllFromCart(loggedInUser);
            Console.WriteLine("Enter the Shipping Address");
            string shippingAddress = Console.ReadLine();
            int flag = 0;
            foreach (var item in cart_items)
            {

                if (item.Key.QuantityInStock < item.Value)
                {
                    flag = 1;
                    Console.WriteLine($"This Product: {item.Key.ProductName} has stockQuantity{item.Key.QuantityInStock}");
                    Console.WriteLine("Please add number of items accordingly and then place order");
                }
            }
            if (flag == 0)
            {
                bool placeOrderStatus = repository.CreateOrder(loggedInUser, cart_items, shippingAddress);

                if (placeOrderStatus)
                {
                    Console.WriteLine("order Placed successfully");
                }
                else
                {
                    Console.WriteLine("Try Again to Order");
                }
            }
        }
        //catch(OrderNotFoundException ex) { Console.WriteLine(ex.Message); }
        //catch (Exception ex) { Console.WriteLine(ex.Message); }





        public void CreateProduct()
        {
            try
            {
                Console.WriteLine("Enter Product Name : ");
                string productName = Console.ReadLine();

                Console.WriteLine("Enter Product Price : ");
                decimal productPrice = decimal.Parse(Console.ReadLine());

                Console.WriteLine("Enter Product Description : ");
                string productDescrition = Console.ReadLine();

                Console.WriteLine("Enter Product Stock Quantity : ");
                int productStockQuantity = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter Product Type[Electronics/Clothing] : ");
                string ptype = Console.ReadLine();
                Product product_obj = null;
                if (ptype == "Electronics")
                {
                    Console.WriteLine("Enter brand : ");
                    string brand = Console.ReadLine();

                    Console.WriteLine("Enter Warranty period : ");
                    int warranty = int.Parse(Console.ReadLine());

                    product_obj = new Electronics(productName, productDescrition, productPrice, productStockQuantity, CategoryType.Electronics, brand, warranty);
                }
                else if ((ptype == "Electronics"))
                {

                    Console.WriteLine("Enter size : ");
                    string size = Console.ReadLine();

                    Console.WriteLine("Enter color : ");
                    string color = Console.ReadLine();

                    product_obj = new Clothing(productName, productDescrition, productPrice, productStockQuantity, CategoryType.Clothing, size, color);

                }



                repository.CreateProduct(product_obj);


            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        public User CreateUser()
        {
            try
            {
                Console.WriteLine("Enter UserName");
                string UserName = Console.ReadLine();



                Console.WriteLine("Enter Password");
                string UserPassword = Console.ReadLine();

                User_Type userrole = User_Type.User;
                if (loggedInUser!=null && loggedInUser.UserType == User_Type.Admin)
                {
                    Console.WriteLine("Enter role");
                    string role = Console.ReadLine();
                    if (role == "Admin")
                    {
                        userrole = User_Type.Admin;
                    }
                    else if (role == "Customer")
                    {
                        userrole = User_Type.User;
                    }
                }


                User user= new User(UserName, UserPassword, userrole);

                User userObj = repository.CreateUser(user);

                if (userObj != null)
                {
                    Console.WriteLine("User created Successfully");
                    return userObj;
                    
                }

                Console.WriteLine("Failed to create user. Please try again.");
                return null;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
                
            }
        }
    
    

        public void GetAllProducts()
        {
            List<Product> products = new List<Product>();
            products = repository.GetAllProducts();
            foreach(Product product in products)
            {
                Console.WriteLine(product);
            }
            
        }

        

        public void AddToCart()
        {

            try
            {
                List<Product> products = new List<Product>();
                products = repository.GetAllProducts();
                foreach(Product product in products)
                {
                    Console.WriteLine(product);
                }
                Console.WriteLine("Enter the Product Id to order");
                int product_id = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter quantity to order");
                int quantity = int.Parse(Console.ReadLine());
                Product obj = products.Find(x=>x.ProductId == product_id);

                bool statius  = repository.AddToCart(loggedInUser,obj,quantity);

                if(statius)
                {
                    Console.WriteLine("successfully placed");
                }
                else
                {
                    Console.WriteLine("try again");
                }
                
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        public void GetOrdersByUser()
        {
            try
            {
                
                List<Order> customerOrdersList = new List<Order>();
                customerOrdersList = repository.GetOrdersByUser(loggedInUser);
                
                foreach (var item in customerOrdersList)
                {
                    Console.WriteLine(item.ToString());
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
