using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Service.Exceptions.AccountManager
{
    public class UserNotCreatedException : Exception
    {
        public IEnumerable<IdentityError> Errors;
        public UserNotCreatedException(IEnumerable<IdentityError> errors) {
            Errors = errors;
        }
    }
}
