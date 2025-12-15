using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Service.Exceptions.AccountManager
{
    internal class UserNotFoundException : Exception
    {
        public UserNotFoundException(string message) : base(message) { }
    }
}
