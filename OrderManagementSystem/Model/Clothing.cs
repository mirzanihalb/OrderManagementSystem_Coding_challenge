using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Model
{
    internal class Clothing : Product
    {
        string _size;
        string _color;

        public Clothing()
        {

        }
        public Clothing(string product_name, string description, decimal price, int quantity_in_stock, CategoryType c_type, string size, string? color) : base(product_name, description, price, quantity_in_stock, c_type)
        {
            _size = size;
            _color = color;
        }
    }
}
