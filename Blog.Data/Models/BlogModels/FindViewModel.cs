using System;
using System.Collections.Generic;
using System.Text;
using Blog.Data.Entityes;
using Blog.Data.Models.UserModels;

namespace Blog.Data.Models.BlogModels
{
    public class FindViewModel
    {
        public List<ViewUserModel> Users { get; set; } = new List<ViewUserModel>();

        public List<Article> Articles { get; set; } = new List<Article>();


    }
}
