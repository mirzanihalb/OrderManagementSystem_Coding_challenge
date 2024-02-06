using OrderManagementSystem.Model;
using OrderManagementSystem.Service;

namespace OrderManagementSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IOrderManagementService Service = new OrderManagementServiceImpl();

            User loggedInUser = null;


            while (loggedInUser == null)
            {
                Console.WriteLine("Welcome To Application");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Register");
                Console.WriteLine("0. Exit");
                int userInput = int.Parse(Console.ReadLine());
                switch (userInput)
                {
                    case 0: return;
                    case 1: loggedInUser = Service.Login(); break;
                    case 2: loggedInUser = Service.CreateUser(); break;
                    default: Console.WriteLine("choose correct input"); break;
                }
            }


            while (loggedInUser != null)
            {

                Console.WriteLine("1 > View All Products To Shop");
                Console.WriteLine("2 > Add To Cart");
                
               
                Console.WriteLine("3 > Place Order");
                Console.WriteLine("4 > View Your Orders");
                Console.WriteLine("5 > cancel Order");



                if (loggedInUser.UserType != User_Type.User)
                {
                    Console.WriteLine("6 > Create customer");
                    
                    Console.WriteLine("7 > Create Product");
                    
                }
                Console.WriteLine("0 > Exit");
                int userInput = int.Parse(Console.ReadLine());
                switch (userInput)
                {
                    case 0: Console.WriteLine("Thanks For Visiting"); return;
                    case 1: Service.GetAllProducts(); break;
                    case 2: Service.AddToCart(); break;
                    case 3: Service.CreateOrder(); break;
                    case 4: Service.GetOrdersByUser(); break;
                    case 5: Service.CancelOrder(); break;
                    case 6: User user_created  = Service.CreateUser(); break;

                    case 7: Service.CreateProduct(); break;
                    
                    default: Console.WriteLine("Enter Correct Option"); break;
                }
            }
        }
    }
}
