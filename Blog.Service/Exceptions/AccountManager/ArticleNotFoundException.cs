using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Service.Exceptions.AccountManager
{
    public class ArticleNotFoundException : Exception
    {
        public ArticleNotFoundException(string message) : base(message) { }
    }
}
