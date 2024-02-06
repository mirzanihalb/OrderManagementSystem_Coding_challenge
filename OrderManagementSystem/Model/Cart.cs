using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Model
{
    internal class Cart
    {
        int _cartId;
        int _userId;
        int _productId;
        int _quantity;

        public Cart()
        {

        }
        public Cart(int cart_id, int user_id, int product_id, int quantity)
        {
            _cartId = cart_id;
            _userId = user_id;
            _productId = product_id;
            _quantity = quantity;
        }

        public int CartId
        {
            get { return _cartId; }
            set { _cartId = value; }
        }

        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        public int ProductId
        {
            get { return _productId; }
            set { _productId = value; }
        }

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }
    }
}
