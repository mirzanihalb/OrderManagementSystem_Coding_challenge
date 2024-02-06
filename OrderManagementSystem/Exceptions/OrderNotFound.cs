using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Exceptions
{
    internal class OrderNotFound:ApplicationException
    {
        public OrderNotFound()
        {
            
        }
        public OrderNotFound(string message):base(message)
        {
            
        }
    }
}
