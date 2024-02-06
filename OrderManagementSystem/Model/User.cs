using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Model
{
    public enum User_Type{
        Admin,
        User
    }
    internal class User
    {
        int _userId;
        string _username;
        string _password;
        User_Type _userType;

        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }
        
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public User_Type UserType
        {
            get { return _userType; }
            set { _userType = value; }
        }


        public User()
        {

        }
        public User(string username, string User_password,User_Type user_type)
        {

            
            _username = username;
            _password = User_password;
            _userType = user_type;
        }

        public override string ToString()
        {
            return $"UserId: {UserId}\tName: {Username}\t ";
        }
    }
}
