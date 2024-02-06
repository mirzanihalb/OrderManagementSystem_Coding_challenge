using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Model
{
    internal class Order
    {
        int _orderId;
        int _userId;
        DateTime _orderDate;
        decimal _totalPrice;
        string _shippingAddress;

        public Order()
        {

        }
        public Order(int order_id, int user_id, DateTime order_date, decimal total_price, string shipping_address)
        {
            _orderId = order_id;
            _userId = user_id;
            _orderDate = order_date;
            _totalPrice = total_price;
            _shippingAddress = shipping_address;
        }

        public int OrderId
        {
            get { return _orderId; }
            set { _orderId = value; }
        }

        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }
        public DateTime OrderDate
        {
            get { return _orderDate; }
            set { _orderDate = value; }
        }
        public decimal TotalPrice
        {
            get { return _totalPrice; }
            set { _totalPrice = value; }
        }
        public string ShippingAddress
        {
            get { return _shippingAddress; }
            set { _shippingAddress = value; }
        }

        public override string ToString()
        {
            return $"OrderId: {OrderId} OrderDate : {OrderDate} TotalPrice : {TotalPrice} Shipping Address : {ShippingAddress}";
        }
    }
}
