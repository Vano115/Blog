using Blog.Data.Entityes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Data.Models.BlogModels
{
    public class ReadArticleViewModel
    {
        public required string Title { get; set; }

        public required string Text { get; set; }

        public List<Tag> Tags { get; set; } = new List<Tag>();

        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
