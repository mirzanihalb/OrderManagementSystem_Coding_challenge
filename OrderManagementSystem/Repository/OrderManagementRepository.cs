using OrderManagementSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using OrderManagementSystem.Utility;
using System.Data.SqlTypes;
using OrderManagementSystem.Exceptions;

namespace OrderManagementSystem.Repository
{
    
    internal class OrderManagementRepository:IOrderManagementRepository
    {
        SqlConnection sqlconnection = null;
        SqlCommand cmd = null;
        public OrderManagementRepository()
        {
            sqlconnection = new SqlConnection(DbConnUtil.GetConnectionString());
            cmd = new SqlCommand();
        }

        

        public User Login(string username)
        {
            try
            {
                cmd.Parameters.Clear();
                cmd.CommandText = "select * from Users where username = @username";
                cmd.Parameters.AddWithValue("username", username);
                cmd.Connection = sqlconnection;
                sqlconnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                User user_obj = new User();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        user_obj.UserId = (int)reader["user_id"];
                        user_obj.Username = (string)reader["username"];
                        user_obj.Password = (string)reader["password"];
                        if ((string)reader["role"] == "Admin")
                        {
                            user_obj.UserType = User_Type.Admin;
                        }
                        else if ((string)reader["role"] == "User")
                        {
                            user_obj.UserType = User_Type.User;
                        }
                    }
                    return user_obj;
                }
                else
                {
                    //throe error
                    throw new UserNotFound("User Not Found");
                    return null;
                }
            }
            catch (UserNotFound) { Console.WriteLine("user not found");return null; }
            finally
            {
                sqlconnection.Close();
            }
            

        }
        public List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();
            cmd.Parameters.Clear();
            cmd.CommandText = "select * from products";
            cmd.Connection = sqlconnection;
            sqlconnection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Product product_obj = new Product();
                product_obj.ProductId = (int)reader["product_id"];
                product_obj.ProductName = (string)reader["product_name"];
                product_obj.Price = (decimal)reader["price"];
                product_obj.Description = (string)reader["description"];
                product_obj.QuantityInStock = (int)reader["quantity_in_stock"];
                products.Add(product_obj);

            }
            sqlconnection.Close();
            return products;
        }

        public bool AddToCart(User loggedInUser, Product product_obj, int quantity)
        {
            try
            {

                cmd.Parameters.Clear();
                cmd.CommandText = "update cart set quantity = case when not exists (select 1 from cart where user_id=@userId and product_id=@productId) then 1 else quantity+@quantity end where user_id =@userId and product_id = @productId";
                cmd.Connection = sqlconnection;
                cmd.Parameters.AddWithValue("userId", loggedInUser.UserId);
                cmd.Parameters.AddWithValue("productId", product_obj.ProductId);
                cmd.Parameters.AddWithValue("quantity", quantity);
                sqlconnection.Open();
                int cartstatus = cmd.ExecuteNonQuery();
                sqlconnection.Close();
                if (cartstatus > 0)
                {
                    return true;
                }
                else
                {
                    cmd.CommandText = "Insert into Cart(user_id,product_id,quantity) values(@userId,@productId,@quantity)";
                    cmd.Parameters.Clear();
                    cmd.Connection = sqlconnection;
                    cmd.Parameters.AddWithValue("userId", loggedInUser.UserId);
                    cmd.Parameters.AddWithValue("productId", product_obj.ProductId);
                    cmd.Parameters.AddWithValue("quantity", quantity);
                    sqlconnection.Open();
                    cartstatus = cmd.ExecuteNonQuery();
                    sqlconnection.Close();
                    if (cartstatus > 0)
                    {
                        return true;
                    }
                    else { return false; }

                }
            }
            catch (SqlException e) { Console.WriteLine(e.Message); return false; }
            catch (Exception e) { Console.WriteLine(e.Message); return false; }
        }
        public Dictionary<Product, int> GetAllFromCart(User loggedInUser)
        {
            try
            {
                List<Product> products = GetAllProducts();

                Dictionary<Product, int> cartItems = new Dictionary<Product, int>();

                cmd.CommandText = "select * from cart where user_id = @userId";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("userId", loggedInUser.UserId);
                cmd.Connection = sqlconnection;
                sqlconnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Product product_obj = products.Find(x => x.ProductId == (int)reader["product_id"]);

                        int quantity = (int)reader["quantity"];
                        cartItems.Add(product_obj, quantity);

                    }
                }
                else
                {
                    throw new UserNotFound("No user Found");
                }
                sqlconnection.Close();
                return cartItems;
            }
            catch (Exception e) { Console.WriteLine(e.Message); return null; }

        }

        public bool CreateOrder(User loggedInUser, Dictionary<Product, int> cartitems, string? shippingAddress)
        {
            try
            {
                cmd.Parameters.Clear();
                decimal totalPrice = 0;
                DateTime orderDate = DateTime.Now.Date;
                foreach (var item in cartitems)
                {
                    totalPrice += item.Key.Price * item.Value;
                }
                cmd.CommandText = "insert into orders(user_id,order_date,total_price,shipping_address) values(@userId,@orderDate,@totalPrice,@shippingAddress);select SCOPE_IDENTITY();";
                cmd.Parameters.AddWithValue("userId", loggedInUser.UserId);
                cmd.Parameters.AddWithValue("orderDate", orderDate);
                cmd.Parameters.AddWithValue("totalPrice", totalPrice);
                cmd.Parameters.AddWithValue("shippingAddress", shippingAddress);
                cmd.Connection = sqlconnection;
                sqlconnection.Open();
                object scope_identity = cmd.ExecuteScalar();

                sqlconnection.Close();


                if (scope_identity != null)
                {
                    try
                    {
                        int orderIdPlaced = Convert.ToInt32(scope_identity);

                        foreach (var item in cartitems)
                        {
                            cmd.Parameters.Clear();
                            cmd.CommandText = "insert into order_items(order_id,product_id,quantity) values(@orderId,@productId,@quantity)";
                            cmd.Parameters.AddWithValue("orderId", orderIdPlaced);
                            cmd.Parameters.AddWithValue("productId", item.Key.ProductId);
                            cmd.Parameters.AddWithValue("quantity", item.Value);
                            cmd.Connection = sqlconnection;
                            sqlconnection.Open();
                            int order_item_insert_status = cmd.ExecuteNonQuery();
                            sqlconnection.Close();


                        }
                        foreach (var item in cartitems)
                        {
                            cmd.Parameters.Clear();
                            cmd.CommandText = "update products set quantity_in_stock-=@quantity where product_id=@productId";
                            cmd.Parameters.AddWithValue("quantity", item.Value);
                            cmd.Parameters.AddWithValue("productId", item.Key.ProductId);
                            cmd.Connection = sqlconnection;
                            sqlconnection.Open();
                            int updateProductsCountStatus = cmd.ExecuteNonQuery();
                            sqlconnection.Close();

                        }

                        return orderIdPlaced > 0;
                    }//OrderNotFoundException
                    catch (Exception ex)
                    {
                        sqlconnection.Close();
                        Console.WriteLine(ex.Message);
                    }


                }

                return false;


            }
            catch(UserNotFound e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return false; }
        }

        public bool CreateProduct(Product product)
        {
            try
            {
                cmd.Parameters.Clear();
                cmd.CommandText = "insert into Products(product_name,price,description,quantity_in_stock,type) values(@name,@price,@description,@quantityInStock,@type);select SCOPE_IDENTITY();";
                cmd.Parameters.AddWithValue("name", product.ProductName);
                cmd.Parameters.AddWithValue("price", product.Price);
                cmd.Parameters.AddWithValue("description", product.Description);
                cmd.Parameters.AddWithValue("quantityInStock", product.QuantityInStock);
                cmd.Parameters.AddWithValue("type", product.Type.ToString());

                cmd.Connection = sqlconnection;
                sqlconnection.Open();
                object scope_identity = cmd.ExecuteScalar();

                sqlconnection.Close();


                if (scope_identity != null)
                {
                    product.ProductId = Convert.ToInt32(scope_identity);
                    return true;
                }
                return false;
            }
            catch (Exception e) { Console.WriteLine(e.Message); return false; }
        }

        public List<Order> GetOrdersByUser(User loggedInUser)
        {
            try
            {

                List<Order> orders = new List<Order>();
                cmd.Parameters.Clear();
                cmd.CommandText = "select * from orders where user_id = @userId";
                cmd.Connection = sqlconnection;
                cmd.Parameters.AddWithValue("userId", loggedInUser.UserId);
                sqlconnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Order order = new Order();
                    order.OrderId = (int)reader["order_id"];
                    order.UserId = (int)reader["user_id"];
                    order.OrderDate = (DateTime)reader["order_date"];
                    order.TotalPrice = (decimal)reader["total_price"];
                    order.ShippingAddress = (string)reader["shipping_address"];
                    orders.Add(order);
                }

                sqlconnection.Close();
                return orders;
            }
            catch(UserNotFound e) { Console.WriteLine("user Not found"); return null; }
            catch (Exception e) { Console.WriteLine(e.Message); return null; }
        }
        public User CreateUser(User user)
        {
            try
            {
                cmd.Parameters.Clear();
                cmd.CommandText = "insert into Users(username,password,role) values(@username,@password,@role);select SCOPE_IDENTITY();";
                cmd.Parameters.AddWithValue("username", user.Username);
                cmd.Parameters.AddWithValue("password", user.Password);
                cmd.Parameters.AddWithValue("role", user.UserType);
                cmd.Connection = sqlconnection;

                sqlconnection.Open();
                object scopeIdentity = cmd.ExecuteScalar();
                sqlconnection.Close();

                if (scopeIdentity != null)
                {
                    user.UserId = Convert.ToInt32(scopeIdentity);
                    return user;
                }


                return null;
            }
            catch (SqlException ex)
            {
                
                Console.WriteLine($"SQL Exception: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        public bool CancelOrder(User user, int order_id)
        {
            try
            {
                Dictionary<int, int> items_ordered = new Dictionary<int, int>();
                cmd.Parameters.Clear();

                cmd.CommandText = "select * from Order_items where order_id = @orderID";
                cmd.Parameters.AddWithValue("orderID", order_id);



                cmd.Connection = sqlconnection;
                sqlconnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {


                        int product_id = (int)reader["product_id"];
                        int quantity = (int)reader["quantity"];
                        items_ordered.Add(product_id, quantity);

                    }
                }
                else
                {
                    throw new OrderNotFound("order Not found");
                }
                sqlconnection.Close();
                int flag = 0;
                foreach (var item in items_ordered)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "update Products set quantity_in_stock = quantity_in_stock+@quantity where product_id = @productId ";
                    cmd.Connection = sqlconnection;

                    cmd.Parameters.AddWithValue("productId", item.Key);
                    cmd.Parameters.AddWithValue("quantity", item.Value);
                    sqlconnection.Open();
                    int status = cmd.ExecuteNonQuery();
                    sqlconnection.Close();
                    flag = 1;
                }
                if (flag == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                sqlconnection.Close();
            }
            


        }
    }
}
