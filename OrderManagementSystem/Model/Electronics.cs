using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Model
{
    internal class Electronics:Product
    {
        string _brand;
        int _warrantyPeriod;

        public Electronics()
        {
            
        }
        public Electronics(string product_name,string description,decimal price,int quantity_in_stock,CategoryType c_type,string brand,int warranty_period):base(product_name,description,price,quantity_in_stock,c_type)
        {
            _brand = brand;
            _warrantyPeriod = warranty_period;
        }
    }
}
