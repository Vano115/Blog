using Blog.Data.Entityes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Data.Models.BlogModels
{
    public class ArticleViewModel
    {
        public required string Title { get; set; }

        public required string Text { get; set; }

        public string Tags { get; set; } = string.Empty;

    }
}
