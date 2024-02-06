using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Model
{
    public enum CategoryType
    {
        Electronics,
        Clothing
    }
    public class Product
    {
        int _productId;
        string _product_name;
        decimal _price;
        string _description;
        int _quantityInStock;
        CategoryType type;

        public Product()
        {

        }
        public Product(string product_name, string description, decimal price, int stock_quantity,CategoryType type)
        {
            
            _product_name = product_name;
            _price = price;
            _description = description;
            _quantityInStock = stock_quantity;
            this.type = type;

        }

        public int ProductId
        {
            get { return _productId; }
            set { _productId = value; }
        }

        public string ProductName
        {
            get { return _product_name; }
            set { _product_name = value; }
        }

        public decimal Price
        {
            get { return _price; }
            set { _price = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public int QuantityInStock
        {
            get { return _quantityInStock; }
            set { _quantityInStock = value; }
        }


        public CategoryType Type
        {
            get { return Type; }
            set { type = value; }
        }

        public override string ToString()
        {
            return $"ProductId : {ProductId} Name : {ProductName} Price : {Price} StockQuantity : {QuantityInStock} Description : {Description}";
        }
    }
}
