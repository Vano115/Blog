using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Service.Exceptions.AccountManager
{
    public class WrongValueException : Exception
    {
        public WrongValueException(string message) : base (message) { }
    }
}
