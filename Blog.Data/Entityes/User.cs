using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Entityes
{
    public class User : IdentityUser
    {
        public required string Nickname { get; set; }

        public string ProfileImage { get; set; } = string.Empty;

        public List<string> Images { get; set; } = new List<string>();

        public List<Comment> Comments { get; set; } = [];

        public List<Article> Articles { get; set; } = [];

        public User() { }

        
    }
}
