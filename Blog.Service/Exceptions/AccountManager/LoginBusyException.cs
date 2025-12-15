using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Service.Exceptions.AccountManager
{
    internal class LoginBusyException : Exception
    {
        public LoginBusyException(string message) : base (message) { }
    }
}
