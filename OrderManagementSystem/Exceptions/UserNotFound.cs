using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Exceptions
{
    internal class UserNotFound:ApplicationException
    {
        public UserNotFound()
        {
            
        }
        public UserNotFound(string msg):base(msg)
        {
            
        }
    }
}
