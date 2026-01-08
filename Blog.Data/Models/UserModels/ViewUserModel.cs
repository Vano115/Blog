using Blog.Data.Entityes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Data.Models.UserModels
{
    public class ViewUserModel
    {
        public string UserName { get; set; } = string.Empty;

        public string ProfileImage { get; set; } = string.Empty;

        public List<string> Images { get; set; } = new List<string>();

        public List<Article> Articles { get; set; } = new List<Article>();
    }
}
