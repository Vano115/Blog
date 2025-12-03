using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Entityes
{
    public class Article
    {
        public int Id { get; set; }

        public required string Title { get; set; }

        public required string Text { get; set; }

        public List<Tag> Tags { get; set; } = [];

        public List<Comment> Comments { get; set; } = [];

        public DateTime PublicDate { get; set; } = DateTime.UtcNow;

        public int Likes { get; set; } = 0;

        public required User Owner { get; set; }

    }
}
