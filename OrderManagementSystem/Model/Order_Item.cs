using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Model
{
    internal class Order_Item
    {
        int _orderItemId;
        int _orderId;
        int _productId;
        int _quantity;


        public Order_Item()
        {

        }
        public Order_Item(int order_item_id, int order_id, int product_id, int quantity)
        {
            _orderItemId = order_item_id;
            _orderId = order_id;
            _productId = product_id;
            _quantity = quantity;
        }

        public int OrderItemId
        {
            get { return _orderItemId; }
            set { _orderItemId = value; }
        }

        public int OrderId
        {
            get { return _orderId; }
            set { _orderId = value; }
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
